using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public GameObject flashlight;
    private Rigidbody2D rb;
    public float speed = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Call IsInFlashlightCone and log the result
        bool isInFlashlightCone = IsInFlashlightCone();
        Debug.Log("Is in flashlight cone: " + isInFlashlightCone);

        // Chase the player only if not in the flashlight cone
        if (!isInFlashlightCone)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        else
        {
            // Stop moving if within the flashlight cone
            rb.velocity = Vector2.zero;
        }
    }

    bool IsInFlashlightCone()
    {
        // Vector from flashlight to enemy
        Vector2 toEnemy = (Vector2)transform.position - (Vector2)flashlight.transform.position;
        // Angle between flashlight direction and direction to enemy
        float angle = Vector2.Angle(flashlight.transform.right, toEnemy);

        // Get the Light2D component
        var light2D = flashlight.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        float outerAngle = light2D.pointLightOuterAngle / 2f;
        float outerRadius = light2D.pointLightOuterRadius;

        Debug.Log($"Angle to enemy: {angle}, Light outer angle: {outerAngle}, Distance to enemy: {toEnemy.magnitude}, Light outer radius: {outerRadius}");

        // Check if the enemy is within the light cone
        if (angle < outerAngle && toEnemy.magnitude < outerRadius)
        {
            Debug.Log("Enemy is within angle and radius.");
            return true;
        }
        return false;
    }
}
