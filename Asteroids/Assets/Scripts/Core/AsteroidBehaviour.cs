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

public class AsteroidBreakDownLogic {
    private AsteroidBreakdownData data;

    public AsteroidBreakDownLogic(AsteroidBreakdownData data) {
        this.data = data;
    }

    public void BreakDownIntoDebris(
        System.Func<AsteroidBehaviour, Vector3, Quaternion, AsteroidBehaviour> instatiateFunction,
        Vector3 position,
        Quaternion rotation
    ) {
        if (data != null) {
            for (int childIndex = 0; childIndex < data.childrenCount; childIndex++) {
                var child = instatiateFunction(data.childAsteroidPrefab, position, rotation);
                child.Initialize(Random.onUnitSphere);
            }
        }
    }
}

public class AsteroidBehaviour : MonoBehaviour, IShootable, IScoreable {
    public event System.Action<int> scored;

    [SerializeField] private int pointsForDestroying = 100;
    [SerializeField] private AsteroidMovementData movementData;

    [SerializeField] private AsteroidBreakdownData breakdownData;

    private AsteroidMovementLogic movementLogic;
    private AsteroidBreakDownLogic breakDownLogic;

    private void Awake() {
        this.breakDownLogic = new AsteroidBreakDownLogic(breakdownData);
    }

    private void Update() {
        transform.Translate(movementLogic.GetPositionDelta(Time.deltaTime), Space.World);
        transform.Rotate(movementLogic.GetRotationDelta(Time.deltaTime));
    }

    public void Initialize(Vector2 direction) {
        movementLogic = new AsteroidMovementLogic(movementData, direction);
    }

    public void GetShot() {
        scored?.Invoke(pointsForDestroying);
        breakDownLogic?.BreakDownIntoDebris(Instantiate<AsteroidBehaviour>, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
