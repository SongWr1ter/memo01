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

    public void DIAN()
    {
        Time.timeScale = 1.0f;
        GlobalResManager.Instance.Player.
        GetComponent<PlayerInfo>().playerPropertes.Strength++;//这种获取引用方式太傻逼了我再也不用了
        toggle(false);
    }
    
}
