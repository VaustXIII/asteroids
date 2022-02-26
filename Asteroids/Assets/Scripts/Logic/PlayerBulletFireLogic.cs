using UnityEngine;

public class PlayerBulletFireLogic {
    private Spawnable<Vector2> bulletPrefab;
    private Transform firePoint;

    private float timeBetweenShots;

    private float lastShotTime;

    public PlayerBulletFireLogic(float fireRate, Spawnable<Vector2> bulletPrefab, Transform firePoint) {
        if (fireRate == 0f) {
            timeBetweenShots = float.PositiveInfinity;
        }
        timeBetweenShots = 1f / fireRate;
        this.bulletPrefab = bulletPrefab;
        this.firePoint = firePoint;
    }

    public void Fire(GameInput input,
        float currentTime,
        Vector3 currentUp,
        System.Func<Spawnable<Vector2>, Vector3, Quaternion, Spawnable<Vector2>> instatiateFunction
    ) {
        if (!input.wasFirePressed) { return; }
        if ((currentTime - lastShotTime) < timeBetweenShots) { return; }
        lastShotTime = currentTime;

        var bullet = instatiateFunction(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.Initialize(currentUp);
    }
}
