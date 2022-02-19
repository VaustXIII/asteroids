using UnityEngine;

public class UIController : MonoBehaviour {

    [SerializeField] private InGameStatsPanelController inGameStatsPanel;

    private void Awake() {
        inGameStatsPanel = inGameStatsPanel ?? this.GetRequiredComponentInChildren<InGameStatsPanelController>();

    }

    public void UpdateIngameStats(PlayerState playerState) {
        inGameStatsPanel.UpdatePanel(playerState);
    }
}
