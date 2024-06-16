using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public GameObject flashlight;
    private Rigidbody2D rb;
    public float speed = 3f;
    private bool isInFlashlight = false;
    private Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveDirection = (player.position - transform.position).normalized;
    }

    void Update()
    {
        // Update move direction towards the player
        moveDirection = (player.position - transform.position).normalized;

        // Check if the path towards the player is blocked
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, Mathf.Infinity, LayerMask.GetMask("Obstacle"));
        if (hit.collider != null)
        {
            // Path is obstructed, find an alternative direction
            Vector2 obstacleNormal = hit.normal;
            moveDirection = Vector2.Reflect(moveDirection, obstacleNormal).normalized;
        }

        // Chase the player only if not in the flashlight cone
        if (!isInFlashlight)
        {
            rb.velocity = moveDirection * speed;
        }
        else
        {
            // Stop moving if within the flashlight cone
            rb.velocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == flashlight)
        {
            isInFlashlight = true;
            Debug.Log("Enemy entered flashlight cone");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == flashlight)
        {
            isInFlashlight = false;
            Debug.Log("Enemy exited flashlight cone");
        }
    }
}
