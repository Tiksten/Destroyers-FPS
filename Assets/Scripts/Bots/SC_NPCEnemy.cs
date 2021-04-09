using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class SC_NPCEnemy : MonoBehaviour
{
    [System.Serializable]
    public class CurrentTarget
    {
        public Helper.TargetType type;
        public Vector3 position;
    }

    [Header("STATS")]

    //HP
    public float maxHP = 100;
    public float _npcHP;
    public float npcHP
    {
        get => _npcHP;
        set => _npcHP = (value <= maxHP) ? value : maxHP;
    }
    [Space(10)]

    //Movement
    private float currentSpeed;
    public float walkSpeed = 3;
    public float runspeed = 6.5f;
    public float stoppingDistance = 5;
    [Space(10)]

    //Shooting
    public float fireRate = 0.1f;
    public float damage = 5;
    public int maxAmmoInMag = 15;
    public int ammoInMag = 15;
    public float reloadTime = 3;
    public float fireDistance = 1000;
    [Space(10)]

    //Melee
    public float meleeRate = 1;
    public float meleeDamage = 20;
    public float meleeDistance = 1;

    //Bool
    public bool canMoveRandomly = false;
    public bool alwaysSeePlayer = false;
    public bool startFriendly = false;
    [Space(20)]


    [Header("REFERENCES")]
    public Transform viewPoint;
    public Transform firePoint;
    public GameObject npcDeadPrefab;
    [Space(10)]
    public Transform[] patrolingPoints;

    public EnemyWeapon weapon;
    [Space(20)]


    [Header("OTHER")]
    //Sounds
    public LayerMask botViewMask;
    public AudioClip[] enemyHitSounds;
    public AudioClip[] enemyDeathSounds;
    public AudioClip[] enemyIdleSounds;
    public AudioSource audioSource;
    [Space(20)]

    [Header("ANIMATION")]
    public Animator legsAnimator;
    public Animator chestAnimator;
    public GameObject mainLookArmature;

    //Legs
    public AnimationClip legsIdle;

    public AnimationClip legsWalk;

    //Chest
    public AnimationClip chestShoot;

    public AnimationClip[] chestIdle;

    public AnimationClip chestHit;

    public AnimationClip[] chestLookingForPlayer;

    public AnimationClip chestWalk;


    public Transform enemyTotalRotation;




    //Flags
    [HideInInspector] public bool canSeePlayer = false; //Literally can see player
    [HideInInspector] public bool tryingToAttack = false; //Flag
    [HideInInspector] public bool canAct = true; //Chest can act (all cooldowns are over)
    [HideInInspector] public bool thereIsPlayerNearby = false; //Bot's flag to look for player

    //Other hidden
    public Transform playerTransform;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Rigidbody r;
    [HideInInspector] public CurrentTarget currentTarget;
    [HideInInspector] public Vector3 previousPos;
    [HideInInspector] public Quaternion defaultChestRotation;


    void Start()
    {
        previousPos = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
        agent.speed = walkSpeed;
        r = GetComponent<Rigidbody>();
        r.useGravity = false;
        r.isKinematic = true;
        StartCoroutine(DoIdleThings());
        defaultChestRotation = mainLookArmature.transform.localRotation;
    }

    void FixedUpdate()
    {
        tryingToAttack = false;

        //Setting current target
        if (CheckPlayerVisibility())
        {
            SetTarget(Helper.TargetType.Player, playerTransform.position);
            StopCoroutine(LookForPlayer());
            StopCoroutine(DoIdleThings());
            thereIsPlayerNearby = true;
            tryingToAttack = true;
        }
        else if (thereIsPlayerNearby)
        {
            SetTarget(Helper.TargetType.LookingForPlayer, playerTransform.position);
        }

        //Follow instruction to every target type
        switch(currentTarget.type)
        {
            case Helper.TargetType.Player:
                {
                    if (tryingToAttack && canAct)
                        TryToAttack();

                    LookAtTarget();
                    //Look at player if see
                    var look = enemyTotalRotation;
                    look.LookAt(currentTarget.position);
                    Debug.Log(enemyTotalRotation.position + "   " + look.rotation);
                    enemyTotalRotation.rotation = Quaternion.Euler(enemyTotalRotation.rotation.x, look.rotation.eulerAngles.y, enemyTotalRotation.rotation.z);//Need Fix*****************************************************************************

                    break;
                }
            case Helper.TargetType.Shot://Ignore
                {
                    break;
                }
            case Helper.TargetType.DeadBody://Ignore
                {
                    break;
                }
            case Helper.TargetType.Sound://Ignore
                {
                    break;
                }
            case Helper.TargetType.LookingForPlayer:
                {
                    StartCoroutine(LookForPlayer());
                    break;
                }
            case Helper.TargetType.Patrolling:
                {
                    break;
                }
            case Helper.TargetType.Staying:
                {
                    break;
                }
            case Helper.TargetType.Nothing:
                {
                    break;
                }
        }



        //Legs anim
        if (previousPos == transform.position)
        {
            legsAnimator.SetBool("IsStaying", true);
            chestAnimator.SetBool("IsStaying", true);
        }
        else
        {
            legsAnimator.SetBool("IsStaying", false);
            chestAnimator.SetBool("IsStaying", false);
        }

        previousPos = transform.position;



        agent.SetDestination(currentTarget.position);
    }

    public void ApplyDamage(float damage)
    {
        currentTarget.type = Helper.TargetType.Player;
        currentTarget.position = playerTransform.position;
        npcHP -= damage;

        if (npcHP <= 0)
        {
            //Death
            Destroy(Instantiate(npcDeadPrefab, transform.position, transform.rotation), 30);
            Destroy(gameObject);
        }

        else
        {
            chestAnimator.Play(chestHit.name);

            if (npcHP / maxHP <= 0.5f)
                chestAnimator.SetBool("Wounded", true);

            //Got hit & not dead
            audioSource.clip = enemyHitSounds[Random.Range(0, enemyHitSounds.Length)];
            audioSource.Play();
        }
    }

    public bool CheckPlayerVisibility()
    {
        if(alwaysSeePlayer)
            return true;

        RaycastHit hit;
        if (Physics.Raycast(viewPoint.position, playerTransform.position - viewPoint.position, out hit, (int)fireDistance, botViewMask))
        {
            if(hit.collider.gameObject.CompareTag("Player"))
                return true;
        }
        return false;
    }

    public IEnumerator LookForPlayer()
    {
        var i = Random.Range(4, 10);
        while (i >= 0)
        {
            //Go search random point
            currentTarget.type = Helper.TargetType.LookingForPlayer;
            currentTarget.position = transform.position + Random.insideUnitSphere * 5;

            yield return new WaitForSeconds(Random.Range(3, 8));

            //Stay
            currentTarget.position = transform.position;
            yield return new WaitForSeconds(Random.Range(2, 5));
            i--;
        }
        thereIsPlayerNearby = false;
    }

    public void SetTarget(Helper.TargetType type, Vector3 pos)
    {
        if ((int)currentTarget.type <= (int)type)
        {
            currentTarget.type = type;
            currentTarget.position = pos;
        }
    }

    public void TryToAttack()
    {
        if (agent.remainingDistance < meleeDistance)
            StartCoroutine(Melee());

        else if (ammoInMag > 0)
            StartCoroutine(Shot());

        else
        {
            Hide();
            StartCoroutine(Reload());
        }
    }

    public IEnumerator Melee()
    {
        canAct = false;

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, meleeDistance))
        {
            if (hit.transform.CompareTag("Player"))
            {
                Helper.GiveDamage(hit.transform.gameObject, meleeDamage);
            }
        }

        yield return new WaitForSeconds(meleeRate);
        canAct = true;
    }

    public IEnumerator Shot()
    {
        canAct = false;

        ammoInMag--;

        chestAnimator.Play(chestShoot.name);
        chestAnimator.SetBool("HaveSeenPlayer", true);

        weapon.Shot(damage);

        yield return new WaitForSeconds(fireRate);
        canAct = true;
    }

    public void Hide()
    {
        //Find nearest hiding spot, check if player see it, go there if not
    }

    public IEnumerator Reload()
    {
        canAct = false;

        //Play anim, wait time, give max ammo

        yield return new WaitForSeconds(reloadTime);
        ammoInMag = maxAmmoInMag;
        canAct = true;
    }

    public IEnumerator DoIdleThings()
    {
        if (patrolingPoints.Length > 0)
        {
            //Patrolling
            currentTarget.type = Helper.TargetType.Patrolling;
            while (true)
            {
                currentTarget.position = patrolingPoints[Random.Range(0, patrolingPoints.Length)].position;
                yield return new WaitForSeconds(Random.Range(25, 45));
            }
        }
        else if (canMoveRandomly)
        {
            currentTarget.type = Helper.TargetType.Nothing;
            while (true)
            {
                currentTarget.position = transform.position + Random.insideUnitSphere * 5;
                yield return new WaitForSeconds(Random.Range(10, 35));
            }
        }
        else
        {
            LookForPlayer();
        }
    }

    public void LookAtTarget()
    {
        var look = mainLookArmature.transform;
        look.LookAt(currentTarget.position);
        var lookInDegrees = look.rotation.eulerAngles;

        if (lookInDegrees.x < 320 && lookInDegrees.x > 180)
            lookInDegrees.x = 320;
        else if (lookInDegrees.x > 40 && lookInDegrees.x < 180)
            lookInDegrees.x = 40;

        mainLookArmature.transform.rotation = Quaternion.Euler(-lookInDegrees.x, look.rotation.y, look.rotation.z);

    }

    public void LookDefault()
    {
        mainLookArmature.transform.localRotation = defaultChestRotation;
    }
}