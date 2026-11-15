using UnityEngine;

// Goes on an obstacle prefab root. Manages the RepairPickup children placed in hard-to-reach spots (top of a ramp,
// inside a gap). Every time the pool spawns the obstacle, it re-rolls which pickup (if any) is shown, so a box
// collected on the last pass comes back. Pickups are children, so they scroll with the obstacle's MoveWithWorld.
public class ObstaclePickups : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float chance = 0.3f; // chance that this spawn shows a pickup at all
    [SerializeField] int maxActive = 1;                  // how many of the child pickups can show at once

    RepairPickup[] pickups;

    void Awake()
    {
        pickups = GetComponentsInChildren<RepairPickup>(true); // include ones currently switched off
    }

    void OnEnable()
    {
        foreach (var p in pickups) p.gameObject.SetActive(false);
        if (pickups.Length == 0 || Random.value > chance) return;

        // switch on up to maxActive pickups, picked at random
        int toShow = Mathf.Min(maxActive, pickups.Length);
        int start = Random.Range(0, pickups.Length);
        for (int i = 0; i < toShow; i++)
            pickups[(start + i) % pickups.Length].gameObject.SetActive(true);
    }
}
