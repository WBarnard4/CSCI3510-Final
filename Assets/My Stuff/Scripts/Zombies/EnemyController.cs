using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 8f;

    public float health = 10;
    public float damageAmount = 0;
    public float EXPAmount = 0;
    public float attackSpeed = 1;    


    private Player playerObj;
    private float timeSinceAttack = 0f;

    Transform target;
    NavMeshAgent agent;
    AudioSource groan;
    Animator animator;

    void Awake()
    {
        //utilize awake here for safer initilization of these components as enemies are spawned
        agent = GetComponent<NavMeshAgent>();
        groan = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        

    }


    void Start()
    {
        target = PlayerManager.Instance.player.transform;
        playerObj = target.GetComponent<Player>();


    }


    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position); //Distance between the zombie and the target

        // Code is being commented in case a search radius is a better way of implementing
        // if (distance <= lookRadius) //if the player enters the look radius
        // {
        //     agent.SetDestination(target.position); //set zombie destination to the player
        //     PlaySound();

        //     animator.SetFloat("Forward", 1.0f); //set our animation state to 1
        // }
        // else
        // {
        //     agent.SetDestination(transform.position); //set zombie destination to its current position
        //     groan.Stop();
        //     animator.SetFloat("Forward", 0.0f); //set animation state to 0
        // }

        //zombies should always be moving towards the player
        agent.SetDestination(target.position); //set zombie destination to the player
        // PlaySound(); //we do not need this to play every 5 seconds right now lmao
        animator.SetFloat("Forward", 1.0f); //set our animation state to 1

        if (distance <= agent.stoppingDistance)
        {
            //Stopping distance is different for each enemy
            //for rangers, we will need to check the tag, and initiate their ranged attacks
            //Idea is to call an attack method, that then communicates to a function within the player script to take damage
            timeSinceAttack -= Time.deltaTime;
            if (timeSinceAttack <= 0)
            {

                animator.SetFloat("Forward", 0.0f); //make the zombie stop when running into a player 
                attackPlayer();                     //call our attack function here

                timeSinceAttack += attackSpeed;
                Debug.Log("Zombie Attacked");
            }
            FaceTarget();

        }
    }

    void PlaySound()
    {
        if (!groan.isPlaying)
        {
            groan.Play(); //only play sounds if we arent already playing
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized; //obtain the direction the zombie must move towards the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); //obtain where the zombie needs to look
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); //smoothly rotate that direction
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void attackPlayer()
    {
        playerObj.takeDamage(damageAmount);
    }
}
