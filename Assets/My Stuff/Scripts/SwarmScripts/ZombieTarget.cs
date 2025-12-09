using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ZombieTarget : MonoBehaviour
{
    //damage modifier edited by upgrade info;
    public float damageModifer = 0;
    public GameObject target;
    public PlayerScript player;

    public GameObject effectsManager;
    public GameObject hitEffect;
    public float effectDuration = 0.1f;
    public AudioClip hitSound;
    public float EXPAmount = 10.0f;
    [SerializeField]
    private TMP_Text textDisplay;
    [HideInInspector]
    public bool giveXP = true;

    private EnemyController enemyController;

    protected Effect effectScript;
    float timer = 0.0f;

    public bool dead = false;

    private bool isRising = true;
    public float riseSpeed = 2.0f;
    private float groundYLevel = 0.0f;
    private NavMeshAgent navAgent;

    public GameObject dirtParticlePrefab; 
    private GameObject activeDirtParticles;

    public AudioClip[] footstepSounds;
    public float stepRate = 0.5f;
    private float stepTimer;
    private AudioSource audioSource;

    public float minStartDelay = 0.5f;
    public float maxStartDelay = 2.0f;
    private float startWaitTimer = 0.0f;
    private bool isWaitingToWalk = false;

    private void Awake()
    {
        //effectScript = effectsManager.GetComponent<Effect>();
        enemyController = GetComponent<EnemyController>(); 

        navAgent = GetComponent<NavMeshAgent>();

        if (dirtParticlePrefab != null)
        {
            activeDirtParticles = Instantiate(dirtParticlePrefab, transform.position, Quaternion.identity);
            activeDirtParticles.transform.SetParent(transform);

        }
        

        if (enemyController != null) enemyController.enabled = false;
        if (navAgent != null) navAgent.enabled = false;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.spatialBlend = 1.0f;
        audioSource.maxDistance = 20.0f;
    }

    private void Start()
    {
        textDisplay.text = "";
        EXPAmount = enemyController.EXPAmount;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        textDisplay.color = Color.red;
    }

    private void Update()
    {
        if (isRising)
        {
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;

            if (transform.position.y >= groundYLevel)
            {
                transform.position = new Vector3(transform.position.x, groundYLevel, transform.position.z);
                isRising = false;

                if (activeDirtParticles != null)
                {
                    Destroy(activeDirtParticles); 
                }

                startWaitTimer = UnityEngine.Random.Range(minStartDelay, maxStartDelay);
                Debug.Log("Start Wait Timer set to: " + startWaitTimer);
                isWaitingToWalk = true;
            }
            return;
        }
        if (isWaitingToWalk)
        {
            startWaitTimer -= Time.deltaTime;
            if (startWaitTimer <= 0)
            {
                isWaitingToWalk = false;
                if (navAgent != null) navAgent.enabled = true;
                if (enemyController != null) enemyController.enabled = true;
            }
            return;
        }
        if (navAgent != null && navAgent.enabled && navAgent.velocity.sqrMagnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0)
            {
                PlayFootstep();
                stepTimer = stepRate;
            }
        }
        if (textDisplay.text != "")
        {
            if (timer < 0.5f)
            {
                timer += Time.deltaTime;
            }
            else
            {
                textDisplay.text = "";
                timer = 0.0f;
            }
        }
        if (enemyController.health <= 0)
        {
            if(transform.position.y > -3.0f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime, transform.position.z);
            } else
            {
                Destroy(gameObject);
            }
        }
        
    }

void PlayFootstep()
    {
        // DEBUG LOGGING START
        if (footstepSounds == null) {
            Debug.LogError(gameObject.name + ": footstepSounds array is NULL!");
        } else if (footstepSounds.Length == 0) {
            Debug.LogError(gameObject.name + ": footstepSounds array is EMPTY (Size 0)!");
        }
        
        if (audioSource == null) {
            Debug.LogError(gameObject.name + ": AudioSource is NULL! Awake() might not have run.");
        }
        // DEBUG LOGGING END

        if (footstepSounds.Length > 0 && audioSource != null)
        {
            int randIndex = UnityEngine.Random.Range(0, footstepSounds.Length);
            audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(footstepSounds[randIndex]);
        }
    }

    public virtual void Process(RaycastHit hit)
    {
        //effectScript.Play(hit, hitSound, hitEffect, effectDuration);
        Debug.Log("Hit registered on ZombieTarget.");
        if (enemyController != null)
        {   
            float damage;
            float dropoff = 0.5f; //lower means more damage up close, less at range
            damage = (float)Math.Floor((500.0f + player.damageModifer) * dropoff/(hit.distance + (50.0f * dropoff - 5.0f)));
            //damage = 500; //temp for testing
            enemyController.health -= damage;
            textDisplay.text = ((int)damage).ToString();
            textDisplay.transform.LookAt(Camera.main.transform);
            //flip the text to face the camera correctly
            textDisplay.transform.Rotate(0, 180, 0);
            if (enemyController.health <= 0 && !dead)
            {
                Debug.Log("Zombie has died.");
                dead = true;
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                Animator animator = GetComponent<Animator>();
                animator.enabled = false;
                if (agent != null)
                {
                    //remove enemycontroller to stop errors from trying to access a removed component
                    Destroy(enemyController);
                    Destroy(agent);
                } else
                {
                    Debug.Log("No NavMeshAgent found on " + gameObject.name);
                }
            }
        }
        //Destroy(target);
    }
}
