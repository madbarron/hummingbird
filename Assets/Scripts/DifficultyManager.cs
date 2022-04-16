using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public List<DifficultySettings> settings;
    public DifficultySettings Settings { get { return settings[selectedIndex]; } }
    private int selectedIndex = 1;

    public void SelectSettingsIndex(int index)
    {
        if (index >= settings.Count)
        {
            throw new System.Exception("Index out of range: " + index.ToString());
        }

        selectedIndex = index;
    }
}
