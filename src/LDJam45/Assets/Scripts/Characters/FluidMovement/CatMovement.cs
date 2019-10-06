﻿using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public bool IsActive;

    [SerializeField] private float MoveSpeed = 5.0f;

    private Rigidbody _catBody;
    private Animator _animator;
    private Vector3 _rotation;
    private Vector3 _inputs = Vector3.zero;

    private void Start()
    {
        _catBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!IsActive)
            return;
        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");
        _inputs = Vector3.ClampMagnitude(_inputs, 1f);
        _rotation = Vector3.Normalize(new Vector3(_inputs.x, 0f, _inputs.z));

        if (_inputs != Vector3.zero)
            transform.forward = _rotation;

        if (_inputs == Vector3.zero) {
            _animator.SetBool("IsWalking", false);
            _catBody.velocity = new Vector3(0, _catBody.velocity.y, 0);
        } else {
            _animator.SetBool("IsWalking", true);
            _catBody.MovePosition(_catBody.position + _inputs * MoveSpeed * Time.fixedDeltaTime);
        }
    }

    private void FixedUpdate()
    {
        
    }
}