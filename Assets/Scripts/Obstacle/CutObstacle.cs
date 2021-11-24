using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutObstacle : MonoBehaviour
{
    private Animator _animator;

    public CuttingStartPoint Point { get; private set; }

    private void Awake()
    {
        Point = GetComponentInChildren<CuttingStartPoint>();
        _animator = GetComponentInChildren<Animator>();
    }

    public void StartAnimation()
    {
        _animator.SetTrigger("Sliced");
    }
}
