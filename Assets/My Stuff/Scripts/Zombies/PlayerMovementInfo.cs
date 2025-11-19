using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovementInfo 
{
    public float forwardAndBack = 0.0f;
    public float leftAndRight = 0.0f;

    public float speed = 0.0f;
    public Vector3 direction = Vector3.zero;
    public Vector3 normalizedDirection = Vector3.zero;
    public Vector3 distance = Vector3.zero;

    public bool movingForwards = false;
    public bool movingBackwards = false;

    public float baseSpeed;
    public float runningAmplifier;
}
