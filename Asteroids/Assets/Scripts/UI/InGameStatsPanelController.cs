using UnityEngine;
using UnityEngine.UI;

public class InGameStatsPanelController : MonoBehaviour {
    [SerializeField] private Text positionText;
    [SerializeField] private Text rotationText;
    [SerializeField] private Text velocityText;
    [SerializeField] private Text lasertShotsCountText;
    [SerializeField] private Text lasertShotsCooldownText;

    public void UpdatePanel(PlayerState playerState) {
        positionText.text = $"Position: {playerState.position}";
        rotationText.text = $"Rotation angle: {playerState.rotationAngle}";
        velocityText.text = $"Velocity: {playerState.velocity}, speed: {playerState.velocity.magnitude}";
        lasertShotsCountText.text = $"Laser shots: {playerState.laserShotsCount}";
        lasertShotsCooldownText.text = $"Laser cooldown: {playerState.laserShotsCooldown}";
    }
}
