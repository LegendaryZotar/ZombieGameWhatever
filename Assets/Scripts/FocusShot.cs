using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusShot : MonoBehaviour
{
    public Vector3 zoomedOffset;
    public Vector3 normalOffset;

    public Image crosshair;
    //public CinemachineCameraOffset camOffset;
    public ThirdPersonMovement playerMovement;

    StateManager SM;

    private void Start()
    {
        playerMovement = PlayerManager.instance.player.GetComponent<ThirdPersonMovement>();
        SM = StateManager.instance;
    }

    void Update()
    {
        //the if statement should later also require a gun to be equipped to function
        if (Input.GetMouseButtonDown(1))
        {
            ZoomIn();
        }else if (Input.GetMouseButtonUp(1))
        {
            ZoomOut();
        }
    }

    public void ZoomIn()
    {
        //make player turn towards crosshair

        if (SM.ChangeViewState(StateManager.ViewStateTypes.Aiming))
        {
            crosshair = crosshair.GetComponent<Image>();
            var tempColor = crosshair.color;
            tempColor.a = 0.8f;
            crosshair.color = tempColor;
            //PlayerManager.instance.player.transform.TransformDirection(cam.forward);
            //camOffset.m_Offset = zoomedOffset;
        }
    }

    public void ZoomOut()
    {
        //relinquish the turning toward the crosshair
        if (SM.ChangeViewState(StateManager.ViewStateTypes.Normal))
        {
            var tempColor = crosshair.color;
            tempColor.a = 0f;
            crosshair.color = tempColor;
            //camOffset.m_Offset = normalOffset;
        }
    }
}
