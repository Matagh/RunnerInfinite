using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Runner m_Runner;

    [SerializeField] float m_MinSwipeDistX;
    [SerializeField] float m_MinSwipeDistY;

    private Vector2 m_StartTouchPosition;

    void Awake()
    {
        m_Runner = gameObject.GetComponent<Runner>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("LeftDash") && m_Runner)
        {
            m_Runner.DashLeft();
        }

        if (Input.GetButtonDown("RightDash") && m_Runner)
        {
            m_Runner.DashRight();
        }

        if (Input.GetButtonDown("Jump") && m_Runner)
        {
            m_Runner.Jump();
        }

        if (Input.GetButtonDown("UsePower") && m_Runner)
        {
            EventManager.TriggerActivatePower();
        }

        if (Input.GetButtonDown("Pause") && m_Runner)
        {
            EventManager.TriggerGamePause();
        }

        #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

        SwipeHandler();

        #endif
    }

    #region     MOBILE CONTROLS

    void SwipeHandler()
    {
        if (Input.touchCount > 0)
        {
            Touch currentTouch = Input.touches[0];

            if(currentTouch.phase == TouchPhase.Began)
            {
                m_StartTouchPosition = currentTouch.position;
            }
            else if (currentTouch.phase == TouchPhase.Ended)
            {
                float swipeDistVertical = (new Vector3(0, currentTouch.position.y, 0) - new Vector3(0, m_StartTouchPosition.y, 0)).magnitude;
                float swipeDistHorizontal = (new Vector3(currentTouch.position.x, 0, 0) - new Vector3(m_StartTouchPosition.x, 0, 0)).magnitude;

                
                if(swipeDistVertical > m_MinSwipeDistY)
                {
                    float swipeValue = Mathf.Sign(currentTouch.position.y - m_StartTouchPosition.y);
                    if(swipeValue > 0) // UP SWIPE
                    {
                        m_Runner.Jump();
                    }
                }
                else if (swipeDistHorizontal > m_MinSwipeDistX)
                {
                    float swipeValue = Mathf.Sign(currentTouch.position.x - m_StartTouchPosition.x);
                    if (swipeValue > 0) // right swipe
                    {
                        m_Runner.DashRight();
                    }
                    else if (swipeValue < 0)// left  swipe
                    {
                        m_Runner.DashLeft();
                    }
                }
            }
        }


    }

    #endregion

}
