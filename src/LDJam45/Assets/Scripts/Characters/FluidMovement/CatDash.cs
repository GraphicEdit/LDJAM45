﻿using System;
using System.Collections;
using UnityEngine;

public class CatDash : MonoBehaviour
{
    public Action OnFinished;

    [SerializeField] private float DashSpeed;
    [SerializeField] private float DashLength = 2;
    [SerializeField] private float PostEmitTime;
    [SerializeField] private Health Health;
    [SerializeField] private TrailRenderer DashTrail;
    [SerializeField] private float DashCooldown;
    [SerializeField] private GameEvent OnStarted;
    [SerializeField] private Rigidbody CatBody;

    private bool _isDashing;
    private float _dashTime;
    private Vector3 _direction;

    public float DashCooldownRemaining;

    public void Dash(Vector3 direction)
    {
        if (DashCooldownRemaining <= 0)
        {
            _isDashing = true;
            Health.IsInvincible = true;
            _dashTime = DashLength;
            DashTrail.emitting = true;
            CatBody.useGravity = false;
            _direction = direction;
            DashCooldownRemaining = DashCooldown;
            OnStarted.Publish();
        }
        else
            OnFinished();
    }

    void Update()
    {
        DashCooldownRemaining = Mathf.Max(0, DashCooldownRemaining - Time.deltaTime);
        if (!_isDashing)
            return;

        if (_direction != Vector3.zero)
            CatBody.transform.forward = _direction;
        CatBody.AddForce(CatBody.transform.forward * DashSpeed * Time.deltaTime, ForceMode.Force);

        _dashTime -= Time.deltaTime;
        if (_dashTime <= 0)
        {
            _isDashing = false;
            Health.IsInvincible = false;
            CatBody.useGravity = true;
            StartCoroutine(TurnOffEmissionsAfterDelay());
            OnFinished();
        }
    }

    private IEnumerator TurnOffEmissionsAfterDelay()
    {
        yield return new WaitForSeconds(PostEmitTime);
        if (!_isDashing)
            DashTrail.emitting = false;
    }
}
