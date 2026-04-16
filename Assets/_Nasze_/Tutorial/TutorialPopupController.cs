using UnityEngine;

public class TutorialPopupController : MonoBehaviour
{
    [Header("Popups")]
    public GameObject tutorialRoot;
    public GameObject TowerBuilding;
    public GameObject EnemyAttack;
    public GameObject TowerUpgradePopup;
    public GameObject TowerStrategyPopup;
    public GameObject TowerSellPopup;

    [Header("Highlight Panels")]
    public RectTransform towerHighlight;
    public RectTransform enemyHighlight;
    public RectTransform upgradeHighlight;
    public RectTransform strategyHighlight;
    public RectTransform sellHighlight;

    [Header("Target UI")]
    public RectTransform towerTargetUI;
    public RectTransform enemyTargetUI;
    public RectTransform upgradeTargetUI;
    public RectTransform strategyTargetUI;
    public RectTransform sellTargetUI;

    [Header("Base HP")]
    public baseHp playerBase;

    private bool towerBuildingPopupShown = false;
    private bool enemyAttackPopupShown = false;
    private bool upgradePopupShown = false;
    private bool strategyPopupShown = false;
    private bool sellPopupShown = false;

    public bool UpgradePopupShown => upgradePopupShown;
    public bool StrategyPopupShown => strategyPopupShown;
    public bool SellPopupShown => sellPopupShown;

    void Start()
    {
        Invoke(nameof(ShowTowerBuilding), 1f);

        if (playerBase != null)
            playerBase.OnGetHp += CheckBaseHp;
    }

    private void PositionHighlight(RectTransform highlight, RectTransform target)
    {
        if (highlight == null || target == null) return;

        highlight.position = target.position;
        highlight.sizeDelta = target.sizeDelta;
    }

    private void ShowPopup(GameObject popup, ref bool shownFlag, RectTransform highlight = null, RectTransform target = null)
    {
        if (tutorialRoot == null || popup == null || shownFlag) return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGame();
        }
        else
        {
            Time.timeScale = 0f;
        }

        tutorialRoot.SetActive(true);
        popup.SetActive(true);

        if (highlight != null && target != null)
        {
            highlight.gameObject.SetActive(true);
            PositionHighlight(highlight, target);
        }

        shownFlag = true;
    }

    public void ShowTowerBuilding() => ShowPopup(TowerBuilding, ref towerBuildingPopupShown, towerHighlight, towerTargetUI);
    public void ShowEnemyAttack() => ShowPopup(EnemyAttack, ref enemyAttackPopupShown, enemyHighlight, enemyTargetUI);
    public void ShowTowerUpgradePopup() => ShowPopup(TowerUpgradePopup, ref upgradePopupShown);
    public void ShowTowerStrategyPopup() => ShowPopup(TowerStrategyPopup, ref strategyPopupShown, strategyHighlight, strategyTargetUI);
    public void ShowTowerSellPopup()
    {
        if (TowerSellPopup == null && tutorialRoot != null)
        {
            TowerSellPopup = FindChildByName(tutorialRoot.transform, "TowerSellPopup");
        }

        ShowPopup(TowerSellPopup, ref sellPopupShown, sellHighlight, sellTargetUI);
    }

    private void CheckBaseHp()
    {
        if (!enemyAttackPopupShown && playerBase.GetCurrHp() < playerBase.GetMaxHp())
        {
            ShowEnemyAttack();
        }
    }

    public void ResumeGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResumeGame();
        }
        else
        {
            Time.timeScale = 1f;
        }

        tutorialRoot.SetActive(false);

        SetActiveIfNotNull(towerHighlight, false);
        SetActiveIfNotNull(enemyHighlight, false);
        SetActiveIfNotNull(upgradeHighlight, false);
        SetActiveIfNotNull(strategyHighlight, false);
        SetActiveIfNotNull(sellHighlight, false);

        SetActiveIfNotNull(TowerBuilding, false);
        SetActiveIfNotNull(EnemyAttack, false);
        SetActiveIfNotNull(TowerUpgradePopup, false);
        SetActiveIfNotNull(TowerStrategyPopup, false);
        SetActiveIfNotNull(TowerSellPopup, false);
    }

    private void SetActiveIfNotNull(Component target, bool isActive)
    {
        if (target != null)
        {
            target.gameObject.SetActive(isActive);
        }
    }

    private void SetActiveIfNotNull(GameObject target, bool isActive)
    {
        if (target != null)
        {
            target.SetActive(isActive);
        }
    }

    private GameObject FindChildByName(Transform parent, string childName)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == childName)
            {
                return child.gameObject;
            }
        }

        return null;
    }
}
