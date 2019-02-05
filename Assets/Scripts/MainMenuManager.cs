using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject m_MainMenuSection;
    [SerializeField] GameObject m_OptionPanel;

    [SerializeField] Slider m_MusicVolumeSlider;
    [SerializeField] Text m_MusicVolumeTextValue;

    [SerializeField] Slider m_EffectsVolumeSlider;
    [SerializeField] Text m_EffectsVolumeTextValue;

    // Use this for initialization
    void Start ()
    {
        m_MusicVolumeSlider.value = SoundManager.Instance.GetMusicAudioSourceVolume();
        m_EffectsVolumeSlider.value = SoundManager.Instance.GetEffectsVolumeLevel();
        UpdateMusicVolumeText();
        UpdateEffectsVolumeText();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Play()
    {
        if (GameManager.Instance)
            EventManager.TriggerGameStart();
        else
            SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnOptionPressed()
    {
        m_MainMenuSection.SetActive(false);
        m_OptionPanel.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        m_MainMenuSection.SetActive(true);
        m_OptionPanel.SetActive(false);
    }

    public void UpdateMusicVolumeText()
    {
        m_MusicVolumeTextValue.text = (m_MusicVolumeSlider.value * 100).ToString();
    }
    public void UpdateEffectsVolumeText()
    {
        m_EffectsVolumeTextValue.text = (m_EffectsVolumeSlider.value * 100).ToString();
    }
}
