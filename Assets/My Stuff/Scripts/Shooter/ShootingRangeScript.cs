using System.Collections.Generic;
using UnityEngine;


public class ShootingRangeScript : MonoBehaviour
{

    public List<GameObject> targets;
    public int TargetsToActivate;

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision!");
        if(collision.tag == "Player")
        {
            Debug.Log("Player Collision!");
            activateTargets();
        }
    }
    
    private void activateTargets()
    {
        foreach (GameObject GO in targets)
        {
            GO.SetActive(false);
        }

        int random1 = Random.Range(0, 12);

        int random2 = Random.Range(0, 12);
        while (random2 == random1)
        {
            random2 = Random.Range(0, 12);
        }

        int random3 = Random.Range(0, 12);
        while (random3 == random2 || random3 == random1)
        {
            random3 = Random.Range(0, 12);
        }

        targets[random1].SetActive(true);
        targets[random2].SetActive(true);
        targets[random3].SetActive(true);
    }
}
