using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;

    private void OnEnable()
    {
        if (playBtn != null)
            playBtn.onClick.AddListener(OnPlayButton);

        if (quitBtn != null)
            quitBtn.onClick.AddListener(OnQuitButton);
    }

    private void OnDisable()
    {
        if (playBtn != null)
            playBtn.onClick.RemoveListener(OnPlayButton);

        if (quitBtn != null)
            quitBtn.onClick.RemoveListener(OnQuitButton);
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnSettingButton()
    {
        if (settingPanel != null)
            settingPanel.SetActive(!settingPanel.activeSelf);
    }

    public void OnQuitButton()
    {
        Debug.Log("Quit Button Pressed");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_ANDROID
        // Moves app to background (correct Android behavior)
        AndroidJavaObject activity =
            new AndroidJavaClass("com.unity3d.player.UnityPlayer")
            .GetStatic<AndroidJavaObject>("currentActivity");

        activity.Call<bool>("moveTaskToBack", true);

#else
        Application.Quit();
#endif
    }
}