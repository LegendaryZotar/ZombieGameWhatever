using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z_Stats : MonoBehaviour
{

    [Header("Health Settings")]
    public float MaxHealth = 100;
    public float Health;
	[ReadOnlyRange(0f, 1f)] public float HealthPercentage = 0.5f;

    [Header("Attack Settings")]
    public float DMG = 5f; //Damage
    public float HitInterval = 0.5f; //Interval between hits in seconds.
    float delay;

    void Start()
    {
        //Implement method to get health from previous run ?
        Health = MaxHealth;

        HealthPercentage = Health / MaxHealth;
    }

    private void Update()
	{
		if(delay > 0)
		{
            delay -= Time.deltaTime;
		}
	}

	private void OnTriggerStay(Collider other)
	{
        if (other.tag == "Player")
        {
            if (delay <= 0)
            {
                other.GetComponent<P_Stats>().Damage(DMG);
                //HitAnimation
                delay = HitInterval;
            }
        }
	}

	private void OnValidate()
    {
        HealthPercentage = Health / MaxHealth;
    }

    public void Damage(float Amount)
    {
        Health -= Amount;

        HealthPercentage = Health / MaxHealth;

        //Play hurt animation

        if (Health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Die stuff
        Destroy(gameObject);
    }
}
