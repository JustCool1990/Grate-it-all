using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class CutObstacle : MonoBehaviour
{
    private Collider _collider;

    public CuttingStartPoint Point { get; private set; }
    public event UnityAction Decompose;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        Point = GetComponentInChildren<CuttingStartPoint>();
    }

    public void StartDecompose()
    {
        Decompose?.Invoke();
    }

    public void DisableCollider()
    {
        _collider.enabled = false;
    }
}
