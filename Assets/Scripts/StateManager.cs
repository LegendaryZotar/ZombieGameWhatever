using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    
    public static StateManager instance;

	#region Singleton

	private void Awake()
    {
        instance = this;
    }

    #endregion

	public enum movementStates { Walking, Running, Still, Crawling }
    public enum cameraStates { Normal, Aiming }
    public enum gameStates { Playing, Paused}

    [ReadOnly] public movementStates movementState;
    [ReadOnly] public cameraStates cameraState;
    [ReadOnly] public gameStates gameState;

    void onCamChangeEvent(cameraStates cameraState)
	{
        this.cameraState = cameraState;
	}

    void onMovementChangeEvent(movementStates movementState)
	{
        this.movementState = movementState;
	}

    private void onGameStateChange(gameStates gameState)
    {
        this.gameState = gameState;
    }
    private void Start()
	{
		EventManager.instance.camChangeEvent.AddListener(onCamChangeEvent);
		EventManager.instance.movementStateChangeEvent.AddListener(onMovementChangeEvent);
        EventManager.instance.gameStateChangeEvent.AddListener(onGameStateChange);
    }

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                EventManager.instance.gameStateChangeEvent.Invoke(gameStates.Paused);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                EventManager.instance.gameStateChangeEvent.Invoke(gameStates.Playing);
            }
        }
	}

    //Use this to change MovementState, If you can change it'll also return true, if you can't it'll return false.
    //(Avoid changing the variable directly)
    //Add "Rules" for moving in here
    /*public bool ChangeMovementState(MovementStateTypes state)
	{
		switch (state)
		{
			case MovementStateTypes.Walking:
                if (canMove)
                {
                    MovementState = state;
                    return true;

                } else return false;

			case MovementStateTypes.Running:
                if (canMove && canRun && ViewState != ViewStateTypes.Aiming)
                {
                    MovementState = state;
                    return true;

                } else return false;

            case MovementStateTypes.Still:
                MovementState = state;
                return true;

            default:
                return false;
		}
	}

    //Use this to change ViewState, If you can change it'll also return true, if you can't it'll return false.
    //(Avoid changing the variable directly)
    //Add "Rules" for viewing in here
    public bool ChangeViewState(ViewStateTypes state)
	{
		switch (state)
		{
			case ViewStateTypes.Normal:
                canRun = true;
                ViewState = state;
                UM.disableCrosshair();
                CM.setDefaultMode();
                return true;

			case ViewStateTypes.Aiming:
                if (canAim)
                {
                    canRun = false;
                    ViewState = state;
                    UM.enableCrosshair();
                    CM.setCamAimMode();
                    return true;
                }
                else
                    return false;
			default:
				return false;
		}
	}*/

    public static bool isPaused() { return instance.gameState == gameStates.Paused; }

	public static movementStates getMovementState() { return instance.movementState; }

    public static cameraStates getCameraState() { return instance.cameraState; }

    public static gameStates getGameState() { return instance.gameState; }
}
