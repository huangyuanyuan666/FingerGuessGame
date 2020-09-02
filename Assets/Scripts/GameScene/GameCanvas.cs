using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    private Transform _startPanel;
    private Transform _playingPanel;
    #endregion

    #region 属性
    #endregion

    #region unity回调
    void Start()
    {
        _startPanel=transform.Find("StartPanel");
        _playingPanel = transform.Find("PlayingPanel");
        transform.Find("QuitAppBtn").GetComponent<Button>().onClick.AddListener(() => { QuitBtnClickEvent(); });
    }
    #endregion

    #region 方法
    #endregion

    #region 事件回调
    void ShowPanel(string panelName)
    {
        switch (panelName)
        {
            case "StartPanel":
                _startPanel.gameObject.SetActive(false);
                _playingPanel.gameObject.SetActive(true);
                break;
            case "PlayingPanel":
                _playingPanel.gameObject.SetActive(false);
                _startPanel.gameObject.SetActive(true);
                break;
            default:break;
        }
    }

    void UpdateScore()
    {
        _startPanel.gameObject.SendMessage("UpdateScore");
    }

    //退出游戏
    void QuitBtnClickEvent()
    {
        Application.Quit();
    }
    #endregion

    #region 帮助方法
    #endregion

}
