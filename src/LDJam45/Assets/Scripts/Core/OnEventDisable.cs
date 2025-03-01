﻿using UnityEngine;

public class OnEventDisable : MonoBehaviour
{
    [SerializeField] private GameEvent trigger;
    [SerializeField] private GameObject target;

    private void OnEnable()
    {
        if (trigger == null || target == null)
            Debug.Log("Missing Either Trigger or Target for Event Enable: " + name);
        trigger.Subscribe(() => target.SetActive(false), this);
    }

    private void OnDisable()
    {
        trigger.Unsubscribe(this);
    }
}
