using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerCollision), typeof(Collider))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _slicedSpeed;
    [SerializeField] private Transform _bottomCutPoint;

    private PlayerCollision _playerCollision;
    private Collider _collider;

    public event UnityAction Slicing;
    public event UnityAction Sliced;

    private void Awake()
    {
        _playerCollision = GetComponent<PlayerCollision>();
        _collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        _playerCollision.FacedWithObstacle += OnFacedWithObstacle;
    }

    private void OnDisable()
    {
        _playerCollision.FacedWithObstacle -= OnFacedWithObstacle;
    }

    private void OnFacedWithObstacle(CutObstacle cutObstacle)
    {
        _collider.enabled = false;
        Slicing?.Invoke();

        transform.position = new Vector3(cutObstacle.transform.position.x, cutObstacle.Point.transform.position.y - (_bottomCutPoint.localPosition.y * transform.localScale.y), transform.position.z);

        StartCoroutine(SliceObstacle(cutObstacle));
    }

    private IEnumerator SliceObstacle(CutObstacle cutObstacle)
    {
        Vector3 target = new Vector3(transform.position.x, transform.position.y - cutObstacle.transform.localScale.y, transform.position.z);

        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _slicedSpeed);

            yield return null;
        }

        _collider.enabled = true;
        Sliced?.Invoke();
    }
}
