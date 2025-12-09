using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZombieGun : MonoBehaviour
{
    //TODO:
    //add sound, and slight recoil to add some oomph when firing
    public float range = 100f;

    public ParticleSystem muzzleFlash;
    private Camera fpsCamera;
    private float nextTimeToFire;

    void Start()
    {
        fpsCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        nextTimeToFire = 0.0f;
    }

    void Update()
    {
        bool ready = Time.time >= nextTimeToFire;
        if ( ready && Input.GetButton("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
            Debug.Log("Muzzle flash played.");
        }
        Debug.Log("Shooting...");
        RaycastHit hit;

        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            ZombieTarget target = hit.transform.GetComponent<ZombieTarget>();
            if (target != null)
            {
                target.Process(hit);
                if(target.dead && target.giveXP)
                {
                    //gain experience for killing zombie
                    PlayerScript player = GameObject.Find("PlayerCapsule").GetComponent<PlayerScript>();
                    if (player != null){
                        player.gainExperience(target.EXPAmount);
                        target.giveXP = false;
                    }
                    else
                    {
                        Debug.Log("PlayerScript component not found on Player.");
                    }
                }
            }
            nextTimeToFire = Time.time + 0.1f;
        }
    }
}
