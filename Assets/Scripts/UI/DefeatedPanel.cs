using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatedPanel : MonoBehaviour
{
    private CanvasGroup group;

    // Start is called before the first frame update
    void Start()
    {
        group = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        MessageCenter.AddListener(OnGameOver);
    }

    private void OnDisable()
    {
        MessageCenter.RemoveListner(OnGameOver);
    }

    void OnGameOver(CommonMessage msg)
    {
        if (msg.Mid != (int)MESSAGE_TYPE.GAME_OVER) return;
        group.alpha = 1f;
        group.interactable = true;
        group.blocksRaycasts = true;
    }
}
