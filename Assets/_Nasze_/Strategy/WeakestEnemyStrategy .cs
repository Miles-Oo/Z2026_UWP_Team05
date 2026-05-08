using System.Collections.Generic;
using UnityEngine;

public class WeakestEnemyStrategy : IBasicTargetStrategy
{
    public EnemyHp SelectTarget(List<EnemyHp> enemies, Transform towerTransform)
    {
        if (enemies == null || enemies.Count == 0)
            return null;

        EnemyHp weakest = null;
        float minHp = float.MaxValue;

        foreach (var e in enemies)
        {
            if (e == null) continue;

            float hp = e.GetCurrHp();

            if (hp < minHp)
            {
                minHp = hp;
                weakest = e;
            }
        }

        return weakest;
    }
}