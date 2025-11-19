using System.Diagnostics;
using UnityEngine;

public class TargetStack : Target
{
    public float impactForce;

    Rigidbody targetRigidBody;

    void Start()
    {
        targetRigidBody = target.GetComponent<Rigidbody>();
    }

    public override void Process(RaycastHit hit)
    {
        targetRigidBody.AddForce(-hit.normal * impactForce);

        effectScript.Play(hit, hitSound, hitEffect, effectDuration);
    }
}
