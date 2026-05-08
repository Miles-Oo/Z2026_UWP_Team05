using System.Collections.Generic;

public interface ITargetSelectionStrategy
{
    EnemyHp SelectTarget(List<EnemyHp> enemies);
}