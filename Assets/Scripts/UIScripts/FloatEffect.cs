using UnityEngine;
using UnityEngine.UI;

public class FloatEffect : MonoBehaviour
{
    public float amplitude = 10.0f; // How much it floats up and down
    public float frequency = 1.0f; // How fast it floats up and down

    private RectTransform rectTransform;
    private Vector3 startPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        rectTransform.anchoredPosition = new Vector2(startPos.x, newY);
    }
}
