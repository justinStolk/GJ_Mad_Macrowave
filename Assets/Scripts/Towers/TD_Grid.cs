using UnityEngine;

public class TD_Grid : MonoBehaviour
{

    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private float cellSize;
    [SerializeField] private bool drawFromCenter;
    [SerializeField] private bool drawUnselected;

    private TowerPoint[,] towerPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        CreateGrid(gridSize.x, gridSize.y);
    }

    public bool IsPositionOccupied(int x, int y)
    {
        return towerPoints[x, y].PointOccupied;
    }

    public bool OccupyTowerPoint(Tower tower, int x, int y)
    {
        if (towerPoints[x, y].PointOccupied)
        {
            return false;
        }
        towerPoints[x, y].SetTower(tower);
        return true;
    }

    public void SnapTowerPosition(Tower towerToSnap, Vector3 referencePosition)
    {
        int x = Mathf.RoundToInt(referencePosition.x);
        int z = Mathf.RoundToInt(referencePosition.z);

        Vector3 towerPosition = new Vector3(x, 0, z);
        towerToSnap.transform.position = towerPosition;
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x + (0.5f * gridSize.x));
        int y = Mathf.RoundToInt(worldPosition.z + (0.5f * gridSize.y));
        return new(x, y);
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
    private void OnDrawGizmos()
    {
        if (!drawUnselected) return;

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
                Gizmos.DrawWireCube(new Vector3(transform.position.x, 0, transform.position.z) +  (new Vector3(x, 0, y) - offset) * cellSize, Vector3.one * cellSize);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (drawUnselected) return;

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
                Gizmos.DrawWireCube(new Vector3(transform.position.x, 0, transform.position.z) + (new Vector3(x, 0, y) - offset) * cellSize , Vector3.one * cellSize);
            }
        }
    }
}
