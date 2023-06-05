using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

public class SoundTogglers : MonoBehaviour
{
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle musicToggle;
    private bool isLoading;

    private void Start()
    {
        soundToggle.onValueChanged.AddListener(OnToggleValueChangedSound);
        musicToggle.onValueChanged.AddListener(OnToggleValueChangedMusic);

        LoadData();
    }

    private void OnToggleValueChangedSound(bool isOn)
    {
        if (!isLoading)
        {
            AudioManager.instance.ToggleSound();
        }
    }

    private void OnToggleValueChangedMusic(bool isOn)
    {
        if (!isLoading)
        {
            AudioManager.instance.ToggleMusic();
        }
    }

    private void LoadData()
    {
        isLoading = true;

        soundToggle.isOn = SaveData.PlayerProfile.effectsOn;
        musicToggle.isOn = SaveData.PlayerProfile.musicOn;

        AudioManager.instance.SetSounds();
        isLoading = false;
    }
}
