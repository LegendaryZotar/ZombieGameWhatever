using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class W_Gun : MonoBehaviour
{
    //[Header("Gun Settings")]
    public FireRateTypes FireRate_Type;
    public float RPS = 2.5f;
    public Transform Fire_Point;

    //[Header("Projectile Settings")]
    public ProjectileTypes Projectile_Type;
    public float Projectile_Speed = 5f;
    public float Projectile_Range = 5f;

    //[Header("Other Settings")]
    public bool CustomCrossHair;
    public Sprite CrossHair;

    //ProjectileType_Prefab
    public GameObject prefab_projectile;
    public bool Parent;
    public Transform prefab_parent;

    //ProjectileType_Raycast
    public Color raycast_Color = Color.white;
    public bool raycast_show;

    void Start()
    {
    }

	private void OnDrawGizmos()
	{
        Camera cam = Camera.main;
        if (raycast_show)
        {
            Gizmos.color = raycast_Color;
            Ray endPoint = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            Gizmos.DrawLine(endPoint.origin, endPoint.origin + endPoint.direction * 10f);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(endPoint.GetPoint(Projectile_Range), 0.1f);
            
        }
	}

	// Update is called once per frame
	void Update()
    {
    }

    public enum FireRateTypes { Automatic, Semi_Automatic }
    public enum ProjectileTypes { Raycast, Prefab }
}
