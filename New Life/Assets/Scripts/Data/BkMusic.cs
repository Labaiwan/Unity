using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BkMusic : MonoBehaviour
{
    private static BkMusic instance;

    public static BkMusic Instance => instance;

    //��������
    private AudioSource bkSource;

    void Awake()
    {
        instance = this;
        bkSource = this.GetComponent<AudioSource>();

        //ͨ������ ������ ���ֵĴ�С�Ϳ���
        //һ��ʼû�о�Ĭ�϶�ȡMusicData���ֵ
        MusicData data = GameDataMgr.Instance.musicData;
        SetMusicOpen(data.isMusicOpen);
        SetMusicVolume(data.MusicVolume);
    }

    //�������ֿ��صķ���
    public void SetMusicOpen(bool isOpen)
    { 
        bkSource.mute = !isOpen;
    }
    //�������������ķ���
    public void SetMusicVolume(float value)
    {
        bkSource.volume = value;
    }
}
