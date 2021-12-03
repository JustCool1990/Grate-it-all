using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class CutObstacle : MonoBehaviour
{
    [SerializeField] private List<CutObstaclePiece> _cutObstaclePieces;

    private Collider _collider;
    private Player _player;

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

    public void Init(Player player)
    {
        _player = player;
        _player.Sliced += OnSliced;
    }

    private void OnSliced()
    {
        EngageGravity();
        _player.Sliced -= OnSliced;
    }

    private void EngageGravity()
    {
        foreach (var cutObstacle in _cutObstaclePieces)
            cutObstacle.EngageGravity();
    }
}
