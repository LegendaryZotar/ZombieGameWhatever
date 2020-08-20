using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransitioner : MonoBehaviour
{
    [ReadOnly] public Vector3 Pstartpos;
    [ReadOnly] public Vector3 Pendpos;
    public float TimeToReach;
    [ReadOnly] public float PPercentage;

    [ReadOnly] public float Pt;

    bool Ptriggered = true;

	private void Start()
	{
        Pstartpos = transform.localPosition;
        Pendpos = Vector3.zero;
	}

	// Update is called once per frame
	void Update()
    {
        if (!Ptriggered)
        {
            PPercentage = Pt / TimeToReach;
            if (PPercentage < 1)
            {
                Pt += Time.deltaTime;
                transform.localPosition = Vector3.Lerp(Pstartpos, Pendpos, PPercentage);
            }
            else
            {
                Ptriggered = true;
                transform.localPosition = Vector3.zero;
            }
        }
    }

    public void Transition()
	{
        Pt = 0;
        Pstartpos = transform.localPosition;
        Pendpos = Vector3.zero;
        Ptriggered = false;
	}
}
