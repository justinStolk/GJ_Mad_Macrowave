using UnityEngine;

public class TowerRangeRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer radiusRenderer;

    [Range(8, 24)]
    [SerializeField] private int lineCount;
    [SerializeField] private float width;

    void Start()
    {
        radiusRenderer.positionCount = lineCount;
        radiusRenderer.startWidth = width;
        radiusRenderer.loop = true;
        StopRender();
    }

    public void RenderRadius(float range, Vector3 position)
    {
        radiusRenderer.enabled = true;

        float theta = (2f * Mathf.PI) / lineCount;  //find radians per segment
        float angle = 0;

        for (int i = 0; i < lineCount; i++)
        {
            float x = range * Mathf.Cos(angle);
            float y = range * Mathf.Sin(angle);
            radiusRenderer.SetPosition(i, new Vector3(x, 0.1f, y) + position);
            //switch 0 and y for 2D games
            angle += theta;
        }
    }

    public void StopRender()
    {
        radiusRenderer.enabled = false;
    }

}
