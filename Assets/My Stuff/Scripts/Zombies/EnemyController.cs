using UnityEngine;
using UnityEngine.AI;

//TODO:
// NEED TO MAKE THE TWO OTHER ZOMBIES WORK LIKE THE WEAK ONE, CHANGE THEIR ANIMATION SHEETS TO FOLLOW THE STYLE
// ADD A RANGED ZOMBIE (POTENTIALLY MIGHT BE TOO TOUGH)
// CREATE A DEATH FUNCTION TO DESTROY THE ENEMY ONCE KILLED
public class EnemyController : MonoBehaviour
{
    public float lookRadius = 8f;

    public float health = 10;
    public float damageAmount = 0;
    public float EXPAmount = 0;
    public float attackSpeed = 1;
    public AnimationClip attackClip;


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


        //we need to match the animation speed to the given attack speed
        float animSpeedMultiplier = attackClip.length / attackSpeed;
        animator.SetFloat("AttackSpeed", animSpeedMultiplier);


    }


    void Update()
    {





        float distance = Vector3.Distance(target.position, transform.position); //Distance between the zombie and the target
        timeSinceAttack -= Time.deltaTime;

        if (distance <= agent.stoppingDistance)
        {
            //Stopping distance is different for each enemy
            //for rangers, we will need to check the tag, and initiate their ranged attacks
            //Idea is to call an attack method, that then communicates to a function within the player script to take damage

            attackPlayer();
            FaceTarget();

        }
        else
        {
            //if we are not attacking, we should be moving towards the player with our walking anim

            agent.SetDestination(target.position); //set zombie destination to the player

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

        if (timeSinceAttack <= 0)
        {


            animator.SetTrigger("Attack");
            agent.SetDestination(transform.position);

            playerObj.takeDamage(damageAmount);
            Debug.Log("sent damage");

            timeSinceAttack = attackSpeed;

        }


    }
}
