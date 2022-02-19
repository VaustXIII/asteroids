using UnityEngine;
using UnityEngine.Assertions;

public class WrapAroundBehaviour : MonoBehaviour {
    private SpriteRenderer[] spriteRenderers;
    private Camera mainCamera;

    private bool isWrappingX;
    private bool isWrappingY;

    private void Start() {
        mainCamera = Camera.main;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        Assert.AreNotEqual(spriteRenderers.Length, 0,
            $"{gameObject.name}.{this.GetType()}: expected to have some sprite renderers on a wrap-around object");
    }

    private void Update() {
        if (IsOffscreen()) {
            WrapAround();
        }
        else {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

    }

    private bool IsOffscreen() {
        foreach (var sprite in spriteRenderers) {
            if (sprite.isVisible) {
                return false;
            }
        }
        return true;
    }

    private void WrapAround() {
        if (isWrappingX && isWrappingY) { return; }

        var newPosition = transform.position;
        var viewPortPosition = mainCamera.WorldToViewportPoint(transform.position);
        if (!isWrappingX && (viewPortPosition.x > 1.0f || viewPortPosition.x < 0f)) {
            newPosition.x = -newPosition.x;
            isWrappingX = true;
        }
        if (!isWrappingY && (viewPortPosition.y > 1.0f || viewPortPosition.y < 0f)) {
            newPosition.y = -newPosition.y;
            isWrappingY = true;
        }

        transform.position = newPosition;
    }
}
