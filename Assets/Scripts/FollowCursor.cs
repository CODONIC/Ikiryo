using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    void Update()
    {
        // Get the mouse position in screen space and convert it to world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the flashlight to the mouse position
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the rotation of the flashlight
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
