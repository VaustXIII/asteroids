using UnityEngine;

public class WrapAroundBehaviour : MonoBehaviour {
    private Camera mainCamera;

    private WrapAroundLogic wrapAroundLogic;

    private void Awake() {
        mainCamera = Camera.main;
        var spriteRenderer = this.GetRequiredComponentInChildren<SpriteRenderer>();

        wrapAroundLogic = new WrapAroundLogic(spriteRenderer.bounds);
    }

    private void Update() {
        transform.position = wrapAroundLogic.UpdatePosition(transform.position, mainCamera);
    }
}
