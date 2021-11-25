using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Animator))]
public class CutObstacle : MonoBehaviour
{
    private Collider _collider;
    private Animator _animator;

    public CuttingStartPoint Point { get; private set; }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
        Point = GetComponentInChildren<CuttingStartPoint>();
    }

    public void StartAnimation()
    {
        _animator.SetTrigger("Sliced");
    }

    public void DisableCollider()
    {
        _collider.enabled = false;
    }
}
