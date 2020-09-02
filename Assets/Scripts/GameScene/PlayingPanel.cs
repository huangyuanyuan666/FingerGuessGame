using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayingPanel : MonoBehaviour
{
    #region 常量
    private const float DaoJiShi = 2.0f;//出结果的时间
    private const int OptionCount = 3;  //三种结果
    #endregion

    #region 事件
    #endregion

    #region 字段
    private Text _currentScoreTxt;
    private Text _currentRoundTxt;
    private Text _resText;

    private Slider _roundSlider;

    private Transform _showOptions;
    private Image _scissdors;
    private Image _stone;
    private Image _paper;

    private Button _restartBtn;

    private int _rightOptionIndex;
    private int _myOptionsIndex;
    private int _currentRound;        //当前第几轮
    private int _currentScore;

    private bool _isGuessing = false; //是否在猜拳中
    #endregion

    #region 属性
    int Round
    {
        get { return _currentRound; }
        set
        {
            if (_currentRound >= GameDatas.MaxRound)
                return;
            _currentRound++;
            _currentRoundTxt.text = "当前回合：" + _currentRound.ToString();
            //更新slider内容
            _roundSlider.value = _currentRound;
        }
    }
    #endregion

    #region unity回调

    void Start()
    {
        _currentScoreTxt = transform.Find("CurrentScoreTxt").GetComponent<Text>();
        _currentRoundTxt = transform.Find("CurrentRoundTxt").GetComponent<Text>();
        _resText = transform.Find("ResTxt").GetComponent<Text>();
        _roundSlider = transform.Find("RoundProgressSlider").GetComponent<Slider>();

        _showOptions = transform.Find("ShowResImg");
        _scissdors = _showOptions.Find("Res_Scissors").GetComponent<Image>();
        _stone = _showOptions.Find("Res_Stone").GetComponent<Image>();
        _paper = _showOptions.Find("Res_Paper").GetComponent<Image>();
        _restartBtn = transform.Find("RestartBtn").GetComponent<Button>();

        transform.Find("ScissorsBtn").GetComponent<Button>().onClick.AddListener(() => { OptionsBtnClickEvent(0); });
        transform.Find("StoneBtn").GetComponent<Button>().onClick.AddListener(() => { OptionsBtnClickEvent(1); });
        transform.Find("PaperBtn").GetComponent<Button>().onClick.AddListener(() => { OptionsBtnClickEvent(2); });
        _restartBtn.onClick.AddListener(() => { RestartBtnClickEvent(); });

        Initialized();
    }

    void OnDisable()
    {
        Initialized();
    }

    #endregion

    #region 方法
    void Initialized()
    {
        //显示内容重置
        _currentScoreTxt.text = "当前分数：0 胜率：0%";
        _resText.text = "";
        for (int i = 0; i < _showOptions.childCount; i++)
        {
            _showOptions.GetChild(i).gameObject.SetActive(false);
        }
        _restartBtn.gameObject.SetActive(false);
        _currentRoundTxt.text = "当前回合：0";
        //变量重置
        _currentScore = 0;
        _currentRound = 0;
        _isGuessing = false;

        _roundSlider.value = 0;

        StopCoroutine("PlayAnim");
    }

    //开始猜拳 计时
    IEnumerator PlayAnim()
    {
        _isGuessing = true;
        float allTime = 0;
        while (allTime < DaoJiShi)
        {
            allTime += Time.deltaTime;
            int res1 = Random.Range(0, 3);
            ShowOptionImage(res1);
            yield return null;
        }
        float res = Random.Range(0, 1.0f);
        //猜对
        if (res <= GameDatas.SuccessRate)
        {
            _rightOptionIndex = _myOptionsIndex;
            AudioManager.Instance.PlayAudio("victory");
        }
        //猜错
        else
        {
            WrongGuess();
            AudioManager.Instance.PlayAudio("defeat");
        }

        ShowOptionImage(_rightOptionIndex);
        ShowRes();
        _isGuessing = false;
    }

    //错误显示
    void WrongGuess()
    {
        int[] options = new int[OptionCount - 1];
        for (int i = 0; i < options.Length; i++)
        {
            options[i] = (_myOptionsIndex + i + 1) % OptionCount;
        }
        int random = Random.Range(0, OptionCount - 1);
        _rightOptionIndex = options[random];
    }

    //显示所有可能结果（猜拳动画模拟）
    void ShowOptionImage(int index)
    {
        for (int i = 0; i < _showOptions.childCount; i++)
        {
            if (i == index)
                continue;
            _showOptions.GetChild(i).gameObject.SetActive(false);
        }
        _showOptions.GetChild(index).gameObject.SetActive(true);
    }

    //显示最终文字结果
    void ShowRes()
    {
        _resText.gameObject.SetActive(true);
        bool res = _myOptionsIndex == _rightOptionIndex;

        if (res)
        {
            _resText.text = "回答正确 你选择的是:" + OptionNumChangeName(_myOptionsIndex) + "。正确结果是：" + OptionNumChangeName(_rightOptionIndex);
            GetScore();
        }
        else
        {
            _resText.text = "回答错误 你选择的是:" + OptionNumChangeName(_myOptionsIndex) + "。正确结果是：" + OptionNumChangeName(_rightOptionIndex);
            UpdateRateWinning();
        }

        if (_currentRound >= GameDatas.MaxRound)
        {
            //显示按钮
            _restartBtn.gameObject.SetActive(true);
        }
    }

    //回合更新
    void GetRound()
    {
        if (_currentRound >= GameDatas.MaxRound)
            return;
        Round = 0;
    }

    //分数更新
    void GetScore()
    {
        _currentScore++;
        UpdateRateWinning();
    }

    //胜率更新
    void UpdateRateWinning()
    {
        float rateWining = _currentScore / (_currentRound * 1.0f) * 100;
        _currentScoreTxt.text = "当前分数：" + _currentScore.ToString() + " 胜率：" + rateWining.ToString("f2") + "%";
    }
    #endregion

    #region 事件回调
    void OptionsBtnClickEvent(int option)
    {
        if (_isGuessing)
            return;

        AudioManager.Instance.PlayAudio("button");

        if (_currentRound >= GameDatas.MaxRound)
            return;

        GetRound();

        _myOptionsIndex = option;
        //留一段时间播放动画 动画播完后显示结果
        StartCoroutine("PlayAnim");
    }

    void RestartBtnClickEvent()
    {
        //保存数据
        int maxScore = _currentScore > GameDatas.ScoreData.MaxScore ? _currentScore : GameDatas.ScoreData.MaxScore;
        Score score = new Score();
        score.MaxScore = maxScore;
        score.CurrentScore = _currentScore;

        Tools.SaveScoreToFile(_currentScore);

        //调出开始页面
        SendMessageUpwards("ShowPanel", gameObject.name, SendMessageOptions.RequireReceiver);
        //开始界面更新数据
        SendMessageUpwards("UpdateScore");
    }
    #endregion

    #region 帮助方法
    //数字转换为文字
    string OptionNumChangeName(int index)
    {
        string name = null;
        switch (index)
        {
            case 0:
                name = "剪刀";
                break;
            case 1:
                name = "石头";
                break;
            case 2:
                name = "布";
                break;
        }
        return name;
    }
    #endregion
}