using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSection : MonoBehaviour
{
    public Transform m_ObstacleSpot;
    public Transform m_FrontCoinsSpot;
    public Transform m_BehindCoinsSpot;

    public List<GameObject> m_ObstacleTypesArray;
    public List<GameObject> m_CoinPatternTypesArray;

    public bool m_IsSpawningObstacle = true;
    public bool m_IsSpawningCoins = true;

    private GameObject m_Obstacle;
    private GameObject m_CoinPattern;

    // Use this for initialization
    void Start ()
    {
        if(m_IsSpawningObstacle)
            GenerateObstacles();

        if (m_IsSpawningCoins)
        {
            GenerateCoins(m_FrontCoinsSpot);
            GenerateCoins(m_BehindCoinsSpot);
        }


    }
	
	// Update is called once per frame
	void Update ()
    {
        //if (Input.GetButtonDown("Jump"))
        //{
        //    GenerateObstacles();
        //}
	}

    public void GenerateObstacles()
    {
        if (m_Obstacle)
        {
            Destroy(m_Obstacle);
        }
        int randomIndex = Random.Range(0, m_ObstacleTypesArray.Count);
        m_Obstacle = Instantiate(m_ObstacleTypesArray[randomIndex], m_ObstacleSpot);
    }

    public void GenerateCoins(Transform _spotTransform)
    {
        if (m_CoinPattern)
        {
            Destroy(m_CoinPattern);
        }
        int randomIndex = Random.Range(0, m_CoinPatternTypesArray.Count);
        m_CoinPattern = Instantiate(m_CoinPatternTypesArray[randomIndex], _spotTransform);
    }


}
