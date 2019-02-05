using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileUiScript : MonoBehaviour
{
    [SerializeField] Button m_PowerButton;


    private void Awake()
    {
#if !UNITY_ANDROID
        Destroy(this.gameObject);
#endif
    }
    // Use this for initialization
    void Start ()
    {
        EventManager.PowerCharged += OnPowerCharged;
        EventManager.ActivatePower += OnPowerActivated;
        m_PowerButton.interactable = false;
	}

    private void OnPowerCharged()
    {
        if(m_PowerButton)
            m_PowerButton.interactable = true;
    }

    private void OnPowerActivated()
    {
        if (m_PowerButton)
            m_PowerButton.interactable = false;
    }

    public void OnPowerButtonPress()
    {
        EventManager.TriggerActivatePower();
    }
}
