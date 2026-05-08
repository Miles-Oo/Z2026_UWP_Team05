using System.Collections.Generic;
using UnityEngine;

public class NearestEnemyStrategy : IBasicTargetStrategy
{
    public EnemyHp SelectTarget(List<EnemyHp> enemies, Transform towerTransform)
    {
        if (enemies == null || enemies.Count == 0)
            return null;

        EnemyHp nearest = null;
        float minDist = float.MaxValue;

        foreach (var e in enemies)
        {
            if (e == null) continue;

            float dist = (e.transform.position - towerTransform.position).sqrMagnitude;

            if (dist < minDist)
            {
                minDist = dist;
                nearest = e;
            }
        }

        return nearest;
    }
}