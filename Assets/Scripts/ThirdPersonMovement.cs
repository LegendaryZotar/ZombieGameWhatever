using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    
    [Header("Default View Movement")]
    public float DefaultWalkSpeed = 6f;
    public float DefaultRunSpeed = 10f;
    
    [Header("Aim View Movement")]
    public float AimWalkSpeed = 3f;

    float speed;
    bool tryingToMove;

    StateManager SM;

	private void Start()
	{
        SM = StateManager.instance;
	}

	public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0 && SM.canMove|| vertical != 0 && SM.canMove)
            tryingToMove = true;
        else
            tryingToMove = false;


        if (tryingToMove)
        {
            if (SM.getViewState() == StateManager.ViewStateTypes.Normal)
            {
                if (Input.GetKey(KeyCode.LeftShift) && SM.ChangeMovementState(StateManager.MovementStateTypes.Running))
                {
                    speed = DefaultRunSpeed;
                }
                else
                {
                    speed = DefaultWalkSpeed;
                    SM.ChangeMovementState(StateManager.MovementStateTypes.Walking);
                }
            }else
                if(SM.getViewState() == StateManager.ViewStateTypes.Aiming)
			{
                speed = AimWalkSpeed;
			}
		}
		else
		{
            SM.ChangeMovementState(StateManager.MovementStateTypes.Still);
		}



        if (SM.getViewState() == StateManager.ViewStateTypes.Normal)
        {
           Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f && Cursor.lockState == CursorLockMode.Locked)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);

            }
        }else if (SM.getViewState() == StateManager.ViewStateTypes.Aiming)
		{
            Vector3 move = transform.right * horizontal + transform.forward * vertical;
            controller.Move(move * speed * Time.deltaTime);
		}//Else if View is Spectating?
    }
}
