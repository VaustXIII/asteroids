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

public class AsteroidBehaviour : MonoBehaviour, IShootable, IScoreable {
    public event System.Action<int> scored;

    [SerializeField] private int pointsForDestroying = 100;
    [SerializeField] private AsteroidMovementData movementData;

    [Header("Breaking into smaller parts")]
    [SerializeField] private AsteroidBehaviour childAsteroidPrefab;
    [Min(1)]
    [SerializeField] private int childrenCount;

    private AsteroidMovementLogic movementLogic;


    private void Update() {
        if (movementLogic == null) {
            return;
        }
        transform.Translate(movementLogic.GetPositionDelta(Time.deltaTime), Space.World);
        transform.Rotate(movementLogic.GetRotationDelta(Time.deltaTime));
    }

    public void Initialize(Vector2 direction) {
        movementLogic = new AsteroidMovementLogic(movementData, direction);
    }

    public void GetShot() {
        scored?.Invoke(pointsForDestroying);
        BreakDownIntoDebris();
    }

    private void BreakDownIntoDebris() {
        if (childAsteroidPrefab != null) {
            for (int childIndex = 0; childIndex < childrenCount; childIndex++) {
                var child = Instantiate(childAsteroidPrefab, transform.position, transform.rotation);
                child.Initialize(Random.onUnitSphere);
            }
        }
        Destroy(gameObject);
    }
}
