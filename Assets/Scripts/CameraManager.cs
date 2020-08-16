using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD.MinMaxSlider;
using System;
using System.Runtime.CompilerServices;

public class CameraManager : MonoBehaviour
{
    Camera cam;
    StateManager SM;
    PlayerManager PM;

    //[Header("Default View Settings")]
    public float XSens;
    public float YSens;
    [MinMaxSlider(1, 10)] public Vector2 DistanceToPlayer;
    [MinMaxSlider(-90, 90)] public Vector2 YClamp;
    //[Header("Advanced Settings")]
    public float Smooth = 10.0f;
    public LayerMask LayersToCheck;
    public float SprintCamOffset;
    public float DistanceBeforeColliding = 0.87f;
    public float DefaultMaxViewAlignment = 100f;

    //[Header("Aim View Settings")]
    public float XAimSens = 2;
    public float YAimSens = 2;
    public float YOffsetOnAim = 0.1f;
    [MinMaxSlider(-90, 90)] public Vector2 AimClamp;
    [MinMaxSlider(0,1)] public Vector2 AimExitClamp;

    //[Header("Advanced Settings)]
    public float AimMaxViewAlignment = 100f;

    //[Header("Info")]
    [ReadOnly] public float CamDistanceToPlayer;
    [ReadOnly] public bool CanMoveCam = true;

    #region Singleton

    public static CameraManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SM = StateManager.instance;
        cam = Camera.main;
        PM = PlayerManager.instance;
    }

    void Update()
    {

        //Debug.Log((-cam.transform.forward.y + 1) / 2);
        if (SM.getViewState() == StateManager.ViewStateTypes.Aiming)
        {
            GunCamMove();
        }
        //else if (cam.transform.parent != null) cam.transform.parent = null;

        //Toggles cursor lock using the ESC key.
        if (Input.GetKeyDown(KeyCode.Escape))
		{
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
            
            if (isLocked())
                UnlockMovement();
            else
                LockMovement();
        }

        //Multiplying by 300f for the "steps" to be the same as YSens
        /*if (CFreeLock.enabled)
        {
            CFreeLock.m_XAxis.m_MaxSpeed = XSens * 300f;
            CFreeLock.m_YAxis.m_MaxSpeed = YSens;
        }*/
    }

    void GunCamMove()
    {
        float temp = cam.transform.localEulerAngles.x > 90 ? cam.transform.localEulerAngles.x - 360f : cam.transform.localEulerAngles.x;
        cam.transform.localEulerAngles = new Vector3(Mathf.Clamp(temp + -Input.GetAxis("Mouse Y") * YAimSens * Time.deltaTime * 50f, AimClamp.x, AimClamp.y), 0, 0f);
        PM.player.transform.Rotate(0f, Input.GetAxis("Mouse X") * XAimSens * Time.deltaTime * 50, 0f);
    }

    Transform temp;
    public void setCamAimMode()
	{
        temp = cam.transform.parent;
        cam.transform.parent = GunManager.instance.ActiveAim;
        cam.GetComponent<CameraTransitioner>().Transition();

        cam.transform.localScale = Vector3.one;

        //Adjust This... (layerMask)
        int layerMask = ~LayerMask.GetMask("CameraIgnore");
        RaycastHit hit;
        //Sending raycast out from 3rd person camera point (CameraBase)
        if(Physics.Raycast(temp.parent.position, temp.parent.forward, out hit, DefaultMaxViewAlignment, layerMask))
		{
            Debug.Log("First: " + cam.transform.position);
            Vector3 point = hit.point;
            cam.transform.LookAt(point);
            cam.transform.rotation = Quaternion.Euler(cam.transform.localEulerAngles.x, 0f, 0f);

            //Debug.Log("Supposed to");

            Vector2 p1 = new Vector2(hit.point.x, hit.point.z);
            Vector2 p2 = new Vector2(cam.transform.parent.position.x, cam.transform.parent.position.z);
            Vector2 p3 = new Vector2(cam.transform.parent.position.x + cam.transform.parent.forward.x, cam.transform.parent.position.z + cam.transform.parent.forward.z);

            float angle = findAngle(p1, p2, p3);

            //Debug.Log(angle);

            PM.player.transform.Rotate(Vector3.up, angle);
        }
		else
		{
            //Making camera look at 3RD person max view alignment point
            cam.transform.LookAt(temp.parent.position + temp.parent.forward * DefaultMaxViewAlignment);
            //PM.player.transform.LookAt(temp.parent.position + temp.parent.forward * DefaultMaxViewAlignment);
            //PM.player.transform.rotation = Quaternion.Euler(0f, PM.player.transform.localEulerAngles.y, 0f);
            //Debug.Log("Second");
            //Debug.Log((temp.parent.position + temp.parent.forward * DefaultMaxViewAlignment));
            cam.GetComponent<CameraTransitioner>().Transition();
        }
        Debug.Log("Second: " + cam.transform.position);

    }

    public void setDefaultMode()
	{

         int layerMask = ~LayerMask.GetMask("CameraIgnore");
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, AimMaxViewAlignment, layerMask))
        {
            Vector3 point = hit.point;
            temp.parent.LookAt(point);
            //Debug.Log("First: " + hit.point);
            temp.parent.GetComponent<CameraFollow>().resetRot();
            temp.parent.rotation = Quaternion.Euler(temp.parent.localEulerAngles.x, temp.parent.localEulerAngles.y, 0f);
        }
        else
        {
            temp.parent.LookAt(cam.transform.position + cam.transform.forward * AimMaxViewAlignment);
            temp.parent.GetComponent<CameraFollow>().resetRot();
            //Debug.Log("Second: " + cam.transform.position + cam.transform.forward * AimMaxViewAlignment);
        }

        cam.transform.parent = temp;
        cam.transform.localEulerAngles = Vector3.zero;

        //cam.transform.localPosition = Vector3.zero;
        cam.GetComponent<CameraTransitioner>().Transition();
        
        cam.transform.localScale = Vector3.one;
        cam.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    float findAngle(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float rads =  Mathf.Atan2(p3.y - p2.y, p3.x - p2.x) - 
                      Mathf.Atan2(p1.y - p2.y, p1.x - p2.x);
        return rads * Mathf.Rad2Deg;
    }

    //Use this function to attempt to lock other scripts
    public void LockMovement(bool Override = false)
	{
        if (Override) { CanMoveCam = false; return; }

        CanMoveCam = false;
	}

    //Use this function to attempt to lock other scripts
    public void UnlockMovement(bool Override = false)
	{
        if (Override) { CanMoveCam = true; return; }

        if(isLocked())
            CanMoveCam = true;
    }

    //Use this function to check if camera can move from other functions
    public bool canMove() { return CanMoveCam; }

    bool isLocked()
	{
        return Cursor.lockState == CursorLockMode.Locked;

    }

    float FindDegree(int x, int y)
    {
        float value = (float)((Mathf.Atan2(x, y) / Mathf.PI) * 180f);
        if (value < 0) value += 360f;

        if (value > 180)
        {
            return value - 360;
        }
        return value;
    }
}
