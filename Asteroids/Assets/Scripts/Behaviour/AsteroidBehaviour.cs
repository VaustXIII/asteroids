using UnityEngine;

public class AsteroidBehaviour : Spawnable<Vector2>, IShootable, IScoreable  {
    public event System.Action<int> scored;

    [SerializeField] private int pointsForDestroying = 100;
    [SerializeField] private AsteroidMovementData movementData;

    [SerializeField] private AsteroidBreakdownData breakdownData;

    private LinearMovementLogic movementLogic;
    private AsteroidBreakDownLogic breakDownLogic;

    private void Awake() {
        this.breakDownLogic = new AsteroidBreakDownLogic(breakdownData);
    }

    private void Update() {
        transform.Translate(movementLogic.GetPositionDelta(Time.deltaTime), Space.World);
        transform.Rotate(movementLogic.GetRotationDelta(Time.deltaTime));
    }

    public override void Initialize(Vector2 direction) {
        movementLogic = new LinearMovementLogic(movementData, direction);
    }

    public void GetShot() {
        scored?.Invoke(pointsForDestroying);
        breakDownLogic?.BreakDownIntoDebris(Instantiate<Spawnable<Vector2>>, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
