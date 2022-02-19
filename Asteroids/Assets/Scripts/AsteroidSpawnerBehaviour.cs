using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawnerBehaviour : MonoBehaviour {
    [SerializeField] private AsteroidBehaviour asteroidPrefab;
    [SerializeField] private float timeBetweenSpawns = 3f;

    [Header("Debug")]
    [SerializeField] private bool drawDebugLines;

    private Camera mainCamera;
    private float lastSpawnTime;

    private void Start() {
        mainCamera = Camera.main;
        // lastSpawnTime = -timeBetweenSpawns;
    }

    private void Update() {
        Spawn();
    }

    private void Spawn() {
        if ((Time.time - lastSpawnTime) < timeBetweenSpawns) { return; }
        lastSpawnTime = Time.time;

        bool isVertical = RandomExt.Bool(); // Верх-низ или право-лево?
        bool isPositive = RandomExt.Bool();
        Vector2 randomDirection;
        if (isVertical) { // Задаём точку на краю Viewport'а
            randomDirection.x = Random.Range(0f, 1f);
            randomDirection.y = isPositive ? 1.1f : 1.1f; // 1.1f чтобы чуть за экраном появлялись
        }
        else {
            randomDirection.y = Random.Range(0f, 1f);
            randomDirection.x = isPositive ? 1.1f : 1.1f;
        }
        var pointOutsideOfCameraView = Camera.main.ViewportToWorldPoint(new Vector3(randomDirection.x, randomDirection.y, 0f));
        pointOutsideOfCameraView.z = 0f;

        var asteroid = Instantiate(asteroidPrefab, pointOutsideOfCameraView, Quaternion.identity);

        Vector3 targetPoint = 0.75f * pointOutsideOfCameraView.y * Random.onUnitSphere;  // точка в районе центра экрана
        targetPoint.z = 0;
        Vector3 initDirection = targetPoint - pointOutsideOfCameraView;  // запустить куда-то в район центра экрана
        if (drawDebugLines) {
            Debug.DrawLine(Vector3.zero, pointOutsideOfCameraView, Color.red, timeBetweenSpawns);
            Debug.DrawLine(Vector3.zero, targetPoint, Color.blue, timeBetweenSpawns);
            Debug.DrawLine(pointOutsideOfCameraView, targetPoint, Color.white, timeBetweenSpawns);
            Debug.DrawLine(Vector3.zero, initDirection, Color.white, timeBetweenSpawns);
        }

        asteroid.Initialize(initDirection);
    }

}
