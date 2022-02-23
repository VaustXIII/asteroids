using UnityEngine;
using UnityEngine.Assertions;

public class EnemiesSpawnerBehaviour : MonoBehaviour {
    public event System.Action<IScoreable> spawned;

    [SerializeField] private AsteroidBehaviour asteroidPrefab;
    [SerializeField] private FlyingSaucerBehaviour flyingSaucerPrefab;
    [SerializeField] private float timeBetweenAsteroidSpawns = 3f;
    [SerializeField] private float timeBetweenFlyingSaucerSpawns = 10f;
    [SerializeField] private Transform targetPlayer;


    [Header("Debug")]
    [SerializeField] private bool drawDebugLines;

    private Camera mainCamera;

    private float lastAsteroidSpawnTime;
    private float lastFlyingSaucerSpawnTime;

    private void Awake() {
        mainCamera = Camera.main;

        Assert.IsNotNull(asteroidPrefab, "Spawner needs asteroid prefab");
        Assert.IsNotNull(flyingSaucerPrefab, "Spawner needs flying saucer prefab");
        Assert.IsNotNull(targetPlayer, "Spawner needs a player to target");
    }

    private void Update() {
        SpawnAsteroid();
        SpawnFlyingSaucer();
    }

    private Vector3 GetRandomPointOutsideOfCameraView(Camera camera) {
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
        var result = camera.ViewportToWorldPoint(new Vector3(randomDirection.x, randomDirection.y, 0f));
        result.z = 0f;

        return result;
    }

    private void SpawnAsteroid() {
        if ((Time.time - lastAsteroidSpawnTime) < timeBetweenAsteroidSpawns) { return; }
        lastAsteroidSpawnTime = Time.time;

        var pointOutsideOfCameraView = GetRandomPointOutsideOfCameraView(mainCamera);

        var asteroid = Instantiate(asteroidPrefab, pointOutsideOfCameraView, Quaternion.identity);

        Vector3 targetPoint = 0.75f * pointOutsideOfCameraView.y * Random.onUnitSphere;  // точка в районе центра экрана
        targetPoint.z = 0;
        Vector3 initDirection = targetPoint - pointOutsideOfCameraView;  // запустить куда-то в район центра экрана
        if (drawDebugLines) {  // only for debug
            Debug.DrawLine(Vector3.zero, pointOutsideOfCameraView, Color.red, timeBetweenAsteroidSpawns);
            Debug.DrawLine(Vector3.zero, targetPoint, Color.blue, timeBetweenAsteroidSpawns);
            Debug.DrawLine(pointOutsideOfCameraView, targetPoint, Color.white, timeBetweenAsteroidSpawns);
            Debug.DrawLine(Vector3.zero, initDirection, Color.white, timeBetweenAsteroidSpawns);
        }

        asteroid.Initialize(initDirection);

        spawned?.Invoke(asteroid);
    }

    private void SpawnFlyingSaucer() {
        if ((Time.time - lastFlyingSaucerSpawnTime) < timeBetweenFlyingSaucerSpawns) { return; }
        lastFlyingSaucerSpawnTime = Time.time;

        var pointOutsideOfCameraView = GetRandomPointOutsideOfCameraView(mainCamera);

        var flyingSaucer = Instantiate(flyingSaucerPrefab, pointOutsideOfCameraView, Quaternion.identity);
        flyingSaucer.Initialize(targetPlayer);

        spawned?.Invoke(flyingSaucer);
    }

}
