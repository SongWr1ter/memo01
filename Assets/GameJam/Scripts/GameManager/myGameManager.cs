using UnityEngine;
using UnityEngine.SceneManagement;

public class myGameManager : SingleTon<myGameManager>
{
    private int Level;
    private bool isGameOver;
    // Start is called before the first frame update
    void Start()
    {
        MessageCenter.AddListener(OnGameOver);
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    private void Update() {
        if(isGameOver)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                ObjectPool.Instance.DequeueAll();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif

        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        MessageCenter.RemoveListner(OnGameOver);
    }

    public void OnGameOver(CommonMessage msg)
    {
        if(msg.Mid != (int)MESSAGE_TYPE.GAME_OVER) return;
        Time.timeScale = 0.001f;
        isGameOver = true;
    }

    public int getCurrentLevel()
    {
        return Level;
    }

    public void addCurrentLevel(int _i=1)
    {
        Level += _i;
    }
}
