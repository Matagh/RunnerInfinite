using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool IsGamePaused = false;

    private HudManager m_HudManager;
    private Runner m_Runner;
    private float m_Score = 0;
    private float m_HighScore;
    private int m_CoinsCollected = 0;

    private int m_CurrentObstaclesAvoided = 0;
    [SerializeField] int m_ObstaclesPerLevel = 20;
    [SerializeField] float m_SpeedIncreaseDeltaOnLevelUp = 1;
    [SerializeField] int m_CoinsNeededToChargePower = 20;

    


    void Awake()
    {
        // SINGLETON PATTERN
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Instance.m_HudManager = FindObjectOfType<HudManager>();
            Destroy(gameObject);
        }

        // don't destroy when changingg scenes
        DontDestroyOnLoad(gameObject);

        // find hud object
        m_HudManager = FindObjectOfType<HudManager>();
        m_Runner = FindObjectOfType<Runner>();

        m_HighScore = PlayerPrefs.GetFloat("highscore", m_HighScore);
    }

    private void Start()
    {
        EventManager.GameStart += GameStart;
        EventManager.GameOver += GameOver;
        EventManager.GamePause += TogglePause;
        EventManager.LevelUp += LevelUp;
        EventManager.CoinCollected += CoinCollected;
        //EventManager.PowerCharged += PowerCharged;

        EventManager.TriggerGameStart();

    }

    private void Update()
    {
        m_Score = Runner.distanceTraveled;
        if(m_HudManager)
            m_HudManager.RefreshHud();

        if(m_Score > m_HighScore)
        {
            m_HighScore = m_Score;
            PlayerPrefs.SetFloat("highscore", m_HighScore);
        }

        // Check level-up
        if(m_CurrentObstaclesAvoided >= m_ObstaclesPerLevel)
        {
            EventManager.TriggerLevelUp();
        }

        if(!m_HudManager)
            m_HudManager = FindObjectOfType<HudManager>();
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("highscore", m_HighScore);
        PlayerPrefs.Save();
    }

    #region EVENTS RESPONSES
    private void GameStart()
    {
        m_Score = 0;
        m_CoinsCollected = 0;
        m_CurrentObstaclesAvoided = 0;
        m_HudManager = FindObjectOfType<HudManager>();
        //m_HudManager.RefreshHud();
        SceneManager.LoadScene("GameScene");
    }

    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void LevelUp()
    {
        print("LEVEL UP");
        m_Runner.IncreaseRunSpeed(m_SpeedIncreaseDeltaOnLevelUp);
        m_CurrentObstaclesAvoided = 0;
        m_ObstaclesPerLevel += 2;
    }

    private void CoinCollected()
    {
        IncrementCoins();
        m_HudManager.RefreshHud();
        if(m_CoinsCollected == m_CoinsNeededToChargePower)
        {
            //m_CoinsCollected = 0;
            EventManager.TriggerPowerCharged();
        }
    }

    private void TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
    #endregion

    /// <summary>
    /// Increase the game score by the specified value
    /// </summary>
    /// <param name="_amount"></param>
    public void IncreaseScore(int _amount)
    {
        m_Score += _amount;

        if (m_HudManager != null)
            m_HudManager.RefreshHud();

        if (m_Score > m_HighScore)
        {
            m_HighScore = m_Score;
        }
    }

    /// <summary>
    /// Increment the number of coins collected
    /// </summary>
    public void IncrementCoins()
    {
        m_CoinsCollected++;

        if (m_HudManager != null)
            m_HudManager.RefreshHud();
    }

    public void ResetCoinsCollected()
    {
        m_CoinsCollected = 0;
    }

    /// <summary>
    /// Increment the number of obstacles avoided by the runner
    /// </summary>
    public void IncrementObstaclesAvoided()
    {
        m_CurrentObstaclesAvoided++;
    }

    #region GETTERS - SETTERS

    public Runner GetRunner() { return m_Runner; }
    public void SetRunner(Runner value) { m_Runner = value; }
    public float GetScore() { return m_Score; }
    public int GetCoinsCollected() { return m_CoinsCollected; }
    public float GetHighScore() { return m_HighScore; }

    public int GetCoinsNeededToChargePower() { return m_CoinsNeededToChargePower; }

    #endregion
}
