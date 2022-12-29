using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;

    public const string HighScoreKey = "HighScore";


    public void Start()
    {
        highScoreText.text = "High Score : " + PlayerPrefs.GetInt(HighScoreKey , 0);
    }    

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
