using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class GameInput {
    public float forward;
    public float turn;
    public bool wasFirePressed;
}

public class PlayerShipBehaviour : MonoBehaviour {

    [Header("Movement")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float turnRate = 200f;
    [SerializeField] private float frictionRate = .2f;


    [Header("Combat")]
    [SerializeField] private BulletBehaviour bulletPrefab;

    [Range(0.1f, 10f)]
    [SerializeField] private float fireRate = 2;


    private Vector2 currentVelocity;
    private float currentAcceleration;
    private float currentTurnRate;

    private float timeBetweenShots;
    private float lastShotTime;


    private GameInput input = new GameInput();

    private void Awake() {
        Assert.AreNotEqual(0f, fireRate,
            $"{gameObject.name}.{this.GetType()}: fireRate can not be zero");
        timeBetweenShots = 1f / fireRate;
    }

    private void Update() {
        Move(input);
        Fire(input);
    }

    public void OnInputFire(InputAction.CallbackContext context) {
        input.wasFirePressed = context.performed;
    }

    public void OnInputMove(InputAction.CallbackContext context) {
        Vector2 inputValue = context.ReadValue<Vector2>();

        input.forward = inputValue.y;
        input.turn = inputValue.x;
    }

    private void Move(GameInput input) {
        currentAcceleration = input.forward > 0 ? acceleration * input.forward : 0f;
        currentTurnRate = turnRate * -input.turn;

        float rotation = currentTurnRate * Time.deltaTime;
        transform.Rotate(0, 0, rotation, Space.Self);

        Vector2 velocityDelta = currentAcceleration * Time.deltaTime * (Vector2)transform.up - frictionRate * currentVelocity;
        currentVelocity += velocityDelta;

        if (currentVelocity.sqrMagnitude > maxSpeed * maxSpeed) {
            currentVelocity = maxSpeed * currentVelocity.normalized;
        }

        Vector2 positionDelta = Time.deltaTime * currentVelocity;
        transform.Translate(positionDelta.x, positionDelta.y, 0f, Space.World);
    }

    private void Fire(GameInput input) {
        if (!input.wasFirePressed) { return; }
        if ((Time.time - lastShotTime) < timeBetweenShots) { return; }
        lastShotTime = Time.time;

        var bullet = Instantiate(bulletPrefab);
        bullet.Initialize(transform.up);
    }
}
