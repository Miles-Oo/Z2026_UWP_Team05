using UnityEngine;
using UnityEngine.UI;

public class TutorialPopupController : MonoBehaviour
{
    [Header("Popups")]
    public GameObject tutorialRoot;
    public GameObject TowerBuilding;
    public GameObject EnemyAttack;
    public GameObject TowerUpgradePopup;
    public GameObject TowerStrategyPopup;

    [Header("Highlight Panels")]
    public RectTransform towerHighlight;
    public RectTransform enemyHighlight;
    public RectTransform upgradeHighlight;
    public RectTransform strategyHighlight;

    [Header("Target UI")]
    public RectTransform towerTargetUI;
    public RectTransform enemyTargetUI;
    public RectTransform upgradeTargetUI;
    public RectTransform strategyTargetUI;

    [Header("Base HP")]
    public baseHp playerBase;

    [Header("Tutorial Toggle")]
    [SerializeField] private Toggle disableTutorialToggle;

    private bool tutorialsDisabled = false;

    private bool towerBuildingPopupShown = false;
    private bool enemyAttackPopupShown = false;
    private bool upgradePopupShown = false;
    private bool strategyPopupShown = false;

    public bool UpgradePopupShown => upgradePopupShown;
    public bool StrategyPopupShown => strategyPopupShown;

    void Start()
    {
        Invoke(nameof(ShowTowerBuilding), 1f);  

        if (playerBase != null)
            playerBase.OnHpChanged += CheckBaseHp;

        if (ObserverBuild.Instance != null)
            ObserverBuild.Instance.TowerBuilt  += OnTowerBuilt;
        
        if (ObserverUpgrade.Instance != null)
            ObserverUpgrade.Instance.TowerUpgraded += OnTowerUpgraded;
    }

    private void OnDestroy()
    {
        if (ObserverBuild.Instance != null)
            ObserverBuild.Instance.TowerBuilt  -= OnTowerBuilt;
        
        if (ObserverUpgrade.Instance != null)
            ObserverUpgrade.Instance.TowerUpgraded -= OnTowerUpgraded;

        if (playerBase != null)
            playerBase.OnHpChanged -= CheckBaseHp;
    }

    private void OnTowerBuilt(GameObject tower)
    {
        if (tutorialsDisabled) return;
        if (!upgradePopupShown)
        {
            ShowTowerUpgradePopup();
        }
    }

    private void OnTowerUpgraded(GameObject tower)
    {
        if (tutorialsDisabled) return;

        if (!strategyPopupShown)
        {
            ShowTowerStrategyPopup();
        }
    }

    private void PositionHighlight(RectTransform highlight, RectTransform target)
    {
        if (highlight == null || target == null) return;

        highlight.position = target.position;
        highlight.sizeDelta = target.sizeDelta;
    }

    private void ShowPopup(GameObject popup, ref bool shownFlag, RectTransform highlight = null, RectTransform target = null)
    {
        if (tutorialsDisabled) return;
        if (tutorialRoot == null || popup == null || shownFlag) return;

        Time.timeScale = 0f;
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

    private void CheckBaseHp()
    {
        if (!enemyAttackPopupShown && playerBase.GetCurrHp() < playerBase.GetMaxHp())
        {
            ShowEnemyAttack();
        }
    }

    public void ResumeGame()
    {
        if (disableTutorialToggle != null && disableTutorialToggle.isOn)
        {
            tutorialsDisabled = true;
        }
        Time.timeScale = 1f;
        tutorialRoot.SetActive(false);

        towerHighlight.gameObject.SetActive(false);
        enemyHighlight.gameObject.SetActive(false);
        upgradeHighlight.gameObject.SetActive(false);
        strategyHighlight.gameObject.SetActive(false);

        TowerBuilding.SetActive(false);
        EnemyAttack.SetActive(false);
        TowerUpgradePopup.SetActive(false);
        TowerStrategyPopup.SetActive(false);
    }
}