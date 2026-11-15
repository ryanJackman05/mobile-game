using UnityEngine;

// Shared "is this an obstacle?" check for zombie scraping and car scrape penalties.
// Obstacles live on the "Obstacle" layer. Until that layer exists, anything solid that scrolls with the world counts.
public static class Obstacles
{
    static int layer = -2; // -2 = not looked up yet

    static int Layer
    {
        get
        {
            if (layer == -2)
            {
                layer = LayerMask.NameToLayer("Obstacle");
                if (layer < 0) Debug.LogWarning("[Obstacles] No 'Obstacle' layer, falling back to MoveWithWorld check");
            }
            return layer;
        }
    }

    public static bool IsObstacle(Collider other)
    {
        if (other.isTrigger) return false; // slow areas and the car's own trigger zones aren't solid
        if (Layer >= 0) return other.gameObject.layer == Layer;
        return other.GetComponentInParent<MoveWithWorld>() != null
               && other.GetComponentInParent<EnemyController>() == null
               && !other.transform.root.CompareTag("Player");
    }
}
