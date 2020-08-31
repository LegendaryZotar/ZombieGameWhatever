using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD.MinMaxSlider;
using System;
using System.Runtime.CompilerServices;

public class CameraManager : MonoBehaviour
{
    //[Header("Default View Settings")]
    public Vector2 sens = new Vector2(4, 4);
    [MinMaxSlider(1, 10)] public Vector2 distanceToPlayer = new Vector2(2,4);
    [MinMaxSlider(-90, 90)] public Vector2 yClamp = new Vector2(-20, 35);

    //[Header("Advanced Settings")]
    public float smooth = 10.0f;
    public float sprintCamOffset = 1;
    public float distanceBeforeColliding = 0.87f;
    public LayerMask layersToCheck;

    //[Header("Aim View Settings")]
    public Vector2 aimSens = new Vector2(2,2);
    public float aimDistanceToPlayer = 1;
    [MinMaxSlider(-90, 90)] public Vector2 aimYClamp = new Vector2(-20, 35);

    //[Header("Debug")]
    public bool raycastDirection = false;
    public float directionDistance = 10f;
    public Color directionColor = Color.white;

    //[Header("Info")]
    [ReadOnly] public float camDistanceToPlayer;
    [ReadOnly] public bool canMoveCam = true;

    #region Singleton

    public static CameraManager instance;

    private void Awake()
    {
        instance = this;
    }

	#endregion

	private void OnDrawGizmos()
	{
        if (raycastDirection)
        {
            Transform cam = Camera.main.transform;
            Gizmos.color = directionColor;
            Gizmos.DrawRay(cam.position, cam.forward * directionDistance);
        }
        Gizmos.color = Color.white;
	}

	void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        EventManager.instance.gameStateChangeEvent.AddListener(onGameStateChange);
    }

    private void onGameStateChange(StateManager.gameStates state)
	{
		switch (state)
		{
			case StateManager.gameStates.Playing:
                canMoveCam = true;
				break;
			case StateManager.gameStates.Paused:
                canMoveCam = false;
                break;
		}
	}


    void Update()
    {
        if (canMoveCam)
        {
            if (Input.GetMouseButtonDown(1))
            {
                EventManager.instance.camChangeEvent.Invoke(StateManager.cameraStates.Aiming);
            }
            
            if (Input.GetMouseButtonUp(1))
            {
                EventManager.instance.camChangeEvent.Invoke(StateManager.cameraStates.Normal);
            }
        }
    }

    public static bool canMove()
	{
        return instance.canMoveCam;
	}
}
