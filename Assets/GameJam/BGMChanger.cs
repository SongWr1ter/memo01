using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMChanger : MonoBehaviour
{
    new private AudioSource audio;
    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        MessageCenter.AddListener(OnGameOver);
    }

    private void OnDestroy() {
        MessageCenter.RemoveListner(OnGameOver);
    }

    public void OnGameOver(CommonMessage msg)
    {
        if(msg.Mid != (int)MESSAGE_TYPE.GAME_OVER) return;
        audio.Stop();
        audio.clip = clip;
        audio.Play();
    }
}
