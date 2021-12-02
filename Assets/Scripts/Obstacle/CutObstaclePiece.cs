using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CutObstaclePiece : MonoBehaviour
{
    [SerializeField] private Vector3 _centerOfMass;
    
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _rigidbody.centerOfMass = _centerOfMass;
    }

    private void Update()
    {
        _rigidbody.centerOfMass = _centerOfMass;
    }

    public void EngageGravity()
    {
        _rigidbody.isKinematic = false;
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_rigidbody.worldCenterOfMass, 0.1f);
    }*/
}
