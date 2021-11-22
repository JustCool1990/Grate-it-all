using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(KeyboardInput), typeof(Rigidbody), typeof(PlayerCollision))]
[RequireComponent(typeof(Player))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField] private Vector2 _jumpDirection;
    //[SerializeField] private float _rotationSpeed;
    //[SerializeField] private float _maxDecreasedRotationSpeed;
    //[SerializeField] private float _decelerationRotationSpeed;
    [SerializeField] private float _reboundRatio;
    [SerializeField] private float _minReverseJumpAngle, _maxReverseJumpAngle;
    [SerializeField] private float _decreaseJumpPower;

    private Vector3 _normal;
    private bool _inJump = false;
    private bool _canJump = true;
    private float _rotateZ = 0;
    //private float _fullTurnover = 360;
    //private float _decelerationRotationCoefficient = 0;
    private bool _clockwiseRotation = false;
    private KeyboardInput _keyboardInput;
    private Rigidbody _rigidbody;
    private PlayerCollision _playerCollision;
    private Player _player;
    //private IEnumerator _coroutine;
    //private bool _coroutineIsActive = false;

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
        _keyboardInput.PressedJumpButton += OnPressedJumpButton;
        _playerCollision.FacedWithPlatform += OnFacedWithPlatform;
        _player.Slicing += OnSlicing;
        _player.Sliced += OnSliced;
    }

    private void OnDisable()
    {
        _keyboardInput.PressedJumpButton -= OnPressedJumpButton;
        _playerCollision.FacedWithPlatform -= OnFacedWithPlatform;
        _player.Slicing -= OnSlicing;
        _player.Sliced -= OnSliced;
    }

    private void FixedUpdate()
    {
        Debug.Log(transform.rotation.eulerAngles.z);
    }

    private void OnPressedJumpButton()
    {
        if (_canJump == true)
            PrepareJump();
    }

    private void PrepareJump()
    {
        _rigidbody.velocity = Vector3.zero;
        //_decelerationRotationCoefficient = 0;
        //_rotateZ = transform.rotation.eulerAngles.z;
        //_clockwiseRotation = CheckingRotationAngle(_rotateZ);
        _clockwiseRotation = CheckingRotationAngle(transform.rotation.eulerAngles.z);

        Jumping(_clockwiseRotation);
    }

    private void Jumping(bool clockwiseRotation)
    {
        _rigidbody.AddForce(clockwiseRotation == true? new Vector2(-_jumpDirection.x, _jumpDirection.y) / _decreaseJumpPower : _jumpDirection, ForceMode.Impulse);
        _inJump = true;

        Jumped?.Invoke(clockwiseRotation);

        /*if (_coroutineIsActive == true)
            StopActiveCoroutine();

        _coroutine = ChangeRotation();
        StartCoroutine(_coroutine);*/
    }

    private void Jump()
    {
        _rigidbody.velocity = Vector3.zero;
        //_decelerationRotationCoefficient = 0;

        //float rotationAngle = transform.rotation.eulerAngles.z;
        _rotateZ = transform.rotation.eulerAngles.z;
        _clockwiseRotation = CheckingRotationAngle(_rotateZ);

        if (_clockwiseRotation == true)
            _rigidbody.AddForce(new Vector2(-_jumpDirection.x, _jumpDirection.y) / _decreaseJumpPower, ForceMode.Impulse);
        else
            _rigidbody.AddForce(_jumpDirection, ForceMode.Impulse);

        //if (rotationAngle > 0)
            //_rotateZ = transform.rotation.eulerAngles.z;

        _inJump = true;

        /*if (_coroutineIsActive == true)
            StopActiveCoroutine();

        _coroutine = ChangeRotation();
        StartCoroutine(_coroutine);*/
    }

    private bool CheckingRotationAngle(float rotationAngle)
    {
        return rotationAngle > _minReverseJumpAngle && rotationAngle < _maxReverseJumpAngle;
    }

    /*private IEnumerator ChangeRotation()
    {
        _coroutineIsActive = true;

        while (Mathf.Abs(_rotateZ) < _fullTurnover)
        {
            _rotateZ -= (_rotationSpeed * Time.deltaTime - _decelerationRotationCoefficient) * (_clockwiseRotation == true ? -1 : 1);
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
    }*/

    private void OnFacedWithPlatform(Collision collision)
    {
        if (_inJump == true)
        {
            _rigidbody.velocity = Vector3.zero;

            /*if (_coroutineIsActive == true)
                StopActiveCoroutine();*/

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
        /*if (_coroutineIsActive == true)
            StopActiveCoroutine();

        StopRotation();*/

        _canJump = false;
        _inJump = false;
        _rigidbody.isKinematic = true;

    }

    private void OnSliced()
    {
        _rigidbody.isKinematic = false;
        _canJump = true;
    }
    
    /*private void StopActiveCoroutine()
    {
        StopCoroutine(_coroutine);
        _coroutineIsActive = false;
    }*/
}
