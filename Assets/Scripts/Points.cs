using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
    public enum Point_Type 
    {
        StartPoint, EndPoint
    };

    public Point_Type PointType;

    void Start()
    {
        if (PointType == Point_Type.StartPoint)
        {
            PlayerManager.instance.player.transform.position = transform.position;
        } else if (PointType == Point_Type.EndPoint)
		{
            if (GetComponent<Collider>() != null)
            {
                Collider temp = GetComponent<Collider>();
                if (temp.enabled)
                {
                    if (!temp.isTrigger)
                        Debug.LogError("EndPoint colliders must be triggers");
                }
                else
                    Debug.LogError("EndPoint colliders must be active!");
            }
            else
                Debug.LogError("EndPoint must have a collider to work!");
		}
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.tag == "Player")
        {
            if (PointType == Point_Type.EndPoint)
            {
                //Do end stuff
                Debug.Log("You have reached the end");
            }
        }
	}
}
