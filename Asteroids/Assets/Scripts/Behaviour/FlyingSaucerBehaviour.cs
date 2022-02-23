using UnityEngine;

public class FlyingSaucerBehaviour : Spawnable<Transform>, IShootable, IScoreable {
    public event System.Action<int> scored;
    [SerializeField] private FlyingSaucerMovementData movementData;
    [SerializeField] private int pointsForDestroying = 300;

    private FlyingSaucerMovementLogic movementLogic;

    private void Update() {
        transform.Translate(movementLogic.GetPositionDelta(transform.position, Time.deltaTime));
    }

    public override void Initialize(Transform target) {
        movementLogic = new FlyingSaucerMovementLogic(movementData, target);
    }

    public void GetShot() {
        scored?.Invoke(pointsForDestroying);
        Destroy(gameObject);
    }
}
