using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _zoomOffset;
    [SerializeField] private Player _player;
    [SerializeField] private Vector3 _zoomRotation;
    [SerializeField] private PlayerJump _playerJump;
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _zoomSpeed;

    private float _lastSpeed;
    private Vector3 _lastOffset;
    private Quaternion _startQuaternion;
    private Quaternion _zoomQuaternion;
    private IEnumerator _coroutine;
    private bool _coroutineIsActive = false;

    private void Awake()
    {
        _startQuaternion = Quaternion.Euler(transform.position);
        _zoomQuaternion = Quaternion.Euler(_zoomRotation);
        _lastOffset = _offset;
        _lastSpeed = _followSpeed;
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
        Follow(_player, _lastOffset, _lastSpeed);
    }

    private Vector3 SetStartPosition(Player player, Vector3 offset)
    {
        return new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, player.transform.position.z + offset.z);
    }

    private void Follow(Player player, Vector3 offset, float speed)
    {
        Vector3 target = SetStartPosition(player, offset);
        Vector3 currentPosition = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        transform.position = currentPosition;
    }

    private void OnSlicing()
    {
        /*if (_coroutineIsActive == true)
            StopActiveCoroutine();

        _coroutine = ChangeRotation(_zoomQuaternion);
        StartCoroutine(_coroutine);*/

        if (_coroutineIsActive == true)
            StopActiveCoroutine();

        _lastSpeed = _zoomSpeed;
        _lastOffset = _zoomOffset;

        _coroutine = ChangeRotation(_zoomQuaternion);
        StartCoroutine(_coroutine);
    }

    private void OnJumped(bool clockwiseRotation)
    {
        /*if (_coroutineIsActive == true)
            StopActiveCoroutine();

        _lastSpeed = _followSpeed;
        _lastOffset = _offset;

        if(transform.rotation != _startQuaternion)
        {
            _coroutine = ChangeRotation(_startQuaternion);
            StartCoroutine(_coroutine);
        }*/
    }

    /*private void OnSliced()
    {

    }*/

    private IEnumerator ChangeRotation(Quaternion rotation)
    {
        _coroutineIsActive = true;

        while (transform.rotation != rotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _zoomSpeed * Time.deltaTime * 2f);       // Magical number
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