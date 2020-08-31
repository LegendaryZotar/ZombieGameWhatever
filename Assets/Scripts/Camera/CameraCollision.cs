using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;

    CameraManager CM;

	void Start()
    {
        CM = CameraManager.instance;
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
        EventManager.instance.camChangeEvent.AddListener(test);
    }

    void test(StateManager.cameraStates state)
	{
        if(state == StateManager.cameraStates.Normal)
		{
            transform.localEulerAngles = Vector3.zero;
		}
	}

    void Update()
    {
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * CM.distanceToPlayer.y);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit, CM.layersToCheck))
        {
            distance = Mathf.Clamp((hit.distance * CM.distanceBeforeColliding), CM.distanceToPlayer.x, CM.distanceToPlayer.y);
        }
        else { distance = CM.distanceToPlayer.y; }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir *
            (StateManager.getCameraState() == StateManager.cameraStates.Aiming ? CM.aimDistanceToPlayer : 
            StateManager.getMovementState() == StateManager.movementStates.Running ? distance + CM.sprintCamOffset : distance), Time.deltaTime * CM.smooth);

        CM.camDistanceToPlayer = distance;

    }
}
