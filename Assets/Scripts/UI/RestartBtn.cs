using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartBtn : MonoBehaviour
{
    public void Restart()
    {
        ObjectPool.Instance.DequeueAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
