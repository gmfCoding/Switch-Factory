using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class EscMenu : MonoBehaviour
{

    public GameObject menu;
    bool status => menu.activeInHierarchy;

    public Slider volume;

    public void Awake()
    {
        if (PlayerPrefs.HasKey("volume"))
            volume.value = PlayerPrefs.GetFloat("volume");
        else
            PlayerPrefs.SetFloat("volume", volume.value);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            menu.SetActive(!status);
    }

    public void OnVolumeChanged()
    {
        AudioListener.volume = volume.value;
        PlayerPrefs.SetFloat("volume", volume.value);
    }
}
