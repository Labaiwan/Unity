using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Toggle togMusic;
    public Toggle togSound;
    public Slider sliderMusic;
    public Slider sliderSound;
    public Button btnQuit;
    public override void Init()
    {
        //��ʼ�������ʾ���� ���ݱ��ش洢��������������ʼ��
        MusicData data = GameDataMgr.Instance.musicData;
        togMusic.isOn = data.isMusicOpen;
        togSound.isOn = data.isSoundOpen;
        sliderMusic.value = data.MusicVolume;
        sliderSound.value = data.SoundVolume;

        togMusic.onValueChanged.AddListener((value)=>
        {
            BkMusic.Instance.SetMusicOpen(value);
            GameDataMgr.Instance.musicData.isMusicOpen = value;
        });

        togSound.onValueChanged.AddListener((value) =>
        {
            GameDataMgr.Instance.musicData.isSoundOpen = value;
        });

        sliderMusic.onValueChanged.AddListener((value) =>
        {
            BkMusic.Instance.SetMusicVolume(value);
            GameDataMgr.Instance.musicData.MusicVolume = value;
        });

        sliderSound.onValueChanged.AddListener((value) =>
        {
            GameDataMgr.Instance.musicData.SoundVolume = value;
        });

        btnQuit.onClick.AddListener(()=>
        {
            //�ڹر����ʱ �洢�޸ĺ������ ��Լ����
            GameDataMgr.Instance.SaveMusicData();
            UIDataMgr.Instance.HidePanel<SettingPanel>();
            if (SceneManager.GetActiveScene().buildIndex > 0)
            {
                UIDataMgr.Instance.showPanel<BackPanel>();
            }
            
        });
    }
}
