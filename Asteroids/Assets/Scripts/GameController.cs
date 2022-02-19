using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private UIController uiController;
    [SerializeField] private PlayerShipBehaviour playerShip;

    private void Awake() {
        uiController = uiController ?? FindObjectOfType<UIController>();
        playerShip = playerShip ?? FindObjectOfType<PlayerShipBehaviour>();
    }

    private void Update() {
        // Сначала думал сделать рекативно через event'ы,
        // но почти все данные отображаемые в UI меняются каждый кадр
        // Кажется лишний оверхед, поэтому просто апдейт
        uiController.UpdateIngameStats(playerShip.GetState());
    }
}
