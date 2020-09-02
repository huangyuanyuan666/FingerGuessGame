using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    //背景音乐播放
    AudioSource m_bgAudioSource;
    //音效播放
    AudioSource m_audioSource;

    static AudioManager m_instance;
    static Mutex m_mutex = new Mutex();
    #endregion

    #region 属性
    public static AudioManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_mutex.WaitOne();

                m_instance = new AudioManager();

                m_mutex.ReleaseMutex();
            }
            return m_instance;
        }
    }
    #endregion

    #region unity回调
    void Awake()
    {
        m_instance = this;
    }

    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        m_bgAudioSource = sources[0];
        m_audioSource = sources[1];
    }
    #endregion

    #region 方法
    //播放音效
    public void PlayAudio(string audioName)
    {
        AudioClip clip = Resources.Load<AudioClip>(GameDatas.AudioPath + audioName);
        if (clip != null)
        {
            m_audioSource.clip = clip;
            m_audioSource.Play();
        }
    }

    public void PlayBg()
    {
        m_bgAudioSource.Play();
    }

    //停止所有音乐
    public void StopAllAudio()
    {
        StopBg();
        StopAudio();
    }

    //停止背景音乐
    public void StopBg()
    {
        m_bgAudioSource.Stop();
    }

    //停止音效
    public void StopAudio()
    {
        m_audioSource.Stop();
    }

    //背景静音
    public void MuteBg(bool isMute)
    {
        m_bgAudioSource.mute = isMute;
    }
    //音效静音
    public void MuteAudio(bool isMute)
    {
        m_audioSource.mute = isMute;
    }
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    #endregion







}
