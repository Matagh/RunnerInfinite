using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private Runner m_Runner;
    [SerializeField] AudioSource m_MusicAudioSource;
    [SerializeField] AudioSource m_UIAudioSource;

    [SerializeField] AudioClip m_AmbientMusic;
    [SerializeField] AudioClip m_PlayerFootstepsSound;
    [SerializeField] AudioClip m_LevelUpSound;
    [SerializeField] AudioClip m_CoinCollectSound;
    [SerializeField] AudioClip m_PowerChargedSound;
    [SerializeField] AudioClip m_PowerActivationSound;
    [SerializeField] AudioClip m_PowerEndSound;

    [SerializeField] AudioClip m_ButtonClickSound;

    [SerializeField, Range(0.0f, 1.0f)] float m_GlobalVolumeLevel = 1f;
    [SerializeField, Range(0.0f, 1.0f)] float m_MusicVolumeLevel = 1f;
    [SerializeField, Range(0.0f, 1.0f)] float m_EffectsVolumeLevel = 1f;

    [SerializeField, Range(0.0f, 1.0f)] float m_MusicVolumeScale = 0.5f;
    [SerializeField, Range(0.0f, 1.0f)] float m_LevelUpVolumeScale = 0.5f;
    [SerializeField, Range(0.0f, 1.0f)] float m_CoinCollectVolumeScale = 0.5f;
    [SerializeField, Range(0.0f, 1.0f)] float m_PowerChargedVolumeScale = 0.5f;
    [SerializeField, Range(0.0f, 1.0f)] float m_PowerActivationVolumeScale = 0.5f;
    [SerializeField, Range(0.0f, 1.0f)] float m_PowerEndVolumeScale = 0.5f;

    private void Awake()
    {// SINGLETON PATTERN
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Instance.m_Runner = FindObjectOfType<Runner>();
            Destroy(gameObject);
        }
        // don't destroy when changingg scenes
        DontDestroyOnLoad(gameObject);

        m_Runner = FindObjectOfType<Runner>();
        //m_MusicAudioSource = GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start ()
    {
        EventManager.GameStart += GameStart;
        EventManager.GameOver += GameOver;
        EventManager.LevelUp += LevelUp;
        EventManager.CoinCollected += CoinCollected;
        EventManager.PowerCharged += PowerCharged;
        EventManager.ActivatePower += ActivatePower;
        EventManager.StopPower += StopPower;

        m_MusicAudioSource.volume = PlayerPrefs.GetFloat("musicVolume", 1);
        m_EffectsVolumeLevel = PlayerPrefs.GetFloat("effectsVolume", 1);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnDestroy()
    {
        PlayerPrefs.Save();
    }

    #region Events Audio responses
    private void GameStart()
    {
        //Start main ambiance sound 
        StartAmbientMusic();
    }

    private void GameOver()
    {
        //start game over melody
        m_MusicAudioSource.Stop();
    }

    private void LevelUp()
    {
        if(m_LevelUpSound)
            m_Runner.m_AudioSource.PlayOneShot(m_LevelUpSound, m_LevelUpVolumeScale * m_EffectsVolumeLevel);
    }

    private void CoinCollected()
    {
        if(m_CoinCollectSound)
            m_Runner.m_AudioSource.PlayOneShot(m_CoinCollectSound, m_CoinCollectVolumeScale * m_EffectsVolumeLevel);
    }

    private void PowerCharged()
    {
        if(m_PowerChargedSound)
            m_Runner.m_AudioSource.PlayOneShot(m_PowerChargedSound, m_PowerChargedVolumeScale * m_EffectsVolumeLevel);
    }

    private void ActivatePower()
    {
        if (m_PowerActivationSound)
            m_Runner.m_AudioSource.PlayOneShot(m_PowerActivationSound, m_PowerActivationVolumeScale * m_EffectsVolumeLevel);
    }

    private void StopPower()
    {
        if (m_PowerEndSound)
            m_Runner.m_AudioSource.PlayOneShot(m_PowerEndSound, m_PowerEndVolumeScale * m_EffectsVolumeLevel);
    }
    #endregion

    public void PlayButtonClickSound()
    {
        m_UIAudioSource.PlayOneShot(m_ButtonClickSound);
    }

    public void ApplyMusicVolumeLevel(float _newLevel)
    {
        m_MusicAudioSource.volume = _newLevel;
        PlayerPrefs.SetFloat("musicVolume", _newLevel);
    }

    public void ApplyEffectsVolumeLevel(float _newLevel)
    {
        m_EffectsVolumeLevel = _newLevel;
        PlayerPrefs.SetFloat("effectsVolume", _newLevel);
    }

    public void StartAmbientMusic()
    {
        //print("Start ambient music");
        m_MusicAudioSource.clip = m_AmbientMusic;
        m_MusicAudioSource.Play();
    }
    public void StopMusic()
    {
        m_MusicAudioSource.Stop();
    }


    #region GETTER / SETTER
    public float GetMusicAudioSourceVolume() { return m_MusicAudioSource.volume; }
    public float GetEffectsVolumeLevel() { return m_EffectsVolumeLevel; }
    #endregion
}