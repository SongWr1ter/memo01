using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    private int score = 0;
    private TMP_Text text;
    [SerializeField]private int maxScore;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        MessageCenter.AddListener(OnAddScore);
    }
    private void OnDestroy() {
        MessageCenter.RemoveListner(OnAddScore);
    }

    public void OnAddScore(CommonMessage msg)
    {
        if(msg.Mid != (int)MESSAGE_TYPE.ADD_SCORE) return;
        score += msg.intParam;
        text.text = "Exp:" + score + "/" + maxScore;
        if(scoreChecker() == 1)
        {
            onScoreFull();
        }
    }

    int scoreChecker()
    {
        if(score >= maxScore)
        {
            myGameManager.Instance.addCurrentLevel();
            maxScore = getNextMaxScore();
            return 1;
        }
        return 0;
    }

    int getNextMaxScore()
    {
        return maxScore + 10;
    }

    void onScoreFull()
    {
        CardPanel.Instance.toggle(true);//¿ªÆôÉý¼¶Ãæ°å
        Time.timeScale = 0;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>().onScoreFull();
    }
}
