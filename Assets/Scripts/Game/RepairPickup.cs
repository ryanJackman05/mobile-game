using UnityEngine;

// Repair box: restores car health when the car drives through it.
// Lives as a child of an obstacle prefab (it scrolls with the obstacle, so no MoveWithWorld of its own);
// ObstaclePickups on the obstacle root decides on each spawn whether it shows.
// Prefab setup: any model + a Collider (set to trigger here) + this script.
public class RepairPickup : MonoBehaviour
{
    [SerializeField] int healAmount = 30;
    [SerializeField] Transform visual;          // optional child to spin/bob, so the collider itself stays put
    [SerializeField] float spinDegPerSec = 90f;
    [SerializeField] float bobHeight = 0.15f;
    [SerializeField] float bobSpeed = 3f;

    Vector3 visualBasePos;

    void Awake()
    {
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
        if (visual != null) visualBasePos = visual.localPosition;
    }

    void Update()
    {
        if (visual == null || LifecycleGuard.IsPaused) return;
        visual.Rotate(0f, spinDegPerSec * Time.deltaTime, 0f, Space.World);
        visual.localPosition = visualBasePos + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
    }

    void OnTriggerEnter(Collider other)
    {
        var body = other.attachedRigidbody;
        if (body == null) return;
        if (other.GetComponentInParent<EnemyController>() != null) return; // a zombie hanging on the car is part of its Rigidbody
        var car = body.GetComponent<PlayerCarController>();
        if (car == null) return;

        car.Repair(healAmount);
        Haptics.Pulse();
        AudioManager.Play(Sfx.RepairPickup);
        gameObject.SetActive(false); // ObstaclePickups re-rolls it next time the obstacle spawns
    }
}
