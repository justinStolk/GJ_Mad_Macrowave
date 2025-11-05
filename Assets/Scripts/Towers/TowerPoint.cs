using UnityEngine;

public class TowerPoint
{
    public bool PointOccupied => tower != null;

    private Tower tower;

    public bool SetTower(Tower newTower)
    {
        if(tower != null)
        {
            return false;
        }
        tower = newTower;
        return true;
    }

    public bool RemoveTower()
    {
        if(tower != null)
        {
            // It might be necessary to retrieve the tower instead, so that we could get a refund as well.
            Object.Destroy(tower);
            return true;
        }
        return false;
    }
}
