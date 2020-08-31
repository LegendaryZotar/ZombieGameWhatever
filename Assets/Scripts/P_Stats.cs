using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class P_Stats : MonoBehaviour
{
    bool isInfected = false;
    float currentCooldown; //current state the cooldown for each infection increase is at
    float DamageReduction = 3;
    float SpeedReduction = 2;
    float HealthCapReduction = 1; //Health cap reduction per second
    ThirdPersonMovement movement;

    [Header("Health Settings")]
    public float MaxHealth = 100;
    public float Health;
    public float InfectionIncrease; //Increase in infection state
    public float infectionCooldown; //Time in seconds for each infection increase to happen
    [ReadOnlyRange(0f, 1f)] public float HealthPercentage = 0.5f;
    [ReadOnlyRange(0, 100)] public float InfectionPercentage = 0;
    [ReadOnly] public int InfectionState = 0;

	[Header("Attack Settings")]
    public float Damage;
    public float AttackSpeed;

    private void OnValidate()
    {
        HealthPercentage = Health / MaxHealth;
    }

    void Start()
    {
        //Implement method to get health from previous run ?
        Health = MaxHealth;

        HealthPercentage = Health / MaxHealth;

        movement = gameObject.GetComponent<ThirdPersonMovement>();
    }

    private void Update()
    {
        if(isInfected == true)
        {
            infected();
        }
        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;

        if (Health > MaxHealth)
            Health = MaxHealth;
    }

    public void TakeDamage(float Amount, int infectionChance)
	{
        if(Health > 0)
            Health -= Amount;

        HealthPercentage = Health / MaxHealth;

        //Play hurt animation

        int i = Random.Range(0, 100);
        if (i <= infectionChance)
        {
            isInfected = true;
        }

        if(Health <= 0)
		{
            Die();
		}
	}

    void infected()
    {
        int currentInfectionState = InfectionState;

        if (currentCooldown <= 0)
        {
            InfectionPercentage += InfectionIncrease;
            currentCooldown = infectionCooldown;
        }
        if (InfectionState < 30)
            InfectionState = 1;
        if (InfectionPercentage >= 30)
            InfectionState = 2;
        if (InfectionPercentage >= 50)
            InfectionState = 3;
        if (InfectionPercentage >= 80)
            InfectionState = 4;
        if (InfectionPercentage >= 100)
            InfectionState = 5;

        if (currentInfectionState != InfectionState)
            InfectionStage();

    }

    void InfectionStage()
    {

        if(InfectionState == 0)
        {
            //not infected
        }
        if(InfectionState == 1)
        {
            //harmless stuff here
        }
        if(InfectionState == 2)
        {
            //weakness
            Damage -= DamageReduction;
        }
        if(InfectionState == 3)
        {
            //slowness
            movement.DefaultRunSpeed -= SpeedReduction;
            movement.DefaultWalkSpeed -= SpeedReduction;
            //nausea
            //reduce stamina cap
            //reduce health cap
        }
        if(InfectionState == 4)
        {
            //weakness 2
            Damage -= DamageReduction;
            //slowness 2
            movement.DefaultRunSpeed -= SpeedReduction;
            movement.DefaultWalkSpeed -= SpeedReduction;
            //nausea 2
            //reduce stamina cap
            //reduce health cap
        }
        if (InfectionState == 5)
        {
            //nausea 2
            //reduce stamina cap
            //reduce health cap
            //rapid health reduction
        }
    }

	void Die()
	{
        //Die stuff
        Debug.Log("You have died!");
	}
}
