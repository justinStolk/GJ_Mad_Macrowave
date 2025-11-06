using System.Collections.Generic;
using UnityEngine;

public class MultiTargetTower : Tower
{
    protected override bool _hasTarget => throw new System.NotImplementedException();

    [SerializeField, Min(2)] private ushort maximumSimultaneousTargets = 2;

    protected override void AttackTarget()
    {
        base.AttackTarget();
        throw new System.NotImplementedException();
    }

    protected override void FindTarget()
    {
        throw new System.NotImplementedException();
    }
}
