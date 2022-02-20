using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    [SerializeField] private UIController uiController;
    [SerializeField] private PlayerShipBehaviour playerShip;
    [SerializeField] private AsteroidSpawnerBehaviour asteroidSpawner;

    private int playerScore;

    private void Awake() {
        uiController = uiController ?? FindObjectOfType<UIController>();
        playerShip = playerShip ?? FindObjectOfType<PlayerShipBehaviour>();
        asteroidSpawner = asteroidSpawner ?? FindObjectOfType<AsteroidSpawnerBehaviour>();
    }

    private void OnEnable() {
        playerShip.crashed += OnPlayerCrashed;
        uiController.retryButtonClicked += OnRetry;
        asteroidSpawner.spawned += OnScoreableObjectSpawned;
    }

    private void OnDisable() {
        playerShip.crashed -= OnPlayerCrashed;
        uiController.retryButtonClicked -= OnRetry;
        asteroidSpawner.spawned -= OnScoreableObjectSpawned;
    }

    private void Update() {
        // Сначала думал сделать рекативно через event'ы,
        // но почти все данные отображаемые в UI меняются каждый кадр
        // Кажется лишний оверхед, поэтому просто апдейт
        uiController.UpdateIngameStats(playerShip.GetState());
    }

    private void OnPlayerCrashed() {
        playerShip.gameObject.SetActive(false);
        asteroidSpawner.gameObject.SetActive(false);
        uiController.OnGameOver(playerScore);
    }

    private void OnRetry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnScoreableObjectSpawned(IScoreable obj) {
        obj.scored += OnScored;
    }

    private void OnScored(int score) {
        playerScore += score;
    }
}
