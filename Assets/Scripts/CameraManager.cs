using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineFreeLook CFreeLock;
    public CinemachineBrain CBrain;

    public float XSens;
    public float YSens;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        //Toggles cursor lock using the ESC key.
        if (Input.GetKeyDown(KeyCode.Escape))
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        //Multiplying by 300f for the "steps" to be the same as YSens
        CFreeLock.m_XAxis.m_MaxSpeed = XSens * 300f;
        CFreeLock.m_YAxis.m_MaxSpeed = YSens;
        CBrain.enabled = Cursor.lockState == CursorLockMode.Locked;
    }
}
