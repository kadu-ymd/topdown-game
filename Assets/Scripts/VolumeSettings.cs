using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioSource musicSource;
    public Slider masterSlider;
    public Slider musicSlider;

    void Start()
    {
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>(); // Busca automaticamente
        }

        if (!PlayerPrefs.HasKey("MasterVolume") || !PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MasterVolume", 0.4f);
            PlayerPrefs.SetFloat("MusicVolume", 0.4f);
        }

        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.4f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.4f);

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