using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public Text m_ScoreText;
    public Text m_HighScoreText;

    private AudioSource m_AudioSource; 


    void Start()
    {
        m_ScoreText.text = "Your Score : " + GameManager.Instance.GetScore().ToString();
        m_HighScoreText.text = "High Score : " + GameManager.Instance.GetHighScore().ToString();

    }

    public void PlayAgain()
    {
        EventManager.TriggerGameStart();
    }

    public void OnMenuButtonPress()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
