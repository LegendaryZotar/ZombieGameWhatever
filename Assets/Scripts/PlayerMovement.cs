using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    
    [Header("Default View Movement")]
    public float defaultWalkSpeed = 6f;
    public float defaultRunSpeed = 10f;
    
    [Header("Aim View Movement")]
    public float defaultAimWalkSpeed = 3f;

    public bool canMove = true;

    float speed;
    bool tryingToMove;

	public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

	private void Start()
	{
        EventManager.instance.gameStateChangeEvent.AddListener(onGameStateChange);
	}

    private void onGameStateChange(StateManager.gameStates state)
    {
        switch (state)
        {
            case StateManager.gameStates.Playing:
                canMove = true;
                break;
            case StateManager.gameStates.Paused:
                canMove = false;
                break;
        }
    }



    void Update()
    {
        if (canMove)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (horizontal != 0 || vertical != 0)
                tryingToMove = true;
            else
                tryingToMove = false;

            if (tryingToMove)
            {
                if (Input.GetKey(KeyCode.LeftShift) && StateManager.getCameraState() == StateManager.cameraStates.Normal)
                {
                    invokeMoveState(StateManager.movementStates.Running);
                    speed = defaultRunSpeed;
                }
                else if (Input.GetKey(KeyCode.LeftControl))
                {
                    invokeMoveState(StateManager.movementStates.Crawling);
                    //speed = crawlSpeed;
                }
                else
                {
                    invokeMoveState(StateManager.movementStates.Walking);
                    if (StateManager.getCameraState() == StateManager.cameraStates.Aiming)
                        speed = defaultAimWalkSpeed;
                    else
                        speed = defaultWalkSpeed;
                }

                Vector3 move = transform.right * horizontal + transform.forward * vertical;
                controller.Move(move * speed * Time.deltaTime);

                return;
            }
        }
        invokeMoveState(StateManager.movementStates.Still);
        speed = 0;
    }

    public void invokeMoveState(StateManager.movementStates state)
    {
        if (StateManager.getMovementState() != state)
            EventManager.instance.movementStateChangeEvent.Invoke(state);
    }
}