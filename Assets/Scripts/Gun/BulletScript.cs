using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public float bMass = 20.0f;
    public float bDamage;
    public float bRange;

    public Vector3 startPoint;
    
    void Start()
    {
        //convert given mass to what Unity uses
        bMass /= 1000;

        //change mass of bullet to what we've got in the variable
        gameObject.GetComponent<Rigidbody>().mass = bMass;

        startPoint = gameObject.transform.position;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.GetComponent<Z_Stats>())
        {
            other.gameObject.GetComponent<Z_Stats>().Damage(bDamage);
            //collision effect here
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        if(Vector3.Distance(startPoint,transform.position) > bRange)
        {
            Destroy(gameObject);
        }
    }
}
