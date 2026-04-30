using UnityEngine;

public class ObserverEnemyAudio : MonoBehaviour
{
    public static ObserverEnemyAudio Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Register(EnemyHp enemy)
    {
        enemy.OnEnemyDeath += HandleEnemyDeath;
    }

    public void Unregister(EnemyHp enemy)
    {
        enemy.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath()
    {
        AudioManager.Instance?.PlaySfx(AudioCue.EnemyDeath);
    }
}