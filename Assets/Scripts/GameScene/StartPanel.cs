using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    private Text _maxScore;
    private Text _currentScore;

    #endregion

    #region 属性
    #endregion

    #region unity回调
    void Start()
    {
        _maxScore = transform.Find("MaxScore").GetComponent<Text>();
        _currentScore = transform.Find("CurrentScore").GetComponent<Text>();

        transform.Find("StartGameBtn").GetComponent<Button>().onClick.AddListener(() => { StartBtnClickEvent(); });
        transform.Find("BgSound").GetComponent<Toggle>().onValueChanged.AddListener((toggle) => { BgToggleValueChangeEvent(toggle); });
        transform.Find("Audio").GetComponent<Toggle>().onValueChanged.AddListener((toggle) => { AudioToggleValueChangeEvent(toggle); });

        Initialized();
    }
    #endregion

    #region 方法
    void Initialized()
    {
        UpdateScore();
    }
    #endregion

    #region 事件回调
    void StartBtnClickEvent()
    {
        AudioManager.Instance.PlayAudio("button");
        SendMessageUpwards("ShowPanel", gameObject.name,SendMessageOptions.RequireReceiver);
    }

    void UpdateScore()
    {
        Score score = GameDatas.ScoreData;
        _maxScore.text = "最高分：" + score.MaxScore.ToString();
        _currentScore.text = "当前分：" + score.CurrentScore.ToString();
    }

    void BgToggleValueChangeEvent(bool toggle)
    {
        AudioManager.Instance.MuteBg(!toggle);
    }
    void AudioToggleValueChangeEvent(bool toggle)
    {
        AudioManager.Instance.MuteAudio(!toggle);
    }
    #endregion

    #region 帮助方法
    #endregion
}
