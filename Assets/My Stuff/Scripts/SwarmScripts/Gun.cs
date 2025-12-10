//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool; // Required for Object Pooling

public class ZombieGun : MonoBehaviour
{
    //TODO:
    //add sound, and slight recoil to add some oomph when firing
    public float range = 100f;
    
    //firerate modifier edited by upgrade info
    public float roundsPerMinute = 300f;
    private float fireDelay;
    public float spread = 0.05f;

    [Header("Visual Settings")]
    public ParticleSystem muzzleFlash;
    public BulletTracerBehavior tracerPrefab;
    public Transform muzzlePoint;

    private Camera fpsCamera;
    private float nextTimeToFire;

    public Vector3 aimPosition;
    public float adsSpeed = 8f;
    private Vector3 defaultPosition;
    
    // Pool to manage tracers efficiently
    private ObjectPool<BulletTracerBehavior> _tracerPool;

    void Start()
    {

        fireDelay = 60f / roundsPerMinute;
        defaultPosition = transform.localPosition;

        fpsCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        nextTimeToFire = 0.0f;

        // Initialize the pool
        _tracerPool = new ObjectPool<BulletTracerBehavior>(
            createFunc: () => Instantiate(tracerPrefab),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            defaultCapacity: 20,
            maxSize: 100
        );
    }

    void Update()
    {
        bool ready = Time.time >= nextTimeToFire;
        if (ready && Input.GetButton("Fire1"))
        {
            Shoot();
        }

        if (Input.GetMouseButton(1)) 
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * adsSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPosition, Time.deltaTime * adsSpeed);
        }
    }

    void Shoot()
    {
        // Update cooldown immediately
        nextTimeToFire = Time.time + fireDelay;

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        float adsSpread = spread;
        if (Input.GetMouseButton(1)) 
        {
            adsSpread *= 0.2f;
        }
        float x = Random.Range(-adsSpread, adsSpread);
        float y = Random.Range(-adsSpread, adsSpread);


        
        RaycastHit hit;
        Vector3 targetPoint; // Where the visual bullet needs to go

        Vector3 shootDirection = fpsCamera.transform.forward + (fpsCamera.transform.right * x) + (fpsCamera.transform.up * y);

        // Check if we hit anything
        bool hasHit = Physics.Raycast(fpsCamera.transform.position, shootDirection, out hit, range);

        if (hasHit)
        {
            targetPoint = hit.point; // Bullet goes to the hit point

            ZombieTarget target = hit.transform.GetComponent<ZombieTarget>();
            if (target != null)
            {
                target.Process(hit);
                if (target.dead && target.giveXP)
                {
                    //gain experience for killing zombie
                    PlayerScript player = GameObject.Find("PlayerCapsule").GetComponent<PlayerScript>();
                    if (player != null) {
                        player.gainExperience(target.EXPAmount);
                        target.giveXP = false;
                    }
                    else {
                        Debug.Log("PlayerScript component not found on Player.");
                    }
                }
            }
        }
        else
        {
            // If we miss, shoot into the distance
            targetPoint = fpsCamera.transform.position + (shootDirection * range);
        }


        SpawnTracer(targetPoint);
    }

    void SpawnTracer(Vector3 hitPoint)
    {

        Vector3 startPos = muzzlePoint != null ? muzzlePoint.position : transform.position;

        BulletTracerBehavior tracer = _tracerPool.Get();
        tracer.Init(startPos, hitPoint, (t) => _tracerPool.Release(t));
    }
}