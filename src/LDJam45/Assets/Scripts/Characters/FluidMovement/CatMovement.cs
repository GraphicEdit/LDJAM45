﻿
using Assets.Scripts.Characters;
using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public bool IsActive;

    [SerializeField] private GameState state;
    [SerializeField] private float MoveSpeed = 5.0f;
    [SerializeField] private CatIsOnGround CatIsOnGround;
    [SerializeField] private Rigidbody CatBody;
    [SerializeField] private GameEvent WalkingStarted;
    [SerializeField] private GameEvent WalkingStopped;
    [SerializeField] private CatBodyAnimator CatBodyAnimator;

    private Vector3 _rotation;
    private Vector3 _inputs = Vector3.zero;

    public void Stop()
    {
        CatBodyAnimator.StopWalk();
        WalkingStopped.Publish();
    }

    private void Update()
    {
        if (!IsActive || state.IsInCutscene)
            return;
        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");
        _inputs = Vector3.ClampMagnitude(Vector3.Normalize(new Vector3(_inputs.x, 0f, _inputs.z)), 1f);
        _rotation = Vector3.Normalize(new Vector3(_inputs.x, 0f, _inputs.z));
        if (_inputs != Vector3.zero)
            CatBody.transform.forward = _rotation;
    }

    private void FixedUpdate()
    {
        if (!IsActive)
            return;
        if (_inputs == Vector3.zero)
            Stop();
        else
        {
            CatBodyAnimator.Walk();
            WalkingStarted.Publish();
            CatBody.AddForce(_inputs * MoveSpeed * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }
}
