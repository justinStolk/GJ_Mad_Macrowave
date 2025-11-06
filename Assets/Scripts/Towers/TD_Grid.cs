using UnityEngine;

public class TD_Grid : MonoBehaviour
{

    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private float cellSize;
    [SerializeField] private bool drawFromCenter;

    private TowerPoint[,] towerPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        CreateGrid(gridSize.x, gridSize.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsPositionOccupied(int x, int y)
    {
        return towerPoints[x, y].PointOccupied;
    }

    private void CreateGrid(int sizeX, int sizeY)
    {
        towerPoints = new TowerPoint[sizeX, sizeY];
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                towerPoints[x, y] = new TowerPoint();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.antiqueWhite;
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 offset = Vector3.zero;
                if (drawFromCenter)
                {
                    offset = new Vector3(gridSize.x, 0, gridSize.y) * 0.5f;
                }                
                Gizmos.DrawWireCube((new Vector3(x, 0, y) - offset) * cellSize , Vector3.one * cellSize);
            }
        }
    }
}
