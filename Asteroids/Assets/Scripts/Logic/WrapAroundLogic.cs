using UnityEngine;

public class WrapAroundLogic {
    private Vector3[] spriteCornerPositions = new Vector3[4];

    private bool isWrappingX = true;
    private bool isWrappingY = true;



    public WrapAroundLogic(Bounds spriteBounds) {
        InitializeSpriteCornerPositions(spriteBounds);
    }

    public Vector3 UpdatePosition(Vector3 currentPosition, Camera mainCamera) {
        if (IsVisible(currentPosition, mainCamera)) {
            isWrappingX = false;
            isWrappingY = false;
            return currentPosition;
        }

        return MaybeWrapAround(currentPosition, mainCamera);
    }

    private void InitializeSpriteCornerPositions(Bounds bounds) {
        var extents = bounds.extents;

        spriteCornerPositions[0] = new Vector3(extents.x, extents.y, 0f);
        spriteCornerPositions[1] = new Vector3(-extents.x, extents.y, 0f);
        spriteCornerPositions[2] = new Vector3(-extents.x, -extents.y, 0f);
        spriteCornerPositions[3] = new Vector3(extents.x, -extents.y, 0f);
    }

    private bool IsVisible(Vector3 currentPosition, Camera camera) {
        foreach (var corner in spriteCornerPositions) {
            var position = currentPosition + corner;
            var viewportPosition = camera.WorldToViewportPoint(position);

            if (viewportPosition.x > 0f &&
                viewportPosition.x < 1f &&
                viewportPosition.y > 0f &&
                viewportPosition.y < 1f) {
                return true;
            }
        }

        return false;
    }

    private Vector3 MaybeWrapAround(Vector3 currentPosition, Camera mainCamera) {
        if (isWrappingX && isWrappingY) { return currentPosition; }

        var newPosition = currentPosition;
        var viewPortPosition = mainCamera.WorldToViewportPoint(currentPosition);
        if (!isWrappingX && (viewPortPosition.x > 1.0f || viewPortPosition.x < 0f)) {
            newPosition.x = -newPosition.x;
            isWrappingX = true;
        }
        if (!isWrappingY && (viewPortPosition.y > 1.0f || viewPortPosition.y < 0f)) {
            newPosition.y = -newPosition.y;
            isWrappingY = true;
        }

        return newPosition;
    }

}
