using System.Collections.Generic;
using UnityEngine;

public class TD_Grid : MonoBehaviour
{

    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private float cellSize;
    [SerializeField] private bool drawFromCenter;
    [SerializeField] private bool drawUnselected;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private LayerMask customColliderLayer;

    [SerializeField] private TowerRangeRenderer tRenderer;

    private Dictionary<Vector2Int, GridObject> gridObjects;
    //private TowerPoint[,] towerPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        CreateGrid(gridSize.x, gridSize.y);
    }

    public bool IsPositionOccupied(int x, int y)
    {
        if(!gridObjects.ContainsKey(new(x, y)))
        {
            return true;
        }
        Vector3 center = new Vector3(x, 0, y) * cellSize;
        Vector3 size = Vector3.one * cellSize;
        bool occupied = gridObjects[new Vector2Int(x, y)] != null;
        bool floorIsFree = !Physics.CheckBox(center, size * 0.5f, Quaternion.identity, customColliderLayer);
        bool canPlaceTower = floorIsFree && !occupied;
        return !canPlaceTower;
    }

    public bool OccupyTowerPoint(GridObject gridObject, Vector2Int position)
    {
        if (gridObjects[position] != null)
        {
            return false;
        }
        gridObjects[position] = gridObject;
        return true;
    }

    public bool RemoveTowerFromPoint(Vector2Int position)
    {
        if(gridObjects[position] != null)
        {
            gridObjects.Remove(position);
            return true;
        }
        return false;
    }

    public void SnapTowerPosition(GridObject gridObjectToSnap, Vector3 referencePosition)
    {
        int x = Mathf.RoundToInt(referencePosition.x);
        int z = Mathf.RoundToInt(referencePosition.z);

        Vector3 towerPosition = new Vector3(x, 0, z);
        gridObjectToSnap.transform.position = towerPosition;
        if(gridObjectToSnap is Tower tower)
        {
            tRenderer.RenderRadius(tower.Range.y, towerPosition);
        }
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x);
        int y = Mathf.RoundToInt(worldPosition.z);
        return new(x, y);
    }

    private void CreateGrid(int sizeX, int sizeY)
    {
        gridObjects = new();
        //towerPoints = new TowerPoint[sizeX, sizeY];
        Vector2Int roundedPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Vector2Int pointAssociation = new Vector2Int(roundedPosition.x + x, roundedPosition.y + y);
                gridObjects.Add(pointAssociation, null);
                //towerPoints[x, y] = new TowerPoint();
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
                Vector3 center = new Vector3(transform.position.x, 0, transform.position.z) + new Vector3(x, 0, y) * cellSize;
                Vector3 size = Vector3.one * cellSize;
                if (Physics.CheckBox(center, size * 0.5f, Quaternion.identity, ~floorLayer))
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.antiqueWhite;
                }
                Gizmos.DrawWireCube(center, size);
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
                Vector3 center = new Vector3(transform.position.x, 0, transform.position.z) + new Vector3(x, 0, y) * cellSize;
                Vector3 size = Vector3.one * cellSize;
                if (Physics.CheckBox(center, size * 0.5f, Quaternion.identity, ~floorLayer))
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.antiqueWhite;

                }
                Gizmos.DrawWireCube(center, size);
            }
        }
    }
}
