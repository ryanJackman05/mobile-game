using UnityEngine;

// Slows the player car once when it drives over this trigger (e.g. hazardL/hazardR on the ramp & container prefab).
public class SlowArea : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float speedPenalty = 0.25f; // share of current speed lost

    bool used; // the car has several colliders; only the first one to enter counts

    void OnEnable() => used = false; // pooled with its obstacle, so re-arm on every spawn

    void OnTriggerEnter(Collider other)
    {
        if (used || other.attachedRigidbody == null) return;
        if (other.attachedRigidbody.GetComponent<PlayerCarController>() == null) return;
        used = true;
        GameManager.current.ApplySpeedPenalty(speedPenalty);
        AudioManager.Play(Sfx.SlowArea);
    }
}
