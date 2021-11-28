using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(KeyboardInput), typeof(Rigidbody), typeof(PlayerCollision))]
[RequireComponent(typeof(Player))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField] private Vector2 _jumpDirection;
    [SerializeField] private float _minReverseJumpAngle, _maxReverseJumpAngle;
    [SerializeField] private float _decreaseJumpPower;

    private bool _inJump = false;
    private bool _canJump = true;
    private bool _jumpOnClockwiseRotation = false;
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
        _jumpOnClockwiseRotation = CheckingRotationAngle(transform.rotation.eulerAngles.z);
        Jumping(DetermineDirection(_jumpOnClockwiseRotation), _jumpOnClockwiseRotation);
    }

    private bool CheckingRotationAngle(float rotationAngle)
    {
        return rotationAngle > _minReverseJumpAngle && rotationAngle < _maxReverseJumpAngle;
    }

    private Vector3 DetermineDirection(bool jumpBack)
    {
        return jumpBack == true ? new Vector2(-_jumpDirection.x, _jumpDirection.y): _jumpDirection;
    }

    private void Jumping(Vector3 direction, bool jumpBack)
    {
        if (jumpBack == true)
            direction /= _decreaseJumpPower;

        _rigidbody.AddForce(direction, ForceMode.VelocityChange);
        _inJump = true;

        Jumped?.Invoke(jumpBack);
    }

    private void OnFacedWithPlatform(bool collisionOnLeft)
    {
        if (_inJump == true)
        {
            _rigidbody.velocity = Vector3.zero;
            Jumping(DetermineDirection(collisionOnLeft), _inJump);

            Jumped?.Invoke(!collisionOnLeft);
        }
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
