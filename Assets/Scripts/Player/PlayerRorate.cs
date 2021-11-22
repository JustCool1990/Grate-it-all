using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerJump), typeof(Player))]
public class PlayerRorate : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _maxDecreasedRotationSpeed;
    [SerializeField] private float _decelerationRotationSpeed;

    private float _rotateZ = 0;
    private float _fullTurnover = 360;
    private float _decelerationRotationCoefficient = 0;
    private PlayerJump _playerJump;
    private Player _player;
    private IEnumerator _coroutine;
    private bool _coroutineIsActive = false;

    private void Awake()
    {
        _playerJump = GetComponent<PlayerJump>();
        _player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        _playerJump.Jumped += OnJumped;
        _player.Slicing += OnSlicing;
    }

    private void OnDisable()
    {
        _playerJump.Jumped -= OnJumped;
        _player.Slicing -= OnSlicing;
    }

    private void OnJumped(bool clockwiseRotation)
    {
        _decelerationRotationCoefficient = 0;

        if (_coroutineIsActive == true)
            StopActiveCoroutine();

        _rotateZ = transform.rotation.eulerAngles.z;
        _coroutine = ChangeRotation(clockwiseRotation);
        StartCoroutine(_coroutine);
    }

    private IEnumerator ChangeRotation(bool clockwiseRotation)
    {
        _coroutineIsActive = true;

        while (Mathf.Abs(_rotateZ) < _fullTurnover)
        {
            _rotateZ -= (_rotationSpeed * Time.deltaTime - _decelerationRotationCoefficient) * (clockwiseRotation == true ? -1 : 1);
            transform.rotation = Quaternion.Euler(0, 0, _rotateZ);

            DecreaseRotationSpeed();

            yield return null;
        }

        StopRotation();
        _coroutineIsActive = false;
    }

    private void DecreaseRotationSpeed()
    {
        if (_decelerationRotationCoefficient < _maxDecreasedRotationSpeed)
            _decelerationRotationCoefficient += Time.deltaTime / _decelerationRotationSpeed;
    }

    private void StopRotation()
    {
        _rotateZ = 0;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnSlicing()
    {
        if (_coroutineIsActive == true)
            StopActiveCoroutine();

        StopRotation();
    }

    private void StopActiveCoroutine()
    {
        StopCoroutine(_coroutine);
        _coroutineIsActive = false;
    }
}
