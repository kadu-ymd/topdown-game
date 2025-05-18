using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioSource musicSource;
    public Slider masterSlider;
    public Slider musicSlider;

    void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        LoadVolume();
    }

    public void LoadVolume()
    {
        AudioListener.volume = masterSlider.value;  // Ajusta o volume mestre
        musicSource.volume = musicSlider.value;  // Ajusta diretamente o volume

        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.Save();
    }
}