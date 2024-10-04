using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCtrl : MonoBehaviour
{
    public Transform shootTrans;
    public BaseAttack attack;
    private GameObject _bulletPrefab;
    private float t = 0;
    private float ShootInterval;
    private bool isAutomatic;
    private int curAmmo;
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
    private PlayerInfo playerInfo;
    private void Start() {
        /*配置attack*/
        ShootInterval = 1.0f / attack.ShootSpeed;
        isAutomatic = attack.isAutomatic;
        maxAmmo = attack.AmmoAmount;
        curAmmo = maxAmmo;
        playerInfo = GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
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
                    if (Input.GetMouseButtonDown(0))
                    {
                        Shoot();
                        t = ShootInterval;
                    }
                }
            }
        }
       
        
    }

    void Shoot()
    {
        curAmmo -= ammoCost;
        if (curAmmo < 0)
        {
            //触发换弹
            curAmmo = 0;
            Reloding();
        }
        else
        {
            attack.Attack(shootTrans.position, shootTrans.rotation);
        }
        playerInfo.SetAmmoAmount(curAmmo);
        //_bulletPrefab = Instantiate(BulletPrefab,shootTrans.position,transform.rotation);
        //TODO: 设置子弹参数
        //TODO: 播放音效
    }

    void Reloding()
    {
        if(isReloading == true) { return; }
        isReloading = true;
    }
}
