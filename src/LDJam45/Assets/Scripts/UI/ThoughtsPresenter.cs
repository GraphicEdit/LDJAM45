﻿using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class ThoughtsPresenter : MonoBehaviour
{
    [SerializeField] private GameState state;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float showDuration = 2f;
    [SerializeField] private float transitionDuration = 1f;

    private Color targetColor;
    private Color targetTransparent;

    private float _fadingInFinishedInSeconds;
    private float _startFadingOutInSeconds;
    private float _finishInSeconds;
    private bool _finishedCurrent;

    private void Awake()
    {
        targetColor = text.color;
        targetTransparent = new Color(targetColor.r, targetColor.g, targetColor.b, 255.0f);
    }

    private void FixedUpdate()
    {
        // UpdateCounters();
        // UpdatePresentation();

        if (state.ThoughtsMessageQueue.Count > 0)
            StartNextMessage();
    }

    private void UpdatePresentation()
    {
        if (_finishedCurrent)
            return;

        if (_fadingInFinishedInSeconds > 0.01f)
            text.color = Color.Lerp(targetTransparent, targetColor, (transitionDuration - _fadingInFinishedInSeconds) / transitionDuration);
        if (_startFadingOutInSeconds < 0.1f)
            text.color = Color.Lerp(targetColor, targetTransparent, (transitionDuration - _finishInSeconds) / transitionDuration);
        if (_finishInSeconds < 0.01f)
        {
            _finishedCurrent = true;
            text.text = string.Empty;
        }
    }

    private void UpdateCounters()
    {
        _fadingInFinishedInSeconds = Mathf.Max(0, _fadingInFinishedInSeconds - Time.deltaTime);
        _startFadingOutInSeconds = Mathf.Max(0, _startFadingOutInSeconds - Time.deltaTime);
        _finishInSeconds = Mathf.Max(0, _finishInSeconds - Time.deltaTime);
    }

    private void StartNextMessage()
    {
        _finishedCurrent = false;
        // Transform parent = text.transform.parent;
        text.transform.localScale = Vector3.zero;
        text.color = targetTransparent;
        text.text = state.ThoughtsMessageQueue.Dequeue();

        text.transform.DOScale(Vector3.one, 1.0f).OnComplete(() => {
            StartCoroutine(DescaleThought(text));
        });

        // _finishInSeconds = showDuration + transitionDuration * 2;
        // _fadingInFinishedInSeconds = transitionDuration;
        // _startFadingOutInSeconds = transitionDuration + showDuration;
    }
    
    IEnumerator DescaleThought(TextMeshProUGUI text) {                
        yield return new WaitForSeconds(2.0f);

        text.transform.DOScale(Vector3.zero, 1.0f).OnComplete(() => {
            text.text = "";
        });        
    }
}