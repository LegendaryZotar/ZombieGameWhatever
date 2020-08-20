using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class W_Gun : MonoBehaviour
{
    private float timeSinceLastShot;
    public int currentAmmo;
    
    private Input shootButton;

    //[Header("Gun Settings")]
    public FireRateTypes FireRate_Type;
    public float FireRate = 1.0f;
    public Transform Fire_Point;
    public float Damage = 10f;
    public int AmmoPerShot = 1;
    public int ClipSize = 10;
    public float ReloadTime = 2.0f;

    //[Header("Projectile Settings")]
    public ProjectileTypes Projectile_Type;
    public float Projectile_Force = 100f;
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
        currentAmmo = ClipSize;
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
    

	void Update()
    {

        //shooting
        timeSinceLastShot += Time.deltaTime;
        if(FireRate_Type == FireRateTypes.Automatic)
        {
            Automatic();
        }else
        {
            SemiAutomatic(); 
        }

        //reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine("Reload");
        }        
    
    }

    void Automatic()
    {
        
        if (Input.GetKey(KeyCode.Mouse0) && timeSinceLastShot >= FireRate)
        {
            Shoot();
            timeSinceLastShot = 0f;
        }
        
    }
    void SemiAutomatic()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && timeSinceLastShot >= FireRate)
        {
            Shoot();
            timeSinceLastShot = 0f;
        }
    }

    void Shoot()
    {
        if (currentAmmo > 0)
        {
            currentAmmo -= AmmoPerShot;
            if (Projectile_Type == ProjectileTypes.Raycast)
            {
                RaycastShot();
            }

            if (Projectile_Type == ProjectileTypes.Prefab)
            {
                PrefabShot();
            }
        }
    }

    void RaycastShot()
    {
        RaycastHit hit;
        if (Projectile_Type == ProjectileTypes.Raycast)
        {
            if (Physics.Raycast(Fire_Point.position, Fire_Point.TransformDirection(Vector3.forward), out hit, Projectile_Range))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.GetComponent<Z_Stats>())
                {
                    hit.transform.GetComponent<Z_Stats>().Damage(Damage);

                    //Add a hit effect later on
                }
            }
        }
        else
        {
            //play sound "no ammo"
        }
    }

    void PrefabShot()
    {
        GameObject Bullet = Instantiate(prefab_projectile, Fire_Point.position, Fire_Point.rotation);

        Bullet.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * Projectile_Force);
        Bullet.GetComponent<BulletScript>().bDamage = Damage;
        Bullet.GetComponent<BulletScript>().bRange = Projectile_Range;
    }



    IEnumerator Reload()
    {
        //play reload animation
        yield return new WaitForSeconds(ReloadTime);
        currentAmmo = ClipSize;
        //subtract used ammo from player stash of ammunition
    }



    public enum FireRateTypes { Automatic, Semi_Automatic }
    public enum ProjectileTypes { Raycast, Prefab }
}
