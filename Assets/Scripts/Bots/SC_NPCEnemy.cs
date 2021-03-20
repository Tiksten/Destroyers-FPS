using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class SC_NPCEnemy : MonoBehaviour, IEntity
{
    public float attackDistance = 3f;
    public float movementSpeed = 4f;
    public float npcHP = 100;
    //How much damage will npc deal to the player
    public int npcDamage = 5;
    public float attackRate = 0.5f;
    public Transform firePoint;
    public GameObject npcDeadPrefab;

    [HideInInspector]
    public Transform playerTransform;
    [HideInInspector]
    public SC_EnemySpawner es;
    NavMeshAgent agent;
    float nextAttackTime = 0;
    Rigidbody r;

    public AudioClip[] enemyHitSounds;
    public AudioClip[] enemyDeathSounds;
    public AudioClip[] enemyBreatheSounds;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackDistance;
        agent.speed = movementSpeed;
        r = GetComponent<Rigidbody>();
        r.useGravity = false;
        r.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance - attackDistance < 0.01f)
        {
            if (Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + attackRate;

                //Attack
                RaycastHit hit;
                if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, attackDistance))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        Debug.DrawLine(firePoint.position, firePoint.position + firePoint.forward * attackDistance, Color.cyan);

                        IEntity player = hit.transform.GetComponent<IEntity>();
                        PlayerHealth playerHealth = hit.transform.GetComponent<PlayerHealth>();
                        playerHealth.TakeDamage(npcDamage);
                    }
                }
            }
        }
        //Move towardst he player if see
        RaycastHit hit2;
        if (Physics.Raycast(transform.position, playerTransform.position - transform.position, out hit2, 100))
            if(hit2.collider.gameObject.tag == "Player")
                agent.destination = playerTransform.position;
        //Always look at player if see
        transform.LookAt(new Vector3(agent.destination.x, transform.position.y, agent.destination.z));
        //Gradually reduce rigidbody velocity if the force was applied by the bullet
        r.velocity *= 0.99f;

        if (Random.Range(0, 1000) <= 2)
        {
            audioSource.clip = enemyBreatheSounds[Random.Range(0, enemyBreatheSounds.Length)];
            audioSource.Play();
        }
    }

    public void ApplyDamage(float points)
    {
        agent.destination = playerTransform.position;
        npcHP -= points;
        if (npcHP <= 0)
        {
            //Destroy the NPC
            GameObject npcDead = Instantiate(npcDeadPrefab, transform.position, transform.rotation);
            //Slightly bounce the npc dead prefab up
            //npcDead.GetComponent<Rigidbody>().velocity = (-(playerTransform.position - transform.position).normalized * 8) + new Vector3(0, 5, 0);
            Destroy(npcDead, 10);
            es.EnemyEliminated(this);
            Destroy(gameObject);

            audioSource.clip = enemyDeathSounds[Random.Range(0, enemyDeathSounds.Length)];
            audioSource.Play();
        }
        else
        {
            audioSource.clip = enemyHitSounds[Random.Range(0, enemyHitSounds.Length)];
            audioSource.Play();
        }
    }
}