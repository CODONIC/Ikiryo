using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    private AudioSource _idleAudioSource;

    [SerializeField]
    private AudioSource _chaseAudioSource;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 _targetDirection;
    private bool _isChasing;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
    }

    private void Start()
    {
        _idleAudioSource.Play();
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
        ManageAudio();
    }

    private void UpdateTargetDirection()
    {
        if (_playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
            _isChasing = true;
        }
        else
        {
            _targetDirection = Vector2.zero;
            _isChasing = false;
        }
    }

    private void RotateTowardsTarget()
    {
        if (_targetDirection == Vector2.zero)
        {
            return;
        }

        float angle = Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        _rigidbody.MoveRotation(rotation);
    }

    private void SetVelocity()
    {
        if (_targetDirection == Vector2.zero)
        {
            _rigidbody.velocity = Vector2.zero;
        }
        else
        {
            _rigidbody.velocity = transform.up * _speed;
        }
    }

    private void ManageAudio()
    {
        if (_isChasing)
        {
            if (!_chaseAudioSource.isPlaying)
            {
                _idleAudioSource.Stop();
                _chaseAudioSource.Play();
            }
        }
        else
        {
            if (!_idleAudioSource.isPlaying)
            {
                _chaseAudioSource.Stop();
                _idleAudioSource.Play();
            }
        }
    }
}
