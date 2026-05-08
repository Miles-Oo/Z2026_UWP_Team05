using System.Collections.Generic;
using UnityEngine;

public interface IBasicTargetStrategy
{
    EnemyHp SelectTarget(List<EnemyHp> enemies, Transform towerTransform);
}