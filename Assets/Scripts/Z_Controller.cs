using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Z_Controller : MonoBehaviour
{

    Transform target;
    NavMeshAgent agent;
    Vector3 rayDir;

    bool playerDetected=false;
    float timeSinceLastSeenPlayer;

    public GameObject PlayerTracker;

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
    public float persistensy = 3f; //How long the Zombie will follow the player even when out of sight (in seconds)


    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();

        //Implement method to get health from previous run ?
        Health = MaxHealth;

        HealthPercentage = Health / MaxHealth;
    }

    private void Update()
	{
        lookForPlayer();
        timeSinceLastSeenPlayer += Time.deltaTime;

        if (playerDetected)
        {
            MoveToPlayer();
        }

        delay -= Time.deltaTime;
	}

    void lookForPlayer()
    {
        RaycastHit hit;

        Vector3 trackerDir = (target.position - transform.position).normalized;
        Quaternion trackerLookRot = Quaternion.LookRotation(new Vector3(trackerDir.x, 0, trackerDir.z));
        PlayerTracker.GetComponent<Transform>().rotation = trackerLookRot;

        if (Physics.Raycast(transform.position, PlayerTracker.transform.forward, out hit, lookRange))
        {
            if(hit.transform.tag == "Player")
            {
                playerDetected = true;
                timeSinceLastSeenPlayer = 0f;
            }
        }

        if (timeSinceLastSeenPlayer >= persistensy && !Physics.Raycast(transform.position, PlayerTracker.transform.forward, out hit, lookRange))
        {
            playerDetected = false;
        }
    }

	private void OnValidate()
    {
        HealthPercentage = Health / MaxHealth;
    }

    void MoveToPlayer()
    {
        float distance = Vector3.Distance(target.position, transform.position);

            agent.SetDestination(target.position);

        //Attack the player
        if (distance <= AttackRange && delay <= 0)
        {
            target.GetComponent<P_Stats>().Damage(DMG);
            FaceTarget();
            delay = HitInterval;
            
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

    void Die()
    {
        //Die stuff
        Destroy(gameObject);
    }
}
