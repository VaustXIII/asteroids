using UnityEngine;

public class LaserLogic {
    private LaserData data;
    private MonoBehaviour laserBehaviour;  // feels icky to have it for only Invoke
    private SpriteRenderer sprite;  // this and collider in logic layer also feels icky
    private BoxCollider2D collider;
    private Transform firePoint;

    private int currentChargesCount;
    private float currentChargeCooldown;
    private bool isActive = false;

    public int CurrentChargesCount => currentChargesCount;
    public float CurrentChargeCooldown => currentChargeCooldown;

    public LaserLogic(LaserData data, MonoBehaviour laserBehaviour, Transform firePoint, SpriteRenderer sprite, BoxCollider2D collider) {
        this.data = data;
        this.laserBehaviour = laserBehaviour;
        this.firePoint = firePoint;
        this.sprite = sprite;
        this.collider = collider;

        currentChargesCount = data.maxChargesCount;

        Deactivate();
    }

    public void Tick(float dt) {
        Cooldown(dt);
    }

    public void Shoot() {
        if (currentChargesCount <= 0) { return; }
        if (isActive) { return; }
        currentChargesCount--;
        Activate();
        laserBehaviour.Invoke(Deactivate, data.activeDuration);
    }

    private void Cooldown(float dt) {
        if (currentChargesCount >= data.maxChargesCount) {
            currentChargeCooldown = data.chargeCooldown;
            return;
        }

        currentChargeCooldown -= dt;
        if (currentChargeCooldown <= 0) {
            currentChargesCount++;
            currentChargeCooldown += data.chargeCooldown;  // += чтобы перенести остаток в следующий кадр
        }
    }

    private void Activate() {
        isActive = true;
        sprite.enabled = true;
        collider.enabled = true;
    }

    private void Deactivate() {
        isActive = false;
        sprite.enabled = false;
        collider.enabled = false;
    }
}
