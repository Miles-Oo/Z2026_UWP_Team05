using UnityEngine;

public class NextWavePresenter : MonoBehaviour
{
    [SerializeField] private WaveManager model;
    [SerializeField] private EnemyNextWaveView view;

    void Start()
    {
        model.OnWaveChanged += UpdatePreview;
        UpdatePreview();
    }

    void OnDestroy()
    {
        model.OnWaveChanged -= UpdatePreview;
    }

    void UpdatePreview()
    {
        var wave = model.GetNextWave();

        if (wave == null)
        {
            view.SetText("LAST WAVE", " ");
            return;
        }

        int total = 0;
        string types = "";

foreach (var e in wave.enemies)
{
    total += e.count;

    // var data = e.enemyPrefab.GetComponent<EnemyData>();
    GameObject prefab = EnemyFactory.GetPrefab(e.enemyType);
    EnemyData data = prefab.GetComponent<EnemyData>();

    string name = data != null ? data.Name : e.enemyType.ToString();

    types += $"{name} x{e.count}\n";
}

        view.SetText(
            $"Next wave enemies: {total}",
            types
        );
    }
}