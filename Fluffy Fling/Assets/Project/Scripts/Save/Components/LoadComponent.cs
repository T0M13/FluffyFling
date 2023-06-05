using tomi.SaveSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadBehaviour", menuName = "Behaviours/LoadBehaviour")]
public class LoadComponent : ScriptableObject, LoadBehaviour
{
    public void Load()
    {
        if (SerializationManager.Load(Application.persistentDataPath + "/saves/playerData.milkstudio") != null)
        {
            SaveData.PlayerProfile = (PlayerProfile)SerializationManager.Load(Application.persistentDataPath + "/saves/playerData.milkstudio");
        }
        else
        {
            Debug.Log("Creating Save Profile");
            SetAudio();
            SetLevels();
        }
    }

    private void SetAudio()
    {
        SaveData.PlayerProfile.masterVolume = -40;
        SaveData.PlayerProfile.musicVolume = -40;
        SaveData.PlayerProfile.effectsVolume = -40;
        SaveData.PlayerProfile.musicOn = true;
        SaveData.PlayerProfile.effectsOn = true;

    }

    private void SetLevels()
    {
        int[] tempLevels = new int[10];

        for (int i = 0; i < tempLevels.Length; i++)
        {
            tempLevels[i] = 0;
        }

        SaveData.PlayerProfile.stars = tempLevels;

        int[] tempScores = new int[10];

        for (int i = 0; i < tempScores.Length; i++)
        {
            tempScores[i] = 0;
        }

        SaveData.PlayerProfile.scores = tempScores;
    }
}
