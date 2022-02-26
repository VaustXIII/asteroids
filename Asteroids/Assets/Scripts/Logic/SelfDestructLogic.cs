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
