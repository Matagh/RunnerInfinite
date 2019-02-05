using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HudManager : MonoBehaviour
{
    [SerializeField] Text m_ScoreLabel;
    [SerializeField] Text m_LevelUpLabel;
    [SerializeField] Text m_PowerChargedLabel;

    [SerializeField] Slider m_PowerSlider;
    [SerializeField] Image m_PowerChargedSliderBackground;
    [SerializeField] Image m_SliderFillArea;

    [SerializeField] GameObject m_PauseMenuPanel;


    private Coroutine m_PowerBlinkCoroutine = null;
    private bool m_IsPowerActive = false;

    private Color m_SliderDefaultFillColor;

    void Start ()
    {
        m_SliderDefaultFillColor = m_SliderFillArea.color;
        EventManager.LevelUp += LevelUp;
        EventManager.PowerCharged += PowerCharged;
        EventManager.ActivatePower += ActivatePower;
        EventManager.StopPower += StopPower;
        EventManager.GamePause += OnTogglePause;

        //AssignUIElements();
        m_LevelUpLabel.GetComponent<CanvasRenderer>().SetAlpha(0f);
        m_PowerChargedLabel.enabled = false;
        m_PowerChargedSliderBackground.enabled = false;
    }

    void Update()
    {
        RefreshHud();
    }

    void OnDestroy()
    {
        EventManager.LevelUp -= LevelUp;
        EventManager.PowerCharged -= PowerCharged;
        EventManager.ActivatePower -= ActivatePower;
        EventManager.StopPower -= StopPower;
        EventManager.GamePause -= OnTogglePause;
    }

    public void RefreshHud()
    {
        if(m_ScoreLabel)
            m_ScoreLabel.text = "Score : " + (int)GameManager.Instance.GetScore();

        if (m_PowerSlider)
        {
            if (m_IsPowerActive)
                m_PowerSlider.value = 1 - (GameManager.Instance.GetRunner().GetCurrentPowerTime() / GameManager.Instance.GetRunner().GetPowerTimer());
            else
                m_PowerSlider.value = (float)GameManager.Instance.GetCoinsCollected() / (float)GameManager.Instance.GetCoinsNeededToChargePower();
        }
            
    }
	
    public void AssignUIElements()
    {
        m_ScoreLabel = GameObject.Find("Score Text").GetComponent<Text>();
        m_LevelUpLabel = GameObject.Find("Level Up Text").GetComponent<Text>();
        m_PowerChargedLabel = GameObject.Find("PowerChargedText").GetComponent<Text>();
        m_PowerSlider = GameObject.Find("PowerSlider").GetComponent<Slider>();
        m_PowerChargedSliderBackground = GameObject.Find("PowerChargedBackground").GetComponent<Image>();
        m_SliderFillArea = GameObject.Find("Fill").GetComponent<Image>();
    }

    private void LevelUp()
    {
        if (m_LevelUpLabel)
        {
            m_LevelUpLabel.GetComponent<CanvasRenderer>().SetAlpha(1f);
            m_LevelUpLabel.CrossFadeAlpha(0f, 2, false);
        }
    }

    private void PowerCharged()
    {
        print("power charged, start blinking");
        if(this)
            m_PowerBlinkCoroutine = StartCoroutine(BlinkPowerChargedElements());
    }

    private void ActivatePower()
    {
        StopCoroutine(m_PowerBlinkCoroutine);

        m_PowerChargedLabel.enabled = false;
        m_PowerChargedSliderBackground.enabled = false;

        m_IsPowerActive = true;
        m_SliderFillArea.color = Color.yellow;
    }

    private void StopPower()
    {
        m_IsPowerActive = false;
        m_SliderFillArea.color = m_SliderDefaultFillColor;
    }

    public IEnumerator BlinkPowerChargedElements()
    {
        while (true)
        {
            if(m_PowerChargedLabel && m_PowerChargedSliderBackground)
            {
                m_PowerChargedLabel.enabled = true;
                m_PowerChargedSliderBackground.enabled = true;
                yield return new WaitForSeconds(0.5f);

                m_PowerChargedLabel.enabled = false;
                m_PowerChargedSliderBackground.enabled = false;
                yield return new WaitForSeconds(0.5f);
            }
            
        }
    }

    public void ActivatePause()
    {
        EventManager.TriggerGamePause();
    }

    public void OnTogglePause()
    {
        if (!m_PauseMenuPanel.activeSelf)
            m_PauseMenuPanel.SetActive(true);
        else
            m_PauseMenuPanel.SetActive(false);
    }

    public void TriggerGameStart()
    {
        EventManager.TriggerGameStart();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
