using UnityEngine;

public class FlyingSaucerMovementLogic {

    private FlyingSaucerMovementData data;
    private Transform target;

    private Vector2 velocity;

    public FlyingSaucerMovementLogic(FlyingSaucerMovementData data, Transform target) {
        this.data = data;
        this.target = target;
    }

    public Vector3 GetPositionDelta(Vector3 from, float dt) {
        if (target == null) {
            return Vector3.zero;
        }

        velocity = target.position - from;
        velocity.Normalize();
        velocity *= data.maxSpeed;

        var positionDelta = dt * velocity;
        return positionDelta;
    }
}
