using UnityEngine;

public class LaserBehaviour : MonoBehaviour {
    [SerializeField] private LaserData laserData;

    private LaserLogic logic;

    private Transform FirePoint => transform.parent;

    public int CurrentChargesCount => logic.CurrentChargesCount;
    public float CurrentChargeCooldown => logic.CurrentChargeCooldown;

    private void Awake() {
        var sprite = this.GetRequiredComponentInChildren<SpriteRenderer>();
        var collider = this.GetRequiredComponent<BoxCollider2D>();

        transform.localScale = new Vector3(laserData.beamWidth, laserData.beamLength, 1f);
        transform.position = FirePoint.position + 0.5f * laserData.beamLength * FirePoint.up;

        logic = new LaserLogic(laserData, this, FirePoint, sprite, collider);
    }

    private void Update() {
        logic.Tick(Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent<IShootable>(out var target)) {
            target.GetShot();
        }
    }

    public void Shoot() {
        logic.Shoot();
    }
}
