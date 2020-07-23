using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class P_Stats : MonoBehaviour
{
    [Header("Health Settings")]
    public float MaxHealth = 100;
    public float Health;
    [ReadOnlyRange(0f, 1f)] public float HealthPercentage = 0.5f;

	/*[Header("Attack Settings")]
    public float Damage;
    public float AttackSpeed;*/

    private void OnValidate()
    {
        HealthPercentage = Health / MaxHealth;
    }

    void Start()
    {
        //Implement method to get health from previous run ?
        Health = MaxHealth;

        HealthPercentage = Health / MaxHealth;
    }

    public void Damage(float Amount)
	{
        if(Health > 0)
            Health -= Amount;

        HealthPercentage = Health / MaxHealth;

        //Play hurt animation

        if(Health <= 0)
		{
            Die();
		}
	}

	void Die()
	{
        //Die stuff
        Debug.Log("You have died!");
	}
}
