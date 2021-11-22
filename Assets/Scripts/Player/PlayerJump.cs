using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(KeyboardInput), typeof(Rigidbody), typeof(PlayerCollision))]
[RequireComponent(typeof(Player))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField] private Vector2 _jumpDirection;
    [SerializeField] private float _reboundRatio;
    [SerializeField] private float _minReverseJumpAngle, _maxReverseJumpAngle;
    [SerializeField] private float _decreaseJumpPower;

    private Vector3 _normal;
    private bool _inJump = false;
    private bool _canJump = true;
    private float _rotateZ = 0;
    private bool _clockwiseRotation = false;
    private KeyboardInput _keyboardInput;
    private Rigidbody _rigidbody;
    private PlayerCollision _playerCollision;
    private Player _player;

    public event UnityAction<bool> Jumped;

    private void Awake()
    {
        _keyboardInput = GetComponent<KeyboardInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _playerCollision = GetComponent<PlayerCollision>();
        _player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        _keyboardInput.JumpButtonClick += OnJumpButtonClick;
        _playerCollision.FacedWithPlatform += OnFacedWithPlatform;
        _player.Slicing += OnSlicing;
        _player.Sliced += OnSliced;
    }

    private void OnDisable()
    {
        _keyboardInput.JumpButtonClick -= OnJumpButtonClick;
        _playerCollision.FacedWithPlatform -= OnFacedWithPlatform;
        _player.Slicing -= OnSlicing;
        _player.Sliced -= OnSliced;
    }

    private void OnJumpButtonClick()
    {
        if (_canJump == true)
            PrepareJump();
    }

    private void PrepareJump()
    {
        _rigidbody.velocity = Vector3.zero;
        _clockwiseRotation = CheckingRotationAngle(transform.rotation.eulerAngles.z);
        Jumping(_clockwiseRotation);
    }

    private void Jumping(bool clockwiseRotation)
    {
        _rigidbody.AddForce(clockwiseRotation == true? new Vector2(-_jumpDirection.x, _jumpDirection.y) / _decreaseJumpPower : _jumpDirection, ForceMode.Impulse);
        _inJump = true;

        Jumped?.Invoke(clockwiseRotation);
    }

    private void Jump()
    {
        _rigidbody.velocity = Vector3.zero;
        _rotateZ = transform.rotation.eulerAngles.z;
        _clockwiseRotation = CheckingRotationAngle(_rotateZ);

        if (_clockwiseRotation == true)
            _rigidbody.AddForce(new Vector2(-_jumpDirection.x, _jumpDirection.y) / _decreaseJumpPower, ForceMode.Impulse);
        else
            _rigidbody.AddForce(_jumpDirection, ForceMode.Impulse);

        _inJump = true;

    }

    private bool CheckingRotationAngle(float rotationAngle)
    {
        return rotationAngle > _minReverseJumpAngle && rotationAngle < _maxReverseJumpAngle;
    }

    private void OnFacedWithPlatform(Collision collision)
    {
        if (_inJump == true)
        {
            _rigidbody.velocity = Vector3.zero;
            _normal = collision.contacts[0].normal;
            _rigidbody.AddForce(_normal * _reboundRatio, ForceMode.Impulse);
            Jumped?.Invoke(CheckCollisionPoint(collision));
            _inJump = false;
        }
    }

    private bool CheckCollisionPoint(Collision collision)
    {
        return transform.position.x > collision.gameObject.transform.position.x;
    }

    private void OnSlicing()
    {
        _canJump = false;
        _inJump = false;
        _rigidbody.isKinematic = true;

    }

    private void OnSliced()
    {
        _rigidbody.isKinematic = false;
        _canJump = true;
    }
}
