using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CutObstacle))]
public class CutObstacleAnimations : MonoBehaviour
{
    private Animator _animator;
    private CutObstacle _cutObstacle;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _cutObstacle = GetComponent<CutObstacle>();
    }

    private void OnEnable()
    {
        _cutObstacle.Decompose += OnDecompose;
    }

    private void OnDisable()
    {
        _cutObstacle.Decompose -= OnDecompose;
    }

    private void OnDecompose()
    {
        _animator.SetTrigger(AnimatorCutObstacle.States.Decompose);
    }
}
