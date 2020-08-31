using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float CameraMoveSpeed = 120.0f;
    public GameObject CameraFollowObj;

    CameraManager CM;
    PlayerManager PM;
    float rotX = 0.0f;

	#region Singleton

	public static CameraFollow instance;

    private void Awake()
    {
        instance = this;
    }

	#endregion

	void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotX = rot.x;
        CM = CameraManager.instance;
        PM = PlayerManager.instance;
    }

    void Update()
    {
        if (CameraManager.canMove())
        {

            float mouseY = Input.GetAxis("Mouse Y");
            
            //Up and down
            rotX -= mouseY *
                (StateManager.getCameraState() == StateManager.cameraStates.Aiming ? CM.aimSens.y : CM.sens.y)
                * 50f * Time.deltaTime;


            rotX = (StateManager.getCameraState() == StateManager.cameraStates.Aiming ?
                Mathf.Clamp(rotX, CM.aimYClamp.x, CM.aimYClamp.y) :
                Mathf.Clamp(rotX, CM.yClamp.x, CM.yClamp.y));

            PM.player.transform.Rotate(0f, Input.GetAxis("Mouse X") *
                (StateManager.getCameraState() == StateManager.cameraStates.Aiming ? CM.aimSens.x : CM.sens.x)
                * Time.deltaTime * 50, 0f);

            Quaternion localRotation = Quaternion.Euler(rotX, PM.player.transform.localEulerAngles.y, 0.0f);
            transform.rotation = localRotation;
        }
    }

    private void LateUpdate()
    {
        if (CameraManager.canMove())
        {
            //Settings camera object to follow
            Transform target = CameraFollowObj.transform;

            //Move towards the target gameobject
            float step = CameraMoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
    }


    Coroutine recoilCoro = null;

    float amount;
    public void AddRecoil(float amount, float duration, float maxRecoil)
    {
        if (recoilCoro != null)
            StopCoroutine(recoilCoro);

        this.amount = Mathf.Clamp(this.amount + amount, 0, maxRecoil);

        recoilCoro = StartCoroutine(_AddOverTime(duration, (a) => rotX -= a));
    }

    IEnumerator _AddOverTime(float duration, System.Action<float> aCallback)
    {
        float t = 0f;
        float step = amount / duration;

        while (t < amount)
        {
            float add = step * Time.deltaTime;
            if (add >= amount - t)
            {
                aCallback(amount - t);
                amount -= t;
                yield break;
            }
            aCallback(add);
            t += add;
            yield return null;
        }
    }
}
