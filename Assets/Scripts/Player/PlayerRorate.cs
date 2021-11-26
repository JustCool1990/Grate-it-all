using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerJump), typeof(Player), typeof(PlayerCollision))]
public class PlayerRorate : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _maxDecreasedRotationSpeed;
    [SerializeField] private float _decelerationRotationSpeed;

    private float _rotateZ = 0;
    private float _decelerationRotationCoefficient = 0;
    private bool _canRotate = false;
    private PlayerJump _playerJump;
    private Player _player;
    private PlayerCollision _playerCollision;
    private IEnumerator _coroutine;
    private bool _coroutineIsActive = false;

    private bool _clockwiseRotation;

    private void Awake()
    {
        _playerJump = GetComponent<PlayerJump>();
        _player = GetComponent<Player>();
        _playerCollision = GetComponent<PlayerCollision>();
    }

    private void OnEnable()
    {
        _playerJump.Jumped += OnJumped;
        _player.Slicing += OnSlicing;
        _playerCollision.FacedWithPlatform += OnFacedWithPlatform;
    }

    private void OnDisable()
    {
        _playerJump.Jumped -= OnJumped;
        _player.Slicing -= OnSlicing;
        _playerCollision.FacedWithPlatform -= OnFacedWithPlatform;
    }

    private void FixedUpdate()
    {
        if(_canRotate == true)
        {
            _rotateZ -= (_rotationSpeed * Time.deltaTime - _decelerationRotationCoefficient) * (_clockwiseRotation == true ? -1 : 1);
            transform.rotation = Quaternion.Euler(0, 0, _rotateZ);
            DecreaseRotationSpeed();
        }
    }

    private void OnJumped(bool clockwiseRotation)
    {
        _canRotate = true;
        _decelerationRotationCoefficient = 0;


        _clockwiseRotation = clockwiseRotation;
        _rotateZ = transform.rotation.eulerAngles.z;


        /*if (_coroutineIsActive == true)
            StopActiveCoroutine();

        _rotateZ = transform.rotation.eulerAngles.z;
        _coroutine = ChangeRotation(clockwiseRotation);
        StartCoroutine(_coroutine);*/
    }

    private IEnumerator ChangeRotation(bool clockwiseRotation)
    {
        _coroutineIsActive = true;

        while (_canRotate == true)
        {
            _rotateZ -= (_rotationSpeed * Time.fixedDeltaTime - _decelerationRotationCoefficient) * (clockwiseRotation == true ? -1 : 1);
            transform.rotation = Quaternion.Euler(0, 0, _rotateZ);
            DecreaseRotationSpeed();

            yield return null;
        }
    }

    private void DecreaseRotationSpeed()
    {
        if (_decelerationRotationCoefficient < _maxDecreasedRotationSpeed)
            _decelerationRotationCoefficient += Time.deltaTime / _decelerationRotationSpeed;
    }

    private void StopRotation()
    {
        _canRotate = false;
        _rotateZ = 0;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnSlicing()
    {
        _canRotate = false;


        if (_coroutineIsActive == true)
            StopActiveCoroutine();

        StopRotation();
    }

    private void OnFacedWithPlatform(Collision collision)
    {
        _canRotate = false;


        if (_coroutineIsActive == true)
            StopActiveCoroutine();
    }

    private void StopActiveCoroutine()
    {
        StopCoroutine(_coroutine);
        _coroutineIsActive = false;
    }
}
