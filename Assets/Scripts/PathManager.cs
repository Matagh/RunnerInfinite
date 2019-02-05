using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    private Queue<PathSection> m_PathQueue;
    [SerializeField] private int m_NumberOfSections = 3;
    [SerializeField] private float m_DistanceBetweenSections = 10;
    [SerializeField] private GameObject m_SectionPrefab;
    [SerializeField] private Transform m_Player;


    // Use this for initialization
    void Start ()
    {
        m_PathQueue = new Queue<PathSection>(m_NumberOfSections);
        float sectionPositionZ = 10f;
        for(int i=0; i< m_NumberOfSections; i++)
        {
            GameObject newSectionObj = Instantiate(m_SectionPrefab, new Vector3(transform.position.x, transform.position.y, sectionPositionZ), Quaternion.identity);
            m_PathQueue.Enqueue(newSectionObj.GetComponent<PathSection>());
            sectionPositionZ += m_DistanceBetweenSections;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(m_Player.transform.position.z > m_PathQueue.Peek().transform.position.z + m_DistanceBetweenSections)
        {
            RecyclePath();
        }
	}

    void RecyclePath()
    {
        // Create new section gameObject
        GameObject newSectionObj = Instantiate(m_SectionPrefab, 
            new Vector3(transform.position.x, transform.position.y, m_PathQueue.Peek().transform.position.z + m_DistanceBetweenSections* m_NumberOfSections), 
            Quaternion.identity);

        // Add it to the queue
        m_PathQueue.Enqueue(newSectionObj.GetComponent<PathSection>());
        // Destroy old section gameObject
        Destroy(m_PathQueue.Peek().gameObject);
        // Remove old section from queue
        m_PathQueue.Dequeue();
    }
}
