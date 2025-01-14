﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject OnDeathVfx;
    [SerializeField] private float IFrames;
    [SerializeField] private Renderer Renderer;
    [SerializeField] private List<GameEvent> OnDeathEvents;
    [SerializeField] private GameState GameState;
    [SerializeField] private CharacterID ID;
    [SerializeField] private AudioClip OnDeathSound;

    [ReadOnly] public Camera Camera;
    public Role Role;
    public int MaxHealth;
    public Action OnDamage { private get; set; } = () => { };
    public float SecondsOfInvincibility;
    public bool IsInvincible;

    private bool _isDead = false;

    private void Awake()
    {
        Camera = FindObjectOfType<Camera>();
    }

    private void Start()
    {
        if (Role.Friendly == Role)
        {
            GameState.MaxHP = GameState.PlayIronmanMode
                ? 1
                : MaxHealth;
            GameState.HealthMap[ID.ID] = GameState.MaxHP;
            GameState.IsInvincibleMap[ID.ID] = SecondsOfInvincibility > 0 || IsInvincible;
        }
        else
        {
            GameState.HealthMap[ID.ID] = MaxHealth;
            GameState.IsInvincibleMap[ID.ID] = SecondsOfInvincibility > 0 || IsInvincible;
        }
    }

    private void Update()
    {
        if (Role == Role.Friendly && Input.GetKey("o") && Input.GetKey("p") && (Input.GetKeyDown("o") || Input.GetKeyDown("p")))
            GameState.HealthMap[ID.ID] += 5;

        SecondsOfInvincibility = Mathf.Max(0, SecondsOfInvincibility - Time.deltaTime);
        GameState.IsInvincibleMap[ID.ID] = SecondsOfInvincibility > 0 || IsInvincible;
    }

    public void ApplyDamage()
    {
        if (_isDead || GameState.IsInvincibleMap[ID.ID])
            return;

        GameState.HealthMap[ID.ID]--;
        if (GameState.HealthMap[ID.ID] <= 0)
        {
            _isDead = true;
            Debug.Log($"{name} is destroyed");
            PlayExplosion();
            StartCoroutine(ResolveDestruction());
        }
        else
        {
            SecondsOfInvincibility = IFrames;
            OnDamage();
        }
    }

    private void PlayExplosion()
    {
        if (OnDeathVfx == null)
            return;

        var explosion = Instantiate(OnDeathVfx, transform.position, transform.rotation);
        explosion.transform.localScale = Renderer.bounds.size;
        var explosionRigidBody = explosion.GetComponent<Rigidbody>();
        var rigidBody = GetComponent<Rigidbody>();
        if (rigidBody != null && explosionRigidBody != null)
            explosionRigidBody.velocity = rigidBody.velocity;
    }

    private IEnumerator ResolveDestruction()
    {
        if (OnDeathSound != null)
            AudioSupport.PlayClipAt(OnDeathSound, Camera.transform.position);
        yield return new WaitForSeconds(0.3f);
        OnDeathEvents.ForEach(x => x.Publish());
        Destroy(gameObject);
    }
}
