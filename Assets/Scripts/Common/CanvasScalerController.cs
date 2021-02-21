using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerController : MonoBehaviour
{
    [SerializeField]
    private float width = 1080;
    [SerializeField]
    private float height = 1920;
    [SerializeField]
    private bool isMatchWidth = true;

    public float Width { get { return width; } }
    public float Height { get { return height; } }

    private void Awake()
    {
        var scaler = GetComponent<CanvasScaler>();

        if (isMatchWidth)
        {
            if ((float) Screen.height / (float) Screen.width > height / width)
            {
                scaler.matchWidthOrHeight = 0;
            }
        }
        else
        {
            if ((float) Screen.height / (float) Screen.width < height / width)
            {
                scaler.matchWidthOrHeight = 1;
            }
        }
    }
}
