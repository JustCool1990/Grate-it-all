using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollision : MonoBehaviour
{
    public event UnityAction<CutObstacle> FacedWithObstacle;
    public event UnityAction<bool> FacedWithPlatform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CutObstacle cutObstacle))
        {
            FacedWithObstacle?.Invoke(cutObstacle);
            cutObstacle.StartDecompose();
            cutObstacle.DisableCollider();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Platform platform))
            FacedWithPlatform?.Invoke(CheckCollisionPoint(collision));
    }

    private bool CheckCollisionPoint(Collision collision)
    {
        return transform.position.x < collision.gameObject.transform.position.x;
    }
}
