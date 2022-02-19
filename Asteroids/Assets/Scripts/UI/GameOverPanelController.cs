using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelController : MonoBehaviour {
    [SerializeField] Text scoreText;
    [SerializeField] Button retryButton;

    public Button.ButtonClickedEvent RetryButtonClicked => retryButton.onClick;

    public void OnGameOver(int score) {
        scoreText.text = $"Your final score is {score}";
    }
}
