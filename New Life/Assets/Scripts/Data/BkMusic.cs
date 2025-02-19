using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BkMusic : MonoBehaviour
{
    private static BkMusic instance;

    public static BkMusic Instance => instance;

    //背景音乐
    private AudioSource bkSource;

    void Awake()
    {
        instance = this;
        bkSource = this.GetComponent<AudioSource>();

        //通过数据 来设置 音乐的大小和开关
        //一开始没有就默认读取MusicData里的值
        MusicData data = GameDataMgr.Instance.musicData;
        SetMusicOpen(data.isMusicOpen);
        SetMusicVolume(data.MusicVolume);
    }

    //背景音乐开关的方法
    public void SetMusicOpen(bool isOpen)
    { 
        bkSource.mute = !isOpen;
    }
    //背景音乐音量的方法
    public void SetMusicVolume(float value)
    {
        bkSource.volume = value;
    }
}
