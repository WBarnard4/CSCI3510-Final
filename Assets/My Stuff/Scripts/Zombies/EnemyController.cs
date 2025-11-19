using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 8f;

    Transform target;
    NavMeshAgent agent;
    AudioSource groan;
    Animator animator;


    void Start()
    {
        target = PlayerManager.Instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        groan = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position); //Distance between the zombie and the target

        if (distance <= lookRadius) //if the player enters the look radius
        {
            agent.SetDestination(target.position); //set zombie destination to the player
            PlaySound();

            animator.SetFloat("Forward", 1.0f); //set our animation state to 1
        }
        else
        {
            agent.SetDestination(transform.position); //set zombie destination to its current position
            groan.Stop();
            animator.SetFloat("Forward", 0.0f); //set animation state to 0
        }

        if (distance <= agent.stoppingDistance)
        {
            animator.SetFloat("Forward", 0.0f); //make the zombie stop when running into a player 
            //We can change this to be something like an attack animation instead
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
}
