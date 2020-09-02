using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Drawing;

public class GameManger : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
       
        Tools.GetScoreFromFile();
        SceneManager.LoadScene("02-GameScene");
    }
}