using UnityEngine;
using System.Collections;

public class BulletTracerBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 200f; // Speed of the visual bullet
    private TrailRenderer _trail;
    private Vector3 _targetPosition;
    private System.Action<BulletTracerBehavior> _killAction;

    private void Awake()
    {
        _trail = GetComponent<TrailRenderer>();
    }

    public void Init(Vector3 startPos, Vector3 targetPos, System.Action<BulletTracerBehavior> killAction)
    {
        transform.position = startPos;
        _targetPosition = targetPos;
        _killAction = killAction;

        // Clear the old trail so it doesn't "streak" from the previous shot position
        if (_trail != null) _trail.Clear();

        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        Vector3 startPos = transform.position;
        float distance = Vector3.Distance(startPos, _targetPosition);
        float traveled = 0f;

        while (traveled < distance)
        {
            // Move the visual representation towards the target
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
            traveled += step;
            yield return null;
        }

        // Return to pool when destination is reached
        _killAction?.Invoke(this);
    }
}