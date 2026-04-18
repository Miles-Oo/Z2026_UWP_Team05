using UnityEngine;

public class EnemyCountPresenter : MonoBehaviour
{
    [SerializeField] private WaveManager model;
    [SerializeField] private UiEnemyCountView view;

    void Start()
    {
        model.OnEnemyCountChanged += UpdateEnemies;
        UpdateEnemies();
    }

    void OnDestroy()
    {
        model.OnEnemyCountChanged -= UpdateEnemies;
    }

    void UpdateEnemies()
    {
        view.SetEnemies(model.aliveEnemies, model.totalEnemiesInWave);
    }
}