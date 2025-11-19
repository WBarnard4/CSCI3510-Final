using UnityEngine;

public class Diamond : Target
{
    float time;
    float startRotationAmount;
    float rotationAmount;
    public float hitIntensity = 250;

    Vector3 rotation;

    void Start()
    {
        startRotationAmount = 10f;
        rotationAmount = startRotationAmount;

        SetRotation();
    }

    void Update()
    {
        time = Time.deltaTime;
        target.transform.Rotate(rotation * time);

        if (rotationAmount > startRotationAmount)
        {
            IncRotation(-0.5f);
        }
    }

    void SetRotation()
    {
        rotation = new Vector3(rotationAmount, rotationAmount, rotationAmount);
    }

    void IncRotation(float delta)
    {
        rotationAmount += delta;
        SetRotation();
    }

    public override void Process(RaycastHit hit)
    {
        IncRotation(hitIntensity);
        effectScript.Play(hit, hitSound, hitEffect, effectDuration);
    }
}
