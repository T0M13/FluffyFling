using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle musicToggle;

    private void Start()
    {
        AudioManager.instance.SetSounds();
        //soundToggle.isOn = AudioManager.instance.EffectsOn;
        //musicToggle.isOn = AudioManager.instance.MusicOn;
    }
}
