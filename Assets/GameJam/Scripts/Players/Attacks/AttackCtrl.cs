using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCtrl : MonoBehaviour
{
    public Transform shootTrans;
    private GameObject _bulletPrefab;
    private float t = 0;
    private float ShootInterval;
    private bool isAutomatic;
    private int _curAmmo;
    private int curAmmo
    {
        get
        {
            return _curAmmo;
        }
        set
        {
            _curAmmo = value;
            playerInfo.SetAmmoAmount(curAmmo);
        }
    }
    private int maxAmmo;
    private int ammoCost
    {
        get
        {
            return attack.AmmoCost;
        }
    }
    private float reloadingTime
    {
        get
        {
            return attack.ReloadingTime;
        }
    }
    private float reloadingTimer;
    private bool isReloading;
    private PlayerInfo _playerInfo;
    private PlayerInfo playerInfo
    {
        get
        {
            if (_playerInfo == null)
            {
                _playerInfo = GetComponent<PlayerInfo>();
            }
            return _playerInfo;
        }
    }
    private Move move;
    private BaseAttack attack
    {
        get { return playerInfo.attack; }
    }


    //skill
    private Animator anim;
    //private Skill spinSkill;
    private bool isSpining;
    private const float SPIN_SHOOT_COF = 0.75f; //spin射速 = 原来的射速 * cof
    private void Start() {
        /*配置attack*/
        ShootInterval = 1.0f / playerInfo.playerPropertes.shotSpeed;
        isAutomatic = attack.isAutomatic;
        maxAmmo = playerInfo.playerPropertes.maxAmmo;
        curAmmo = maxAmmo;
        move = GetComponent<Move>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reloading();
        }
        if (isReloading)
        {
            reloadingTimer += Time.deltaTime;
            playerInfo.SetReloadingBarValue(reloadingTimer / reloadingTime);
            if (reloadingTimer >= reloadingTime)
            {
                playerInfo.SetReloadingBarValue(0f);
                reloadingTimer = 0.0f;
                isReloading = false;
                curAmmo = maxAmmo;
            }
        }
        else
        {
            if(!isSpining)
                if (isAutomatic)
                {
                    if (t >= 0)
                    {
                        t -= Time.deltaTime;
                    }
                    else
                    {
                        Shoot();
                        t = ShootInterval;
                    }
                }
                else
                {
                    if (t >= 0)
                    {
                        t -= Time.deltaTime;
                    }
                    else
                    {
                        if (Input.GetMouseButton(0))
                        {
                            Shoot();
                            t = ShootInterval;
                        }
                    }
                }
        }

        if (Input.GetMouseButtonDown(1) && !isReloading && !isSpining)
        {
            ReleaseSkill();
        }
    }

    void Shoot()
    {
        attack.Attack(shootTrans.position, shootTrans.rotation);
        curAmmo -= ammoCost;
        if (curAmmo <= 0)
        {
            //触发换弹
            curAmmo = 0;
            Reloading();
        }
        
        //_bulletPrefab = Instantiate(BulletPrefab,shootTrans.position,transform.rotation);
        //TODO: 设置子弹参数
        //TODO: 播放音效
    }

    void Reloading()
    {
        if(isReloading == true) { return; }
        isReloading = true;
    }

    void ReleaseSkill()
    {
        anim.SetBool("spin",true);
        anim.SetTrigger("spinTrigger");
        move.SetGunActive(0);
        isSpining = true;
        StartCoroutine(SpinShooting());
        
        
    }

    IEnumerator SpinShooting()
    {
        float interval = ShootInterval * SPIN_SHOOT_COF;
        //360°随机选取一个角度射击
        float angle = Random.Range(0, 360f);
        Quaternion quaternion = Quaternion.identity * Quaternion.Euler(0, 0, angle);

        while (curAmmo >= 0)
        {
            curAmmo -= ammoCost;
            if (curAmmo < 0)
            {
                //触发换弹
                curAmmo = 0;
                Reloading();
                break;
            }
            else
            {
                attack.Attack(shootTrans.position, quaternion);
            }
            yield return new WaitForSeconds(interval);
            angle = Random.Range(0, 360f);
            quaternion = Quaternion.identity * Quaternion.Euler(0, 0, angle);
        }
        
        anim.SetBool("spin", false);
        move.SetGunActive(1);
        yield return new WaitForSeconds(0.05f);
        
        isSpining = false;
    }

    //失败的封装
    public void UpdateShotSpeed()
    {
        ShootInterval = 1.0f / playerInfo.playerPropertes.shotSpeed;
    }

    public void UpdateMaxAmmo()
    {
        maxAmmo= playerInfo.playerPropertes.maxAmmo;
    }
}
