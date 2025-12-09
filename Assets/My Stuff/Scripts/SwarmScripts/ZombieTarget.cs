using System;
using TMPro;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.AI;

public class ZombieTarget : MonoBehaviour
{
    public GameObject target;

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

    private void Awake()
    {
        //effectScript = effectsManager.GetComponent<Effect>();
        enemyController = GetComponent<EnemyController>(); 
    }

    private void Start()
    {
        textDisplay.text = "";
        EXPAmount = enemyController.EXPAmount;
    }

    private void Update()
    {
        
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

    public virtual void Process(RaycastHit hit)
    {
        //effectScript.Play(hit, hitSound, hitEffect, effectDuration);
        Debug.Log("Hit registered on ZombieTarget.");
        if (enemyController != null)
        {   
            float damage;
            float dropoff = 0.5f; //lower means more damage up close, less at range
            damage = (float)Math.Floor(500.0f * dropoff/(hit.distance + (50.0f * dropoff - 5.0f)));
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
