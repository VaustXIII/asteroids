using UnityEngine;

public class AsteroidMovementLogic {
    private AsteroidMovementData data;

    private float angularSpeed;
    private Vector3 velocity;

    public AsteroidMovementLogic(AsteroidMovementData data, Vector2 direction) {
        this.data = data;

        var speed = Random.Range(data.initialSpeedMin, data.initialSpeedMax);
        velocity = speed * direction.normalized;

        angularSpeed = Random.Range(data.initialAngularSpeedMin, data.initialAngularSpeedMax);
        angularSpeed = RandomExt.Bool() ? angularSpeed : -angularSpeed;
    }

    public Vector3 GetPositionDelta(float dt) {
        return dt * velocity;
    }

    public Vector3 GetRotationDelta(float dt) {
        return dt * angularSpeed * Vector3.forward;
    }
}
