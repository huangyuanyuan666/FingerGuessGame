using System.Collections;
using System.Collections.Generic;

using System.IO;
using UnityEngine;

public static class Tools
{
    static string _scoreJsonPath = Application.streamingAssetsPath + '/' + GameDatas.JsonPath + "ScoreData.json";
    //将分数存储到json文件
    public static void SaveScoreToFile(int currentScore)
    {
        Score score = GameDatas.ScoreData;

        int maxScore;
        if (score != null)
        {
            maxScore = score.MaxScore > currentScore ? score.MaxScore : currentScore;
        }
        else
        {
            maxScore = currentScore;
        }

        Score newScore = new Score();
        newScore.MaxScore = maxScore;
        newScore.CurrentScore = currentScore;

        GameDatas.ScoreData = newScore;

        string json = JsonUtility.ToJson(newScore);
        File.WriteAllText(_scoreJsonPath,json);
    }

    //从json文件中获得分数
    public static void GetScoreFromFile()
    {
        string json = File.ReadAllText(_scoreJsonPath);

        GameDatas.ScoreData = JsonUtility.FromJson<Score>(json);
    }

    //json转score
    public static Score JsonToScore(string json)
    {
        Score score = JsonUtility.FromJson<Score>(json);
        return score;
    }

    //分数转json文本
    public static string ScoreToJson(Score score)
    {
        string json = JsonUtility.ToJson(score);
        return json;
    }
}

[SerializeField]
public class Score
{
    public int MaxScore;
    public int CurrentScore;
}