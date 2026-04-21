using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerUpgradeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private RawImage upgradeImage;
    [SerializeField] private Button upgradeButton;

    private TowerPresenter presenter;
    [SerializeField] private Button undoButton;
    [SerializeField] private Button redoButton;
    
    private void OnEnable()
    {
        if (ObserverUpgrade.Instance != null)
            ObserverUpgrade.Instance.TowerUpgraded += OnTowerUpgraded;
    }

    private void OnDisable()
    {
        if (ObserverUpgrade.Instance != null)
            ObserverUpgrade.Instance.TowerUpgraded -= OnTowerUpgraded;
    }

    private void OnTowerUpgraded(GameObject tower)
    {
        if (presenter == null) return;

        if (presenter.GetSelectedTower() == tower)
        {
            presenter.Refresh();
        }
    }

    public void Bind(TowerPresenter presenter)
    {
        this.presenter = presenter;
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => presenter.OnUpgradeClicked());
        
        undoButton.onClick.RemoveAllListeners();
        undoButton.onClick.AddListener(() => presenter.Undo());

        redoButton.onClick.RemoveAllListeners();
        redoButton.onClick.AddListener(() => presenter.Redo());
    }

    public void Unbind()
    {
        upgradeButton.onClick.RemoveAllListeners();
        presenter = null;
    }
    
    public void UpdateText(int cost)
    {
        upgradeCostText.text = $"Upgrade: {cost}";
    }

    public void UpdateImage(Texture tex)
    {
        if (tex == null)
        {
            upgradeImage.enabled = false;
            return;
        }

        upgradeImage.enabled = true;
        upgradeImage.texture = tex;
    }

    public void ShowMaxLevel()
    {
        upgradeCostText.text = "MAX LEVEL";
        upgradeImage.enabled = false;
    }
}