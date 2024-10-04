using UnityEngine;
using System;

public class GlobalResManager : SingleTon<GlobalResManager>
{
    [SerializeField]
    private GameObject player;
    public GameObject Player{
        get{
            if(player == null)
            {
                player = GameObject.FindWithTag("Player");
            }
            return player;
        }
    }
}
