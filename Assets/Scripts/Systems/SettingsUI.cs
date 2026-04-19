using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private GameObject settingsPanel;

    [Header("Buttons")]
    [SerializeField] private Button backBtn;
    [SerializeField] private Button logOutBtn;
    [SerializeField] private Button deleteAccount;

    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";

    void OnEnable()
    {
        if (backBtn != null)
            backBtn.onClick.AddListener(OnBack);

        if (logOutBtn != null)
            logOutBtn.onClick.AddListener(OnLogout);

        if (deleteAccount != null)
            deleteAccount.onClick.AddListener(OnDeleteAccount);
    }

    void OnDisable()
    {
        if (backBtn != null)
            backBtn.onClick.RemoveListener(OnBack);

        if (logOutBtn != null)
            logOutBtn.onClick.RemoveListener(OnLogout);

        if (deleteAccount != null)
            deleteAccount.onClick.RemoveListener(OnDeleteAccount);
    }

    private void Start()
    {
        float music = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfx = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        musicSlider.value = music;
        sfxSlider.value = sfx;

        SetMusicVolume(music);
        SetSFXVolume(sfx);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat(MUSIC_KEY, value);
        AudioListener.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat(SFX_KEY, value);
        AudioListener.volume = value;
    }

    public void OnBackButton()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OnBack()
    {
        Debug.Log("Back Btn clicked");

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OnLogout()
    {
        Debug.Log("Logout Btn clicked");

        PlayerPrefs.DeleteKey("UserToken");
    }

    public void OnDeleteAccount()
    {
        Debug.Log("Delete Btn Account clicked");
    }
}