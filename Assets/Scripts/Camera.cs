using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Vector2 _offset;

    private void Awake()
    {
        Follow(_player);
    }

    private void Update()
    {
        Follow(_player);
    }

    private void Follow(Player player)
    {
        transform.position = new Vector3(_offset.x + player.transform.position.x, _offset.y + player.transform.position.y, transform.position.z);
    }
}