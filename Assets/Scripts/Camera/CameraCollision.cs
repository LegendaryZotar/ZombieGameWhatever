using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;

    CameraManager CM;
    StateManager SM;

    [Range(0f, 100f)] public float Dist;
    [Range(0f, 100f)] public float Dist2;


	private void OnDrawGizmos()
	{
        Gizmos.color = new Color(128, 0, 128);
        //Gizmos.DrawRay(PlayerManager.instance.player.transform.position + new Vector3(0f,2f,0f), PlayerManager.instance.player.transform.forward * 1000f);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + transform.forward * 3f, transform.forward * 1000f);
        RaycastHit hit;
        int layerMask = ~LayerMask.GetMask("CameraIgnore");
        if (Physics.Raycast(transform.position + transform.forward * 4, transform.forward, out hit, 100, layerMask))
        {
            Gizmos.DrawWireSphere(hit.point, 0.5f);
            Vector3 point = hit.point;

			/*point.z = 0f;
            point.x = 0f;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(point, 1);*/
		}
		else
		{
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position + transform.forward * Dist, 0.5f);
		}

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 1000f);
        RaycastHit hit2;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit2, 100, layerMask))
		{
            Gizmos.DrawWireSphere(hit2.point, 0.3f);
		}else
            Gizmos.DrawWireSphere(Camera.main.transform.position + transform.forward * Dist2, 0.5f);
    }

	void Start()
    {
        CM = CameraManager.instance;
        SM = StateManager.instance;
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    void Update()
    {
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * CM.DistanceToPlayer.y);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit, CM.LayersToCheck))
        {
            distance = Mathf.Clamp((hit.distance * CM.DistanceBeforeColliding), CM.DistanceToPlayer.x, CM.DistanceToPlayer.y);
        }
        else { distance = CM.DistanceToPlayer.y; }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir *
            (SM.getMovementState() == StateManager.MovementStateTypes.Running ? distance + CM.SprintCamOffset : distance), Time.deltaTime * CM.Smooth);

        CM.CamDistanceToPlayer = distance;

    }
}
