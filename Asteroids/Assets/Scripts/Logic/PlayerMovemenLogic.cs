using UnityEngine;

public class PlayerMovemenLogic {
    private PlayerMovementData data;

    private Vector2 currentVelocity;
    private float currentAcceleration;
    private float currentTurnRate;

    public Vector2 CurrentVelocity => currentVelocity;

    public PlayerMovemenLogic(PlayerMovementData data) {
        this.data = data;
    }

    public float GetRotationDelta(GameInput input, float dt) {
        currentTurnRate = data.turnRate * -input.turn;

        float rotation = currentTurnRate * dt;
        return rotation;
    }

    public Vector3 GetPositionDelta(Vector3 currentUp, GameInput input, float dt) {
        currentAcceleration = input.forward > 0 ? data.acceleration * input.forward : 0f;

        Vector2 velocityDelta = currentAcceleration * dt * (Vector2)currentUp - data.frictionRate * currentVelocity;
        currentVelocity += velocityDelta;

        if (currentVelocity.sqrMagnitude > data.maxSpeed * data.maxSpeed) {
            currentVelocity = data.maxSpeed * currentVelocity.normalized;
        }

        Vector2 positionDelta = dt * currentVelocity;
        return positionDelta;
    }
}
