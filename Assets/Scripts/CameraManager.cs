using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Runner m_Runner;
    private Vector3 m_Offset;

	// Use this for initialization
	void Start ()
    {
        m_Offset = m_Runner.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        FollowPlayer();
    }


    /// <summary>
    /// Make the camera follow the player with sart offset
    /// </summary>
    private void FollowPlayer()
    {
        transform.position = new Vector3(0, transform.position.y, m_Runner.transform.position.z - m_Offset.z);
    }
}
