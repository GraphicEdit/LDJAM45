﻿using UnityEngine;

public class CooldownPresenter : MonoBehaviour
{
    [SerializeField] private GameObject slashHud;
    [SerializeField] private GameObject rendHud;
    [SerializeField] private GameObject dashHud;
    [SerializeField] private GameObject laserHud;
    [SerializeField] private GameState state;
    
    // TODO: Maybe make this reactive to save CPU
    private void Update()
    {
        slashHud.SetActive(state.SlashUnlocked);
        rendHud.SetActive(state.RendUnlocked);
        dashHud.SetActive(state.DashUnlocked);
        laserHud.SetActive(state.LaserEyesUnlocked);
    }
}
