using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    enum MoveDirection { OnLeft, OnRight };
    enum Line { Left, Middle, Right };
    Line m_CurrentLine;

    public static float distanceTraveled;
    private Animator m_playerAnim;
    private Rigidbody m_Rgb;

    [SerializeField] GameObject m_RunnerBody; 

    private float m_RunSpeed;
    public float m_StartRunSpeed = 5;
    public float m_MaxRunSpeed = 8;

    public float m_groundDistance = 0.3f;
    public float m_jumpForce = 5;
    public float m_DashSpeed = 5;
    public LayerMask m_whatIsGround;

    private bool m_isLeftDashing = false;
    private bool m_isRightDashing = false;
    private bool m_CanDashLeft = true;
    private bool m_CanDashRight = true;

    [SerializeField] float m_DashTimer = 1;
    private float m_CurrentDashTimer;


    private float m_DashDestinationOnX;
    private float m_LeftLinePosOnX = -2;
    private float m_RightLinePosOnX = 2;
    private float m_MiddleLinePosOnX = 0;

    private bool m_IsJumping;
    private Quaternion m_JumpOrientation;

    private float lastCheckedPositionOnZ = 0;

    private bool m_IsPowerCharged = false;
    private bool m_IsPowerActive = false;
    [SerializeField] float m_PowerTimer = 10;
    private float m_CurrentPowerTime = 0;

    [SerializeField] Material m_DefaultBodyMaterial;
    [SerializeField] Material m_PoweredBodyMaterial;

    public AudioSource m_AudioSource;
    [SerializeField] AudioClip m_LeftFootStepSound;
    [SerializeField] AudioClip m_RightFootStepSound;
    [SerializeField] AudioClip m_JumpSound;

    [SerializeField] List<AudioClip> m_FootStepBankSound;
    [SerializeField] float m_FootstepsVolumeScale = 0.5f;

    // Use this for initialization
    void Start ()
    {
        if (!GameManager.Instance.GetRunner())
            GameManager.Instance.SetRunner(this);
        m_playerAnim = GetComponent<Animator>();
        m_Rgb = GetComponent<Rigidbody>();
        m_CurrentLine = Line.Middle;
        ResetDashing();
        distanceTraveled = 0;
        m_RunSpeed = m_StartRunSpeed;

        EventManager.GameStart += OnGameStart;
        EventManager.PowerCharged += PowerCharged;
        EventManager.ActivatePower += ActivatePower;

        distanceTraveled = 0;
        m_IsPowerCharged = false;
        m_IsPowerActive = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Infinite run
        m_Rgb.velocity = new Vector3(0, m_Rgb.velocity.y, m_RunSpeed);
        // Infinite run animation
        m_playerAnim.SetFloat("Speed", m_RunSpeed);

        if (transform.position.z - lastCheckedPositionOnZ >= 1)
        {
            distanceTraveled++;
            lastCheckedPositionOnZ = transform.position.z;
        }

        HandleDashUpdate();
        HandleJumpUpdate();
        HandlePowerUpdate();

        // Make player orientation follow player motion
        Vector3 motionDirection = m_Rgb.velocity.normalized;
        motionDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(motionDirection);
    }

    private void ChangeLine(MoveDirection direction)
    {
        if(direction == MoveDirection.OnLeft)
        {
            if (m_CurrentLine == Line.Left)
                return;
            else if (m_CurrentLine == Line.Middle)
            {
                m_DashDestinationOnX = m_LeftLinePosOnX;
                m_CurrentLine = Line.Left;
            }
                
            else
            {
                m_DashDestinationOnX = m_MiddleLinePosOnX;
                m_CurrentLine = Line.Middle;
            }
                
        }
        else
        {
            if (m_CurrentLine == Line.Right)
                return;
            else if (m_CurrentLine == Line.Middle)
            {
                m_DashDestinationOnX = m_RightLinePosOnX;
                m_CurrentLine = Line.Right;
            }
            else
            {
                m_DashDestinationOnX = m_MiddleLinePosOnX;
                m_CurrentLine = Line.Middle;
            }   
        }
    }

    private void HandleDashUpdate()
    {
        if (m_isLeftDashing)
        {
            if(transform.position.x > m_DashDestinationOnX)
            {
                m_Rgb.velocity = m_Rgb.velocity + Vector3.left * m_DashSpeed;
            }
            else
            {
                ResetDashing();
                transform.position = new Vector3(m_DashDestinationOnX, transform.position.y, transform.position.z);
            }
        }

        if (m_isRightDashing)
        {
            if (transform.position.x < m_DashDestinationOnX)
            {
                m_Rgb.velocity = m_Rgb.velocity + Vector3.right * m_DashSpeed;
            }
            else
            {
                ResetDashing();
                transform.position = new Vector3(m_DashDestinationOnX, transform.position.y, transform.position.z);
            }
        }
    }
    private void HandleJumpUpdate()
    {
        if (m_IsJumping)
        {
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, m_groundDistance, m_whatIsGround))
            {
                m_playerAnim.SetBool("grounded", true);
                m_IsJumping = false;
            }
            else
            {
                m_playerAnim.SetBool("grounded", false);
            }
        }
    }
    private void HandlePowerUpdate()
    {
        if (m_IsPowerActive)
        {
            if (m_CurrentPowerTime < m_PowerTimer)
            {
                m_CurrentPowerTime += Time.deltaTime;

                if ((m_PowerTimer - m_CurrentPowerTime) < m_PowerTimer * 0.2f)
                {
                    StartCoroutine(BlinkPlayerMaterial());
                }
            }
            else
            {
                EventManager.TriggerStopPower();
                DeactivatePower();
            }
        }
    }

    private void ResetDashing()
    {
        if (m_isLeftDashing)
            m_isLeftDashing = false;
        if (m_isRightDashing)
            m_isRightDashing = false;

        // Reset dash timer
        m_CurrentDashTimer = 0;
    }

    public void DashLeft()
    {
        if (CanDash(MoveDirection.OnLeft))
        {
            m_isLeftDashing = true;
            ChangeLine(MoveDirection.OnLeft);
        }
    }
    public void DashRight()
    {
        if (CanDash(MoveDirection.OnRight))
        {
            m_isRightDashing = true;
            ChangeLine(MoveDirection.OnRight);
        }
    }

    public void Jump()
    {
        if (!m_IsJumping)
        {
            m_Rgb.AddForce(Vector3.up * m_jumpForce, ForceMode.VelocityChange);
            m_IsJumping = true;
            m_playerAnim.SetTrigger("Jump");
            m_AudioSource.PlayOneShot(m_JumpSound, 0.15f);
        }
    }

    private bool CanDash(MoveDirection _direction)
    {
        // Check if already dashing
        if (m_isLeftDashing || m_isRightDashing)
            return false;

        if(_direction == MoveDirection.OnLeft)
        {
            if (m_CurrentLine == Line.Left)
                return false;
        }
        else
        {
            if (m_CurrentLine == Line.Right)
                return false;
        }
        return true;
    }

    public void IncreaseRunSpeed(float _value)
    {
        if (m_RunSpeed + _value <= m_MaxRunSpeed)
        {
            m_RunSpeed += _value;
        }
        else
            m_RunSpeed = m_MaxRunSpeed;
    }

    public void ActivatePower()
    {
        if (m_IsPowerCharged)
        {
            m_RunnerBody.GetComponent<Renderer>().material = m_PoweredBodyMaterial;
            Physics.IgnoreLayerCollision(10, 9);
            m_IsPowerActive = true;
            m_IsPowerCharged = false;
        }
    }
    public void DeactivatePower()
    {
        m_RunnerBody.GetComponent<Renderer>().material = m_DefaultBodyMaterial;
        m_CurrentPowerTime = 0;
        Physics.IgnoreLayerCollision(10, 9, false);
        m_IsPowerActive = false;
        GameManager.Instance.ResetCoinsCollected();
    }
    private void PowerCharged()
    {
        m_IsPowerCharged = true;
    }

    private void OnGameStart()
    {
        distanceTraveled = 0;
    }

    public IEnumerator BlinkPlayerMaterial()
    {
        while (m_IsPowerActive)
        {
            m_RunnerBody.GetComponent<Renderer>().material = m_PoweredBodyMaterial;
            yield return new WaitForSeconds(0.2f);

            m_RunnerBody.GetComponent<Renderer>().material = m_DefaultBodyMaterial;
            yield return new WaitForSeconds(0.2f);
        }
            yield break;
    }

    #region AUDIO / SOUNDS

    private void RightFootOnGround()
    {
        if (!m_IsJumping)
        {
            if (m_FootstepsVolumeScale >= 0 && m_FootstepsVolumeScale <= 1) // check footstep volume scale has a valid value
            {
                m_AudioSource.PlayOneShot(GetRandomFootstepSound(), m_FootstepsVolumeScale);
            }
            else
                Debug.LogError("Invalid footsteps volume scale value !");
        }
        
    }

    private void LeftFootOnGround()
    {
        if (!m_IsJumping)
        {
            if (m_FootstepsVolumeScale >= 0 && m_FootstepsVolumeScale <= 1) // check footstep volume scale has a valid value
            {
                m_AudioSource.PlayOneShot(GetRandomFootstepSound(), m_FootstepsVolumeScale);
            }
            else
                Debug.LogError("Invalid footsteps volume scale value !");
        }
    }

    private AudioClip GetRandomFootstepSound()
    {
        if (m_FootStepBankSound.Count > 0)
        {
            int randomIndex = Random.Range(0, m_FootStepBankSound.Count);
            return m_FootStepBankSound[randomIndex];
        }
        else
            return null;
        
    }

    #endregion

    #region COLLISION / TRIGGER
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            EventManager.TriggerGameOver();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            GameManager.Instance.IncrementObstaclesAvoided();
        }

        if (other.CompareTag("Coin"))
        {
            if(!m_IsPowerActive && !m_IsPowerCharged)
            {
                EventManager.TriggerCoinCollected();
                Destroy(other.gameObject);
            }
            
        }
    }
    #endregion

    #region GETTER / SETTER
    public float GetRunSpeed() { return m_RunSpeed; }
    public bool IsPowerCharged() { return m_IsPowerCharged; }
    public float GetPowerTimer() { return m_PowerTimer; }
    public float GetCurrentPowerTime() { return m_CurrentPowerTime; }
    #endregion

    private void OnDestroy()
    {
        EventManager.GameStart -= OnGameStart;
        EventManager.PowerCharged -= PowerCharged;
        EventManager.ActivatePower -= ActivatePower;
    }

}
