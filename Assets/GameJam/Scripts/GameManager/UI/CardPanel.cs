using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPanel : SingleTon<CardPanel>
{
    private CanvasGroup group;
    // Start is called before the first frame update
    void Start()
    {
        group = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggle(bool _b)
    {
        if(_b)
        {
            group.alpha = 1.0f;
            group.interactable = true;
            group.blocksRaycasts = true;
        }
        else
        {
            group.alpha = 0f;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
    }

    public void Power_Up()
    {
        Time.timeScale = 1.0f;
        GlobalResManager.Instance.Player.
        GetComponent<PlayerInfo>().playerPropertes.Strength++;//这种获取引用方式太傻逼了我再也不用了
        toggle(false);
    }

    public void Health_Up()
    {
        Time.timeScale = 1.0f;
        GlobalResManager.Instance.Player.
        GetComponent<PlayerInfo>().Health_Up(1);
        toggle(false);
    }

    public void Shot_Speed_Up()
    {
        Time.timeScale = 1.0f;
        GlobalResManager.Instance.Player.
        GetComponent<PlayerInfo>().Shot_Speed_Up(1f);
        toggle(false);
    }

    public void Ammo_UP()
    {
        Time.timeScale = 1.0f;
        GlobalResManager.Instance.Player.
        GetComponent<PlayerInfo>().Ammo_Up(2);
        toggle(false);
    }

}
