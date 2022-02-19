using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour {
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float turnRate = 200f;
    [SerializeField] private float frictionRate = .2f;


    private Vector2 currentVelocity = Vector2.zero;
    private float currentAcceleration = 0f;
    private float currentTurnRate = 0f;

    private void Update() {
        GetInputs();
        Move();
    }

    private void GetInputs() {
        currentAcceleration = 0f;
        if (Input.GetKey(KeyCode.W)) {
            currentAcceleration = acceleration;
        }
        else if (Input.GetKey(KeyCode.S)) {

            currentAcceleration = -acceleration;
        }


        currentTurnRate = 0f;
        if (Input.GetKey(KeyCode.A)) {
            currentTurnRate = turnRate;
        }
        else if (Input.GetKey(KeyCode.D)) {
            currentTurnRate = -turnRate;
        }
    }

    private void Move() {
        float rotation = currentTurnRate * Time.deltaTime;
        transform.Rotate(0, 0, rotation, Space.Self);

        Vector2 velocityDelta = currentAcceleration * Time.deltaTime * (Vector2)transform.up - frictionRate * currentVelocity;
        currentVelocity += velocityDelta;
        
        if (currentVelocity.sqrMagnitude > maxSpeed*maxSpeed) {
            currentVelocity = maxSpeed * currentVelocity.normalized;
        }

        Vector2 positionDelta = Time.deltaTime * currentVelocity;
        transform.Translate(positionDelta.x, positionDelta.y, 0f, Space.World);
    }
}
