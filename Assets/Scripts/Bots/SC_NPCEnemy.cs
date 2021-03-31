using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class SC_NPCEnemy : MonoBehaviour, IEntity
{
    public bool canMoveRandomly = false;
    public bool alwaysSeePlayer = false;

    public Transform viewPoint;
    public EnemyAnimationController enemyAnim;

    public float meleeAttackDistance = 5f;
    public float movementSpeed = 4f;
    public float npcHP = 100;
    public float maxHP = 100;
    //How much damage will npc deal to the player
    public int npcMeleeDamage = 5;
    public float attackRate = 0.5f;
    public Transform firePoint;
    public GameObject npcDeadPrefab;

    [HideInInspector]
    public Transform playerTransform;
    //[HideInInspector]
    //public SC_EnemySpawner es;
    NavMeshAgent agent;
    float nextAttackTime = 0;
    Rigidbody r;

    public AudioClip[] enemyHitSounds;
    public AudioClip[] enemyDeathSounds;
    public AudioClip[] enemyBreatheSounds;
    public AudioSource audioSource;

    [HideInInspector] public bool canSeePlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = meleeAttackDistance;
        agent.speed = movementSpeed;
        r = GetComponent<Rigidbody>();
        r.useGravity = false;
        r.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance - meleeAttackDistance < 0.01f)
        {
            if (Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + attackRate;

                //Attack
                RaycastHit hit;
                if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, meleeAttackDistance))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        Debug.DrawLine(firePoint.position, firePoint.position + firePoint.forward * meleeAttackDistance, Color.cyan);

                        IEntity player = hit.transform.GetComponent<IEntity>();
                        PlayerHealth playerHealth = hit.transform.GetComponent<PlayerHealth>();
                        playerHealth.TakeDamage(npcMeleeDamage);
                    }
                }
            }

            if(canMoveRandomly && agent.remainingDistance - meleeAttackDistance < 0.01f)
            {
                agent.destination = agent.destination + new Vector3(Random.Range(-15, 15), Random.Range(-5, 5), Random.Range(-15, 15));
                enemyAnim.currentTarget = agent.destination;
                canMoveRandomly = false;
                StartCoroutine(WaitForRandomMovementEnd());
            }
        }

        


        //Move towards the player if see
        RaycastHit hit2;
        if (Physics.Raycast(viewPoint.position, playerTransform.position - viewPoint.position, out hit2, 100))
        {
            if (hit2.collider.gameObject.tag == "Player")
            {
                canSeePlayer = true;
                agent.destination = playerTransform.position;
                enemyAnim.Shoot(agent.destination);
            }
            else
            {
                canSeePlayer = false;
                StartCoroutine(WaitForRandomMovementEnd());
            }
        }


        //Always look at player if see
        transform.LookAt(new Vector3(agent.destination.x, transform.position.y, agent.destination.z));


        //Gradually reduce rigidbody velocity if the force was applied by the bullet
        r.velocity *= 0.5f;

        if (Random.Range(0, 1000) <= 1)
        {
            audioSource.clip = enemyBreatheSounds[Random.Range(0, enemyBreatheSounds.Length)];
            audioSource.Play();
        }
    }

    public void ApplyDamage(float points)
    {
        agent.destination = playerTransform.position;
        npcHP -= points;
        enemyAnim.currentTarget = playerTransform.position;

        if (npcHP <= 0)
        {   
            audioSource.clip = enemyDeathSounds[Random.Range(0, enemyDeathSounds.Length)];
            audioSource.Play();
            //Destroy the NPC
            GameObject npcDead = Instantiate(npcDeadPrefab, transform.position, transform.rotation);
            //Slightly bounce the npc dead prefab up
            //npcDead.GetComponent<Rigidbody>().velocity = (-(playerTransform.position - transform.position).normalized * 8) + new Vector3(0, 5, 0);
            Destroy(npcDead, 10);
            //es.EnemyEliminated(this);
            Destroy(gameObject);
        }

        else
        {
            audioSource.clip = enemyHitSounds[Random.Range(0, enemyHitSounds.Length)];
            audioSource.Play();

            enemyAnim.DamageGiven(npcHP, maxHP);
        }
    }

    public IEnumerator WaitForRandomMovementEnd()
    {
        yield return new WaitForSeconds(Random.Range(5, 25));
        if(!canSeePlayer)
            canMoveRandomly = true;
    }
}