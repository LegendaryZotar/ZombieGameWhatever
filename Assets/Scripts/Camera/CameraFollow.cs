using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float CameraMoveSpeed = 120.0f;
    public GameObject CameraFollowObj;


    CameraManager CM;

    [ReadOnly] public float rotY = 0.0f;
    public float rotX = 0.0f;



    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotX = rot.x;
        rotY = rot.y;

        CM = CameraManager.instance;
    }

    void Update()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
       // Debug.Log(Mathf.Clamp(-1, CM.YClamp.x, CM.YClamp.y));
        rotY = fix(rot.y);
		if (Input.GetKeyDown(KeyCode.J))
		{
            Debug.Log("a");
            rotX -= 30;
		}
        if (CM.canMove())
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            //Left and right
            //rotY += (mouseX * CM.XAimSens * 50f * Time.deltaTime);
            addRot((mouseY * CM.YSens * 50f * Time.deltaTime), (mouseX * CM.XAimSens * 50f * Time.deltaTime));

            //Up and down
           // rotX -= mouseY * CM.YSens * 50f * Time.deltaTime;

            //rotX = Mathf.Clamp(rotX, CM.YClamp.x, CM.YClamp.y);

            Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            //Debug.Log(rotX);
            transform.rotation = localRotation;
        }
    }

    float fix(float f)
	{
        if (f > 180)
		{
            f -= 360f;
            return f;
		}else if(f < -180)
		{
            f += 360f;
            return f;
		}
        return f;
	}

    public void resetRot()
	{
        //Debug.Log("Before : X: " + rotX + " Y: " + rotY);
        Vector3 rot = transform.localRotation.eulerAngles;
        //Debug.Log("rot: " + rot);
        rotX = Mathf.Clamp( rot.x > 270 ? rot.x - 360 : rot.x , CM.YClamp.x, CM.YClamp.y);
        rotY = fix(rot.y);
        //Debug.Log("After : X: " + rotX + " Y: "+  rotY);
    }

	private void LateUpdate()
	{
        CameraUpdater();
	}

    public void addRot(float vertical, float horizontal)
	{
        float temp1 = rotX;
        rotX -= vertical;
        rotY += horizontal;

        float temp2 = rotX;
        rotY = fix(rotY);
        rotX = Mathf.Clamp(rotX, CM.YClamp.x, CM.YClamp.y);
    }

    public Vector2 getRot() { return new Vector2(rotX, rotY); }

    void CameraUpdater()
	{
        if (CM.canMove())
        {
            //Settings camera object to follow
            Transform target = CameraFollowObj.transform;

            //Move towards the target gameobject
            float step = CameraMoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
	}
}
