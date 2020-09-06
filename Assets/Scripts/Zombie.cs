using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{

    Transform target;
    NavMeshAgent agent;
    Vector3 rayDir;

    bool playerDetected=false;
    float timeSinceLastSeenPlayer;

    public GameObject PlayerTracker;

    [Header("Zombie Settings")]
    public float Speed;
    public int InfChance = 20;

    [Header("Health Settings")]
    public float MaxHealth = 100;
    public float Health;
	[ReadOnlyRange(0f, 1f)] public float HealthPercentage = 0.5f;

    [Header("Attack Settings")]
    public float DMG = 5f; //Damage
    public float HitInterval = 0.5f; //Interval between hits in seconds.
    public float AttackRange = 2f;
    float delay;

    [Header("Perception Settings")]
    public float lookRange = 10f; //From how far away he can see the player
    public float persistency = 3f; //How long the Zombie will follow the player even when out of sight (in seconds)


    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();

        //Implement method to get health from previous run ?
        Health = MaxHealth;

        HealthPercentage = Health / MaxHealth;

        agent.speed = Speed;
        agent.stoppingDistance = AttackRange + 0.2f;
    }

    private void Update()
	{
        timeSinceLastSeenPlayer += Time.deltaTime;
        LookForTarget(target.tag);
        if (playerDetected)
        {
            MoveToTarget();
        }

        delay -= Time.deltaTime;
	}

    void LookForTarget(string TargetTag)
    {
        RaycastHit hit;

        Vector3 trackerDir = (target.position - transform.position).normalized;
        Quaternion trackerLookRot = Quaternion.LookRotation(new Vector3(trackerDir.x, 0, trackerDir.z));
        PlayerTracker.GetComponent<Transform>().rotation = trackerLookRot;

        if (Physics.Raycast(transform.position, PlayerTracker.transform.forward, out hit, lookRange))
        {
            if(hit.transform.tag == TargetTag)
            {
                playerDetected = true;
                timeSinceLastSeenPlayer = 0f;
            }
        }

        if (timeSinceLastSeenPlayer >= persistency && !Physics.Raycast(transform.position, PlayerTracker.transform.forward, out hit, lookRange))
        {
            playerDetected = false;
        }
    }

	private void OnValidate()
    {
        HealthPercentage = Health / MaxHealth;
    }

    void MoveToTarget()
    {
        float distance = Vector3.Distance(target.position, transform.position);


        Debug.Log(distance);
        //Attack the player
        if (distance <= AttackRange)
        {
            if (delay <= 0)
            {
                DealDamage();
            }
            FaceTarget();


        }else
        {
            agent.SetDestination(target.position);
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
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

    void DealDamage()
    {
        target.GetComponent<P_Stats>().TakeDamage(DMG, InfChance);
        delay = HitInterval;
    }

    void Die()
    {
        //Die stuff
        Destroy(gameObject);
    }

}
