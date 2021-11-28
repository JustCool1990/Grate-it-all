using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerJump), typeof(Player), typeof(PlayerCollision))]
public class PlayerRorate : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _maxDecreasedRotationSpeed;
    [SerializeField] private float _rotationDecelerationSpeed;
    [SerializeField] private float _minDecelerationAngle;
    [SerializeField] private float _maxDecelerationAngle;

    private PlayerJump _playerJump;
    private Player _player;
    private PlayerCollision _playerCollision;
    private float _decelerationAngle = 0;
    private float _halfCircle = 180;
    private float _rotateZ;
    private float _decelerationRotationCoefficient = 0;
    private bool _canRotate;
    private bool _clockwiseRotation;
    private bool _moreHalfCircleRotation = false;
    private bool _slowRotation => _moreHalfCircleRotation == false? Mathf.Abs(_rotateZ) > _decelerationAngle : _rotateZ < _decelerationAngle;

    private void Awake()
    {
        _playerJump = GetComponent<PlayerJump>();
        _player = GetComponent<Player>();
        _playerCollision = GetComponent<PlayerCollision>();

        StopRotation();
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
        if (_canRotate == true)
            Rotate();
    }

    private void OnJumped(bool jumpBack)
    {
        _canRotate = true;
        _decelerationRotationCoefficient = 0;
        _clockwiseRotation = jumpBack;
        _rotateZ = transform.rotation.eulerAngles.z;
        _moreHalfCircleRotation = ChoosingDecelerationAngle(_rotateZ, _clockwiseRotation);
        _decelerationAngle = _moreHalfCircleRotation == true ? _minDecelerationAngle : _maxDecelerationAngle;

    }

    private bool ChoosingDecelerationAngle(float rotation, bool clockwiseRotation)
    {
        return clockwiseRotation == false && rotation >= _halfCircle ? true : false;
    }

    private void Rotate()
    {
        _rotateZ -= (_rotationSpeed * Time.deltaTime - _decelerationRotationCoefficient) * (_clockwiseRotation == true ? -1 : 1);
        transform.rotation = Quaternion.Euler(0, 0, _rotateZ);

        if (_slowRotation == true)
            DecreaseRotationSpeed();
    }

    private void DecreaseRotationSpeed()
    {
        if (_decelerationRotationCoefficient < _maxDecreasedRotationSpeed)
            _decelerationRotationCoefficient += Time.deltaTime * _rotationDecelerationSpeed;
    }

    private void StopRotation()
    {
        _canRotate = false;
        _rotateZ = 0;
    }

    private void OnSlicing()
    {
        StopRotation();
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnFacedWithPlatform(bool collisionPoint)
    {
        StopRotation();
    }
}
