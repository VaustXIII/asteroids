using UnityEngine;
using UnityEngine.Assertions;

public static class Utils {
    public static Vector3 GetRandomPointOutsideOfCameraView(Camera camera) {
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
}


// Вот тут прям совсем запутанно получилось, не додумался как красиво спавнить и иницииализировать объекты разных типов, не стал даже в отдельный файлы выносить
// Буду признателен за подсказки/мысли на этот счёт :)
public abstract class SpawnerLogic<PrefabType, SpawnableInitType> where PrefabType : Spawnable<SpawnableInitType>, IScoreable {
    [System.Serializable]
    public class SpawnData {
        public PrefabType prefab;
        public float timeBetweenSpawns;
    }

    public event System.Action<IScoreable> spawned;

    [SerializeField] private SpawnData data;

    private System.Func<PrefabType, Vector3, Quaternion, PrefabType> instatiateFunc;
    private float timeTillNextSpawn;

    public SpawnerLogic(
        SpawnData data,
        System.Func<PrefabType, Vector3, Quaternion, PrefabType> instantiateFunc
    ) {
        this.data = data;
        this.instatiateFunc = instantiateFunc;

        timeTillNextSpawn = data.timeBetweenSpawns;
    }

    protected abstract void InitializeSpawnedObject(PrefabType spawnedObject, SpawnableInitType initData);

    public void Tick(float dt, Camera camera, SpawnableInitType initData) {
        timeTillNextSpawn -= dt;
        if (timeTillNextSpawn <= 0f) {
            timeTillNextSpawn += data.timeBetweenSpawns;
            Spawn(camera, initData);
        }
    }

    private void Spawn(Camera camera, SpawnableInitType initData) {
        var pointOutsideOfCameraView = Utils.GetRandomPointOutsideOfCameraView(camera);

        var spawnedObject = instatiateFunc(data.prefab, pointOutsideOfCameraView, Quaternion.identity);
        InitializeSpawnedObject(spawnedObject, initData);
        spawned?.Invoke(spawnedObject);
    }
}

public class AsteroidSpawnerLogic<AsteroidPrefabType> : SpawnerLogic<AsteroidPrefabType, Vector2> // Ну это конечно хех, мда
        where AsteroidPrefabType : Spawnable<Vector2>, IScoreable {
    public AsteroidSpawnerLogic(SpawnData data, System.Func<AsteroidPrefabType, Vector3, Quaternion, AsteroidPrefabType> instantiateFunc)
        : base(data, instantiateFunc) {
    }

    protected override void InitializeSpawnedObject(AsteroidPrefabType spawnedObject, Vector2 initData) { // Тут initData игнорируется, что конечно же некраисво
        Vector3 targetPoint = 0.75f * spawnedObject.transform.position.y * Random.onUnitSphere;  // точка в районе центра экрана
        targetPoint.z = 0;
        Vector3 initDirection = targetPoint - spawnedObject.transform.position;  // запустить куда-то в район центра экрана
        spawnedObject.Initialize(initDirection);
    }
}

public class FlyingSaucerSpawnerLogic<FlyingSaucerPrefabType> : SpawnerLogic<FlyingSaucerPrefabType, Transform>
        where FlyingSaucerPrefabType : Spawnable<Transform>, IScoreable {
    public FlyingSaucerSpawnerLogic(SpawnData data, System.Func<FlyingSaucerPrefabType, Vector3, Quaternion, FlyingSaucerPrefabType> instantiateFunc)
        : base(data, instantiateFunc) {
    }

    protected override void InitializeSpawnedObject(FlyingSaucerPrefabType spawnedObject, Transform initData) {
        spawnedObject.Initialize(initData);
    }
}

public class EnemiesSpawnerBehaviour : MonoBehaviour {
    public event System.Action<IScoreable> spawned;

    [SerializeField] private Transform targetPlayer;

    [SerializeField] private AsteroidSpawnerLogic<AsteroidBehaviour>.SpawnData asteroidSpawnerData;
    [SerializeField] private FlyingSaucerSpawnerLogic<FlyingSaucerBehaviour>.SpawnData flyingSaucerSpawnerData;

    [Header("Debug")]
    [SerializeField] private bool drawDebugLines;

    private Camera mainCamera;


    private AsteroidSpawnerLogic<AsteroidBehaviour> asteroidSpawnerLogic;
    private FlyingSaucerSpawnerLogic<FlyingSaucerBehaviour> flyingSaucerSpawnerLogic;

    private void Awake() {
        mainCamera = Camera.main;

        asteroidSpawnerLogic = new AsteroidSpawnerLogic<AsteroidBehaviour>(asteroidSpawnerData, Instantiate<AsteroidBehaviour>);
        asteroidSpawnerLogic.spawned += OnLogicSpawend;

        flyingSaucerSpawnerLogic = new FlyingSaucerSpawnerLogic<FlyingSaucerBehaviour>(flyingSaucerSpawnerData, Instantiate<FlyingSaucerBehaviour>);
        flyingSaucerSpawnerLogic.spawned += OnLogicSpawend;
    }

    private void Update() {
        asteroidSpawnerLogic.Tick(Time.deltaTime, mainCamera, Vector2.zero); // Vector2.zero игнорируется, что конечно же некраисво
        flyingSaucerSpawnerLogic.Tick(Time.deltaTime, mainCamera, targetPlayer);
    }

    private void OnLogicSpawend(IScoreable scoreable) {
        spawned?.Invoke(scoreable);
    }

}
