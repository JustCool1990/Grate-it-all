using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollision : MonoBehaviour
{
    public event UnityAction<CutObstacle> FacedWithObstacle;
    public event UnityAction<Collision> FacedWithPlatform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CutObstacle cutObstacle))
        {
            FacedWithObstacle?.Invoke(cutObstacle);
            cutObstacle.StartAnimation();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Platform platform))
            FacedWithPlatform?.Invoke(collision);
    }
}
