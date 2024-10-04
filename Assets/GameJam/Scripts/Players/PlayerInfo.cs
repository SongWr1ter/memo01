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
}

public class PlayerInfo : MonoBehaviour,IDamageable
{
    public PlayerPropertes playerPropertes;
    public int health{get;set;}
    public TMP_Text HPtext;
    public TMP_Text Ammotext;
    public Image reloadingImage;

    private void Start() {
        health = playerPropertes.maxHealth;
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
