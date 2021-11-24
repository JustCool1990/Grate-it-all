using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerJump _playerJump;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _zoomOffset;
    [SerializeField] private float _unzoomedAngleX;
    [SerializeField] private float _zoomedAngleX;
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _accelerationZoomCoefficient;

    private Vector3 _lastOffset;
    private Quaternion _startQuaternion;
    private Quaternion _zoomQuaternion;
    private bool _returnStartQuaternion = false;
    private IEnumerator _coroutine;
    private bool _coroutineIsActive = false;

    private void Awake()
    {
        _startQuaternion = Quaternion.Euler(new Vector3(_unzoomedAngleX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
        _zoomQuaternion = Quaternion.Euler(new Vector3(_zoomedAngleX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));

        _lastOffset = _offset;
    }

    private void Start()
    {
        transform.position = SetStartPosition(_player, _lastOffset);
    }

    private void OnEnable()
    {
        _player.Slicing += OnSlicing;
        _playerJump.Jumped += OnJumped;
    }

    private void OnDisable()
    {
        _player.Slicing -= OnSlicing;
        _playerJump.Jumped -= OnJumped;
    }

    private void Update()
    {
        Follow(_player, _lastOffset);
    }

    private Vector3 SetStartPosition(Player player, Vector3 offset)
    {
        return new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, player.transform.position.z + offset.z);
    }

    private void Follow(Player player, Vector3 offset)
    {
        Vector3 target = SetStartPosition(player, offset);
        Vector3 currentPosition = Vector3.Lerp(transform.position, target, _followSpeed * Time.deltaTime);
        transform.position = currentPosition;
    }

    private void OnSlicing()
    {
        SetFollowTarget(_zoomOffset, _zoomQuaternion);
        _returnStartQuaternion = true;
    }

    private void OnJumped(bool clockwiseRotation)
    {
        if(_returnStartQuaternion == true)
        {
            _returnStartQuaternion = false;
            SetFollowTarget(_offset, _startQuaternion);
        }
    }

    private void SetFollowTarget(Vector3 offset, Quaternion quaternion)
    {
        if (_coroutineIsActive == true)
            StopActiveCoroutine();

        _lastOffset = offset;

        _coroutine = ChangeRotation(quaternion);
        StartCoroutine(_coroutine);
    }

    private IEnumerator ChangeRotation(Quaternion rotation)
    {
        _coroutineIsActive = true;
        float delayGradualIncrease = 0;

        while (transform.rotation != rotation)
        {
            if (delayGradualIncrease < _zoomSpeed)
                delayGradualIncrease += Time.deltaTime * _accelerationZoomCoefficient;

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, delayGradualIncrease * Time.deltaTime);
            yield return null;
        }

        _coroutineIsActive = false;
    }

    private void StopActiveCoroutine()
    {
        StopCoroutine(_coroutine);
        _coroutineIsActive = false;
    }
}