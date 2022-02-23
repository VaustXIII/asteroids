using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

public class WrapAroundBehaviour : MonoBehaviour {
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private Vector3[] spriteCornerPositions = new Vector3[4];

    private bool isWrappingX = true;
    private bool isWrappingY = true;

    private void Awake() {
        mainCamera = Camera.main;
        spriteRenderer = this.GetRequiredComponentInChildren<SpriteRenderer>();

        InitializeSpriteCornerPositions();
    }

    private void Update() {
        if (IsVisible()) {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }
        WrapAround();
    }

    private void InitializeSpriteCornerPositions() {
        var bounds = spriteRenderer.bounds;
        var extents = bounds.extents;

        spriteCornerPositions[0] = new Vector3(extents.x, extents.y, 0f);
        spriteCornerPositions[1] = new Vector3(-extents.x, extents.y, 0f);
        spriteCornerPositions[2] = new Vector3(-extents.x, -extents.y, 0f);
        spriteCornerPositions[3] = new Vector3(extents.x, -extents.y, 0f);
    }

    private bool IsVisible() {
        foreach (var corner in spriteCornerPositions) {
            var position = spriteRenderer.bounds.center + corner;
            var viewportPosition = mainCamera.WorldToViewportPoint(position);

            if (viewportPosition.x > 0f &&
                viewportPosition.x < 1f &&
                viewportPosition.y > 0f &&
                viewportPosition.y < 1f) {
                return true;
            }
        }

        return false;
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
