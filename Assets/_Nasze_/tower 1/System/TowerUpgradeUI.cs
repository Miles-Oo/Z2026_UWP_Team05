using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerUpgradeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private RawImage upgradeImage;
    [SerializeField] private Button upgradeButton;

    private TowerPresenter presenter;

    public void Bind(TowerPresenter presenter)
    {
        this.presenter = presenter;
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => presenter.OnUpgradeClicked());
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