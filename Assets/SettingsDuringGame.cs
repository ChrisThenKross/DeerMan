using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsDuringGame : MonoBehaviour
{
    public GameObject SettingsPanel;

    public void SettingTrigger()
    {
        SettingsPanel.SetActive(true);
    }

    public void SettingsDeTrigger()
    {
        SettingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
