using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    CameraFollow CF;

    public float interactionDistance = 5f;

    private void Start()
    {
        CF = CameraFollow.instance;
    }

    public void Update()
    {
        //shoot a raycast at what it's looking
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            
            RaycastHit hit;
            Physics.Raycast(CF.transform.position, CF.transform.TransformDirection(Vector3.forward), out hit, interactionDistance, LayerMask.NameToLayer("CamerIgnore"));
            var interactable = hit.transform.GetComponent<Item>();
            if (hit.transform.GetComponent<Item>() != null)
            interactable.Interact(gameObject);
            
            
        }

    }
}
