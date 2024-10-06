using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public class PlayerPropertes
{
    public int Strength;
    public int health;
    public int maxHealth;
    public float shotSpeed;
    public int maxAmmo;
}

public class PlayerInfo : MonoBehaviour,IDamageable
{
    public PlayerPropertes playerPropertes;
    public BaseAttack attack;
    public int health{get;set;}
    public TMP_Text HPtext;
    public TMP_Text Ammotext;
    public Image reloadingImage;
    private bool invicible;

    private void Start() {
        health = playerPropertes.maxHealth;
        playerPropertes.shotSpeed = attack.ShootSpeed;
        playerPropertes.maxAmmo = attack.AmmoAmount;
    }

    public void GetDamaged(int _damage)
    {
        if(invicible) return;
        health -= _damage;
        if(health <= 0)
        {
            MessageCenter.SendMessage(new CommonMessage()
            {
                Mid = (int)MESSAGE_TYPE.GAME_OVER
            });
            gameObject.SetActive(false);
            return;
        }
        invicible = true;
        GetComponent<SpriteRenderer>().DOColor(Color.red,.125f).SetLoops(2, LoopType.Yoyo);
        GetComponent<SpriteRenderer>().DOFade(0.3f, 0.25f).SetLoops(8,LoopType.Yoyo).OnComplete(() =>
        {
            invicible = false;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Collider2D>().enabled = true;
        });
        playerPropertes.health = health;
        HPtext.text = "Health:" + health;
    }

    public void Health_Up(int _amount)
    {
        health += _amount;
        playerPropertes.health = health;
        HPtext.text = "Health:" + health;
    }

    public void Shot_Speed_Up(float _amount)
    {
        playerPropertes.shotSpeed += _amount;
        GetComponent<AttackCtrl>().UpdateShotSpeed();
    }

    public void Ammo_Up(int _amount)
    {
        playerPropertes.maxAmmo += _amount;
        GetComponent<AttackCtrl>().UpdateMaxAmmo();
    }

    public void SetAmmoAmount(int _amount)
    {
        Ammotext.text = "Ammo:" + _amount;
    }

    public void SetReloadingBarValue(float _value)
    {
        reloadingImage.fillAmount = _value;
    }

    //public void onScoreFull()
    //{
    //    Color c =face.color;
    //    face.color = new Color(c.r,c.g,c.b,c.a + 0.1f);
    //}
}
