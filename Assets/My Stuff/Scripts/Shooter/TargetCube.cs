using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class TargetCube : Target
{
    bool rotationOn = false;
    float rotationAmount = 360;
    float currentRotationAmount = 0;

    Vector3 rotation;
    float time;

    void Update()
    {
        if (rotationOn)
        {
            if (currentRotationAmount < rotationAmount)
            {
                time = Time.deltaTime;
                target.transform.Rotate(rotation * time);
                currentRotationAmount += rotationAmount * time;
            }
            else
            {
                quaternion origRotation = transform.rotation;
                rotationOn = false;
                // target.transform.rotation = Quaternion.Euler(0, 0, 0);

                //updated code, results in the cube rotating to its original rotation position
                target.transform.rotation = origRotation;

            }
        }
    }

    void Rotate(float degrees)
    {
        if (!rotationOn)
        {
            rotationOn = true;
            rotationAmount = degrees;
            currentRotationAmount = 0f;
            rotation = new Vector3(0, degrees, 0);
        }
    }

    public override void Process(RaycastHit hit)
    {
        if (gameObject.tag == "Target")
        {
            Rotate(rotationAmount);
            effectScript.Play(hit, hitSound, hitEffect, effectDuration);

        }
        else if (gameObject.tag == "RangeTarget")
        {
            StartCoroutine(DisableRangeTarget(hit));

        }

    }

    IEnumerator DisableRangeTarget(RaycastHit hit)
    {
        Rotate(rotationAmount);
        effectScript.Play(hit, hitSound, hitEffect, effectDuration);
        yield return new WaitForSeconds(1f);
        transform.parent.gameObject.SetActive(false);
    }


}
