using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clocker : MonoBehaviour
{
    private const float MAX_WAITING_TIME = 1201f;
    [SerializeField]
    private float speed = 1.0f;
    private TMP_Text text;
    private float timer;
    private bool fin;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        timer = MAX_WAITING_TIME;
    }

    // Update is called once per frame
    void Update()
    {
        if(fin) return;
        if (timer > 0)
        {
            timer -= Time.deltaTime * speed;
            SetClock(timer);
        }
        else
        {
            Finish();
        }
    }

    void Finish()
    {
        timer = MAX_WAITING_TIME;
        fin = true;
        text.text = " ";
        MessageCenter.SendMessage(new CommonMessage
        {
            Mid = (int)MESSAGE_TYPE.WIN
        });
    }

    void SetClock(float t)
    {
        float min = t / 60.0f;
        float sec = t % 60.0f;
        min = Mathf.Floor(min);
        sec = Mathf.Floor(sec);
        string m = min.ToString();
        if (min < 10)
        {
            m = "0" + m;
        }
        string s = sec.ToString();
        if (sec < 10)
        {
            s = "0" + s;
        }
        text.text = m + ":" + s;
    }
}
