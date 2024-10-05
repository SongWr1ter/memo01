using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

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

    private void Start() {
        health = playerPropertes.maxHealth;
        playerPropertes.shotSpeed = attack.ShootSpeed;
        playerPropertes.maxAmmo = attack.AmmoAmount;
    }

    public void GetDamaged(int _damage)
    {
        health -= _damage;
        if(health <= 0)
        {
            MessageCenter.SendMessage(new CommonMessage()
            {
                Mid = (int)MESSAGE_TYPE.GAME_OVER
            });
            return;
        }
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
