using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VictoryPanel : MonoBehaviour
{
    private CanvasGroup group;

    // Start is called before the first frame update
    void Start()
    {
        group = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        MessageCenter.AddListener(OnGameWin);
    }

    private void OnDisable()
    {
        MessageCenter.RemoveListner(OnGameWin);
    }

    void OnGameWin(CommonMessage msg)
    {
        if(msg.Mid != (int)MESSAGE_TYPE.WIN) return;
        group.alpha = 1f;
        group.interactable = true;
        group.blocksRaycasts = true;
    }
}
