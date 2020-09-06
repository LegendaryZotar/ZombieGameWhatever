using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class W_Gun : MonoBehaviour
{
    private float timeSinceLastShot;
    public int currentAmmo;
    
    private Input shootButton;

    GunManager GM;

    //[Header("Gun Settings")]
    public FireRateTypes fireRateType;
    public float fireRate = 1.0f;
    public float reloadTime = 2.0f;
    public float recoil = 5f;
    public Transform firePoint;

    //[Header("Ammo Settings")]
    public int ammoPerShot = 1;
    public int clipSize = 10;

    //[Header("Projectile Settings")]
    public ProjectileTypes projectType;
    public GameObject projectilePrefab;
    public float projectileForce = 100f;
    public float projectileRange = 5f;
    public float damage = 10f;

    //[Header("Other Settings")]
    public bool customCrossHairBool;
    public GunManager.CrossHair customCrossHair;

    //ProjectileType_Prefab

    //ProjectileType_Raycast
    public bool raycastDirection = false;
    public Color directionColor = Color.white;

    void Start()
    {
        GM = GunManager.instance;

        currentAmmo = clipSize;
    }

	private void OnDrawGizmos()
	{
        if (raycastDirection)
        {
            Gizmos.color = directionColor;
            Gizmos.DrawRay(firePoint.position, firePoint.forward * projectileRange);
        }

        Gizmos.color = Color.white;
	}
    

	void Update()
    {

        //shooting
        timeSinceLastShot += Time.deltaTime;
        if(fireRateType == FireRateTypes.Automatic)
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
        
        if (Input.GetKey(KeyCode.Mouse0) && timeSinceLastShot >= fireRate)
        {
            Shoot();
            timeSinceLastShot = 0f;
        }
        
    }
    void SemiAutomatic()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && timeSinceLastShot >= fireRate)
        {
            Shoot();
            timeSinceLastShot = 0f;
        }
    }

    void Shoot()
    {
        if (currentAmmo > 0)
        {
            currentAmmo -= ammoPerShot;
            if (projectType == ProjectileTypes.Raycast)
            {
                RaycastShot();
            }

            GameObject bullet = null;

            if (projectType == ProjectileTypes.Prefab)
            {
                bullet = PrefabShot();
            }

            EventManager.instance.playerAttemptShootEvent.Invoke(this, EventManager.PlayerAttemptShootEvent.result.SUCCESSFUL, bullet);

            GM.AddRecoil(recoil);
        }
        else
        {
            EventManager.instance.playerAttemptShootEvent.Invoke(this, EventManager.PlayerAttemptShootEvent.result.FAILED_NOAMMO, null);
            //play sound "no ammo"
        }
    }

    void RaycastShot()
    {
        RaycastHit hit;

        if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit, projectileRange))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.GetComponent<Zombie>())
            {
                hit.transform.GetComponent<Zombie>().Damage(damage);

                //Add a hit effect later on
            }
        }

    }

    GameObject PrefabShot()
    {
        GameObject Bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Bullet.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * projectileForce);
        Bullet.GetComponent<BulletScript>().bDamage = damage;
        Bullet.GetComponent<BulletScript>().bRange = projectileRange;

        return Bullet;
    }



    IEnumerator Reload()
    {
        //play reload animation
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = clipSize;
        //subtract used ammo from player stash of ammunition
    }



    public enum FireRateTypes { Automatic, Semi_Automatic }
    public enum ProjectileTypes { Raycast, Prefab }
}
