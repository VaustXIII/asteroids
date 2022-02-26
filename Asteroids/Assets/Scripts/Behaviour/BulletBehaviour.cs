using UnityEngine;


public class SelfDestructLogic {
    public event System.Action selfDestructed;

    private SelfDestructData data;

    private float timeLeft;

    public SelfDestructLogic(SelfDestructData data) {
        this.data = data;
    }

    public void Initialize() {
        timeLeft = data.lifeTime;
    }

    public void Tick(float dt) {
        timeLeft -= dt;

        if (timeLeft <= 0f) { SelfDestruct(); }
    }

    private void SelfDestruct() {
        selfDestructed?.Invoke();
    }
}

public class BulletBehaviour : Spawnable<Vector2> {

    [SerializeField] private AsteroidMovementData movementData;
    [SerializeField] private SelfDestructData selfDestructData;


    private LinearMovementLogic movementLogic;
    private SelfDestructLogic selfDestructLogic;

    private void Update() {
        transform.Translate(movementLogic.GetPositionDelta(Time.deltaTime), Space.World);
        selfDestructLogic.Tick(Time.deltaTime);
    }

    private void OnDestroy() {
        if (selfDestructLogic != null) {
            selfDestructLogic.selfDestructed -= OnSelfDestruct;
        }
    }

    public override void Initialize(Vector2 direction) {
        movementLogic = new LinearMovementLogic(movementData, direction);
        selfDestructLogic = new SelfDestructLogic(selfDestructData);

        selfDestructLogic.Initialize();
        selfDestructLogic.selfDestructed += OnSelfDestruct;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent<IShootable>(out var target)) {
            target.GetShot();
            OnSelfDestruct();
        }
    }

    private void OnSelfDestruct() {
        Destroy(gameObject);
    }
}
