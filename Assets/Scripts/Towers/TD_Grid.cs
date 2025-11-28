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

    private Dictionary<Vector2Int, Tower> towers;
    //private TowerPoint[,] towerPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        CreateGrid(gridSize.x, gridSize.y);
    }

    public bool IsPositionOccupied(int x, int y)
    {
        if(!towers.ContainsKey(new(x, y)))
        {
            return true;
        }
        Vector3 center = new Vector3(x, 0, y) * cellSize;
        Vector3 size = Vector3.one * cellSize;
        bool occupied = towers[new Vector2Int(x, y)] != null;
        bool floorIsFree = !Physics.CheckBox(center, size * 0.5f, Quaternion.identity, customColliderLayer);
        bool canPlaceTower = floorIsFree && !occupied;
        return !canPlaceTower;
    }

    public bool OccupyTowerPoint(Tower tower, Vector2Int position)
    {
        if (towers[position] != null)
        {
            return false;
        }
        towers[position] = tower;
        return true;
    }

    public bool RemoveTowerFromPoint(Vector2Int position)
    {
        if(towers[position] != null)
        {
            towers.Remove(position);
            return true;
        }
        return false;
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
        int x = Mathf.RoundToInt(worldPosition.x);
        int y = Mathf.RoundToInt(worldPosition.z);
        return new(x, y);
    }

    private void CreateGrid(int sizeX, int sizeY)
    {
        towers = new();
        //towerPoints = new TowerPoint[sizeX, sizeY];
        Vector2Int roundedPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Vector2Int pointAssociation = new Vector2Int(roundedPosition.x + x, roundedPosition.y + y);
                towers.Add(pointAssociation, null);
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
