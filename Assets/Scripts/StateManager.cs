using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    #region Singleton

    public static StateManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    // Start is called before the first frame update
    public enum MovementStateTypes { Walking, Running, Still }
    public enum ViewStateTypes { Normal, Aiming }

    [ReadOnly] public MovementStateTypes MovementState;
    [ReadOnly] public ViewStateTypes ViewState;

    public bool canRun = true;
    public bool canMove = true;
    public bool canAim = true;

    CameraManager CM;
    UIManager UM;

	private void Start()
	{
        CM = CameraManager.instance;
        UM = UIManager.instance;
	}

	private void Update()
	{
        if (Input.GetMouseButtonDown(1))
        {
            ChangeViewState(ViewStateTypes.Aiming);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            ChangeViewState(ViewStateTypes.Normal);
        }
    }

	//Use this to change MovementState, If you can change it'll also return true, if you can't it'll return false.
	//(Avoid changing the variable directly)
	//Add "Rules" for moving in here
	public bool ChangeMovementState(MovementStateTypes state)
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
	}

    public MovementStateTypes getMovementState() { return MovementState; }

    public ViewStateTypes getViewState() { return ViewState; }
}
