using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
	[SerializeField] float m_rotationSpeed = 100;

    // Update is called once per frame
    void Update ()
    {
        float angleRot = Time.deltaTime * m_rotationSpeed;
        transform.Rotate(Vector3.up * angleRot, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
