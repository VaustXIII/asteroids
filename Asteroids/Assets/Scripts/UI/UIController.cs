using UnityEngine;

public class UIController : MonoBehaviour {
    public event System.Action retryButtonClicked;

    [SerializeField] private InGameStatsPanelController inGameStatsPanel;
    [SerializeField] private GameOverPanelController gameOverPanel;

    private void Awake() {
        inGameStatsPanel = inGameStatsPanel ?? this.GetRequiredComponentInChildren<InGameStatsPanelController>();
        gameOverPanel = gameOverPanel ?? this.GetRequiredComponentInChildren<GameOverPanelController>();

        inGameStatsPanel.gameObject.SetActive(true);
        gameOverPanel.gameObject.SetActive(false);
    }

    private void OnEnable() {
        gameOverPanel.RetryButtonClicked.AddListener(OnRetryButtonClicked);
    }

    private void OnDisable() {
        gameOverPanel.RetryButtonClicked.RemoveListener(OnRetryButtonClicked);
    }

    public void UpdateIngameStats(PlayerState playerState) {
        inGameStatsPanel.UpdatePanel(playerState);
    }

    public void OnGameOver(int playerScore) {
        inGameStatsPanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.OnGameOver(playerScore);
    }

    private void OnRetryButtonClicked() {
        retryButtonClicked?.Invoke();
    }
}
