using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI gemText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Buttons")]
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button shopBtn;

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject messagePanel;

    [Header("Message")]
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Result Screen")]
    [SerializeField] private TextMeshProUGUI resultWaveText;
    [SerializeField] private TextMeshProUGUI resultEnemiesText;
    [SerializeField] private TextMeshProUGUI resultCoinsText;

    #region Initialization

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        if (pauseBtn != null)
            pauseBtn.onClick.AddListener(TogglePause);

        if (shopBtn != null)
            shopBtn.onClick.AddListener(ToggleShop);
    }

    private void OnDisable()
    {
        if (pauseBtn != null)
            pauseBtn.onClick.RemoveListener(TogglePause);

        if (shopBtn != null)
            shopBtn.onClick.RemoveListener(ToggleShop);
    }

    private void Start()
    {
        // Subscribe to game events
        GameManager.Instance.OnCoinsChanged += UpdateCoins;
        GameManager.Instance.OnGemsChanged += UpdateGems;
        GameManager.Instance.OnHPChanged += UpdateHP;
        GameManager.Instance.OnGameOver += ShowGameOver;
        GameManager.Instance.OnVictory += ShowVictory;

        WaveManager.Instance.OnWaveStart += UpdateWave;

        // Initial UI sync
        UpdateCoins(GameManager.Instance.Coins);
        UpdateGems(GameManager.Instance.Gems);
        UpdateHP(GameManager.Instance.PlayerHP);
        UpdateWave(0);

        CloseAllPanels();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnCoinsChanged -= UpdateCoins;
        GameManager.Instance.OnGemsChanged -= UpdateGems;
        GameManager.Instance.OnHPChanged -= UpdateHP;
        GameManager.Instance.OnGameOver -= ShowGameOver;
        GameManager.Instance.OnVictory -= ShowVictory;

        if (WaveManager.Instance != null)
            WaveManager.Instance.OnWaveStart -= UpdateWave;
    }

    #endregion

    #region HUD Updates

    private void UpdateCoins(int value) => coinText?.SetText("{0}", value);
    private void UpdateGems(int value) => gemText?.SetText("{0}", value);
    private void UpdateHP(int value) => hpText?.SetText("{0}", value);
    private void UpdateWave(int value) => waveText?.SetText(value == 0 ? "Wave --" : $"Wave {value}");

    #endregion

    #region Panel Control

    private void CloseAllPanels()
    {
        pausePanel?.SetActive(false);
        shopPanel?.SetActive(false);
        gameOverPanel?.SetActive(false);
        victoryPanel?.SetActive(false);
        messagePanel?.SetActive(false);
    }

    private void OpenPanel(GameObject panel)
    {
        if (panel == null) return;

        CloseAllPanels();
        panel.SetActive(true);
    }

    public void TogglePause()
    {
        if (pausePanel == null) return;

        bool open = !pausePanel.activeSelf;

        CloseAllPanels();
        pausePanel.SetActive(open);

        Time.timeScale = open ? 0f : 1f;
    }

    public void ToggleShop()
    {
        if (shopPanel == null) return;

        bool open = !shopPanel.activeSelf;

        CloseAllPanels();
        shopPanel.SetActive(open);
    }

    public void ResumeGame()
    {
        pausePanel?.SetActive(false);
        Time.timeScale = 1f;
    }

    #endregion

    #region Game State Panels

    private void ShowGameOver()
    {
        OpenPanel(gameOverPanel);
        FillResultScreen();
        Time.timeScale = 0f;
    }

    private void ShowVictory()
    {
        OpenPanel(victoryPanel);
        FillResultScreen();
        Time.timeScale = 0f;
    }

    private void FillResultScreen()
    {
        int wave = WaveManager.Instance != null ? WaveManager.Instance.CurrentWave : 0;

        resultWaveText?.SetText("Wave Reached: {0}", wave);
        resultEnemiesText?.SetText("Enemies Defeated: {0}", GameManager.Instance.EnemiesDefeated);
        resultCoinsText?.SetText("Coins Earned: {0}", GameManager.Instance.CoinsEarned);
    }

    #endregion

    #region Navigation Buttons

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameManager.Instance.RestartGame();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        GameManager.Instance.GoToMainMenu();
    }

    #endregion

    #region Message System

    public void ShowMessage(string message, float duration = 2f)
    {
        if (messagePanel == null || messageText == null) return;

        messageText.text = message;
        messagePanel.SetActive(true);

        CancelInvoke(nameof(HideMessage));
        Invoke(nameof(HideMessage), duration);
    }

    private void HideMessage()
    {
        messagePanel?.SetActive(false);
    }

    #endregion
}