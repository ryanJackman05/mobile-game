using UnityEngine;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] bool withConstraints;
    [Tooltip("Run over zombies that touch this trigger (kill, coins, slowdown). Turn off on the slim FrontCrash box so a zombie dead ahead isn't counted twice.")]
    [SerializeField] bool killsEnemies = true;
    [Tooltip("End the run when this trigger touches something solid. Turn off on the wide Front box so it only rams zombies.")]
    [SerializeField] bool crashes = true;
    [Tooltip("Only crash when this trigger touches the ground (e.g. the Top trigger: the car has flipped onto its roof). Obstacles are ignored.")]
    [SerializeField] bool groundOnly;

    private void OnTriggerEnter(Collider other)
    {
        // assume correct layer interaction exclusions - > check project config and gameobject layer
        if (other.CompareTag("Enemy")){
            if (!killsEnemies) return;
            other.GetComponent<EnemyController>().Kill();
            SendMessageUpwards("HitEnemy");
        }
        else if (crashes && !other.isTrigger && (!groundOnly || IsGround(other))){ // triggers (slow areas, game over zones) never crash
            var car = GetComponentInParent<PlayerCarController>();
            if (car != null) car.Crash(withConstraints, groundOnly ? GameManager.DeathCause.Flipped : GameManager.DeathCause.Crashed);
        }
    }

    // Ground is the "Ground" layer if it exists. Until then: any solid, static collider that isn't an obstacle (the road).
    static int groundLayer = -2; // -2 = not looked up yet
    static bool IsGround(Collider other)
    {
        if (other.isTrigger) return false;
        if (groundLayer == -2) groundLayer = LayerMask.NameToLayer("Ground");
        if (groundLayer >= 0) return other.gameObject.layer == groundLayer;
        return other.attachedRigidbody == null && !Obstacles.IsObstacle(other);
    }
}
