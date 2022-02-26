using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class PlayerShipBehaviour : MonoBehaviour {
    public event System.Action crashed;

    [SerializeField] private PlayerMovementData movementData;


    [Header("Combat")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private BulletBehaviour bulletPrefab;

    [SerializeField] private LaserBehaviour laserPrefab;


    [Range(0.1f, 10f)]
    [SerializeField] private float fireRate = 2;

    [Header("Debug")]
    [SerializeField] private bool debugIsInvincible;

    private PlayerMovemenLogic movementLogic;
    private PlayerBulletFireLogic bulletFireLogic;

    private LaserBehaviour laser;


    private GameInput input = new GameInput();
    private PlayerState state = new PlayerState();

    private void Awake() {
        Assert.IsNotNull(bulletPrefab, $"{gameObject.name}.{this.GetType()}: need a bullet prefab");
        Assert.AreNotEqual(0f, fireRate,
            $"{gameObject.name}.{this.GetType()}: fireRate can not be zero");


        movementLogic = new PlayerMovemenLogic(movementData);
        bulletFireLogic = new PlayerBulletFireLogic(fireRate, bulletPrefab, firePoint);
    }

    private void Start() {
        laser = Instantiate(laserPrefab, parent: firePoint);
    }

    private void Update() {
        transform.Rotate(0f, 0f, movementLogic.GetRotationDelta(input, Time.deltaTime), Space.Self);
        transform.Translate(movementLogic.GetPositionDelta(transform.up, input, Time.deltaTime), Space.World);

        bulletFireLogic.Fire(input, Time.time, transform.up, Instantiate<Spawnable<Vector2>>);
        FireLaser(input);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (debugIsInvincible) { return; }
        crashed?.Invoke();
    }

    public void OnInputFire(InputAction.CallbackContext context) {
        input.wasFirePressed = context.performed;
    }

    public void OnInputFireLaser(InputAction.CallbackContext context) {
        input.wasFireLaserPressed = context.performed;
    }

    public void OnInputMove(InputAction.CallbackContext context) {
        Vector2 inputValue = context.ReadValue<Vector2>();

        input.forward = inputValue.y;
        input.turn = inputValue.x;
    }

    public PlayerState GetState() {
        state.position = transform.position;
        state.rotationAngle = transform.rotation.eulerAngles.z;
        state.velocity = movementLogic.CurrentVelocity;
        state.laserChargesCount = laser.CurrentChargesCount;
        state.laserChargeCooldown = laser.CurrentChargeCooldown;
        return state;
    }

    private void FireLaser(GameInput input) {
        if (!input.wasFireLaserPressed) { return; }
        laser.Shoot();
    }

    private void OnDrawGizmosSelected() {
        if (firePoint != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(firePoint.position, 0.5f);
        }
    }
}
