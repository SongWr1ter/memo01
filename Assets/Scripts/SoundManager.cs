using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager _instance;
    public static SoundManager instance
    {
        //SoundManager对象创建时先创建实例对象
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    //public static SoundManager instance;//单例模式
    // name --> AudioSource 映射, 区分音乐，音效
    [SerializeField]private AudioSource _backgroundSource,_playerEffectsSource,_enemyEffectsSource;//三种声源
    static Dictionary<string, AudioClip> table_musics;//音乐表
    static Dictionary<string, AudioClip> table_effects;//音效表
   
    static bool isEffect=true,isMusic=true;//是否播放音效，背景音乐
    bool music_ToggleChange=false;//防止暂停之后重复播放多次
    private AudioClip BG_currentClip;//获取当前正在播放的音频，以便下一次播放

    /*记得初始化AudioSource（SoundManager中的控制BG_Audio的AudioSource）*/
    private float targetvolume;//渐进目标音量
    private bool isfading=false;//控制是否继续渐进
    private bool isup=true;//判断是渐入还是渐出
    private float music_time;//渐进时间

    private void Awake() {
        _backgroundSource.volume=0f;
        //创建单例对象，并且让它在场景切换时也不会被清除
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != _instance)
        {   
            //此脚本永不销毁，并且每次进入初始场景时进行判断，若存在重复的则销毁
            Destroy(gameObject);
        }
        table_musics = new Dictionary<string, AudioClip> ();
        table_effects = new Dictionary<string, AudioClip> ();

        //本地加载 
        AudioClip [] BG_audioArray = Resources.LoadAll<AudioClip>("Audios/BGM/");
        AudioClip [] Effect_audioArray = Resources.LoadAll<AudioClip>("Audios/Effects/");
        
        foreach (AudioClip item in BG_audioArray) {
            //print(item.name);
            table_musics.Add(item.name,item);
        }//存放背景音乐到字典
        foreach (AudioClip item in Effect_audioArray) {
            table_effects.Add(item.name,item);
        }//存放音效到字典
        
    }


    private void Update() {
        /*控制背景音乐的开关*/
        if (isMusic == true && music_ToggleChange == true)
        {
            //_backgroundSource.clip=BG_currentClip;
            _backgroundSource.volume=1.0f;
            music_ToggleChange = false;
        }
        if (isMusic == false && music_ToggleChange == true)
        {
            _backgroundSource.volume=0f;
            isfading=false;
            music_ToggleChange = false;
        }
        /*Fade音乐渐进主体*/
        if (!isfading) return;//不用渐进了，返回
        if(Math_Abs(_backgroundSource.volume - targetvolume) >= 0.01f&&isup)
        {
            _backgroundSource.volume+=Mathf.Lerp(0,targetvolume,(1/music_time)*Time.deltaTime);       
        }else if(Math_Abs(_backgroundSource.volume - targetvolume) >= 0.01f&&!isup){
            _backgroundSource.volume-=Mathf.Lerp(targetvolume,1,(1/music_time)*Time.deltaTime);    
        }
        else
        {
            _backgroundSource.volume = targetvolume;
            isfading = false;//达到目标音量，退出
        }

    }
    public void PlayBGaudio(string audioName,float tar_vol = 1.0f,float time = 10f)
    {

        if (table_musics.ContainsKey(audioName)&&isMusic) 
        {
            _backgroundSource.clip=table_musics[audioName];
            //_backgroundSource.volume=0;
            _backgroundSource.Play();
            FadeIn(tar_vol,time);
            BG_currentClip=_backgroundSource.clip;
        }
        if (!isMusic) //用户取消背景音
        {
            _backgroundSource.clip=table_musics[audioName];
            _backgroundSource.volume=0;
            _backgroundSource.Play();//以0音量播放
            BG_currentClip=_backgroundSource.clip;
        }
    }//播放背景音乐
     public void PlayEnemyEffect(string audioName){
        if (table_effects.ContainsKey(audioName)&&isEffect) 
        {
            _enemyEffectsSource.clip=table_effects[audioName];
            _enemyEffectsSource.Play();
        }
    }//播放敌人音效
    public void PlayPlayerEffect(string audioName){
        if (table_effects.ContainsKey(audioName)&&isEffect) 
        {
            _playerEffectsSource.clip=table_effects[audioName];
            _playerEffectsSource.Play();
        }
    }//播放玩家音效
    
    public void isPlay_BGaudio (bool f) {
        music_ToggleChange=true;
        isMusic=f;
    }//flag=0,背景音乐不再播放
    public void isPlay_EffectMusic (bool f) {
        isEffect=f;
    }//flag=0,音效不再播放


    /*------以下是音乐渐进的代码---------*/
    private void FadeMusic(float targetVolume/*0 or 1*/, float durtime/*seconds*/,bool is_up)
    {
        targetvolume = targetVolume;
        if(durtime==0){
            isfading=false;
            _backgroundSource.volume=targetvolume;
            return ;

        }else{
            music_time=durtime;
            isfading = true;
            isup=is_up;
        }
        
    }//渐进参数初始化

    public void MusicFadeOut(float tar_vol,float t)
    {
        if(isMusic)
            FadeMusic(tar_vol,t,false);
    }//渐出

    private void FadeIn(float tar_vol,float t)
    {
        FadeMusic(tar_vol,t,true);
    }//渐入
    private float Math_Abs(float num){
        if(num<=0) return -num;
        else return num;
    }
    /*------------------分割线--------------------*/

}
