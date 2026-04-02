using UnityEngine;

public class TutorialPopupController : MonoBehaviour
{
    [Header("Popups")]
    public GameObject tutorialRoot;
    public GameObject TowerBuilding;
    public GameObject EnemyAttack;
    public GameObject TowerUpgradePopup;

    [Header("Highlight Panels (każdy popup ma własny)")]
    public RectTransform towerHighlight;
    public RectTransform enemyHighlight;
    public RectTransform upgradeHighlight;

    [Header("Target UI")]
    public RectTransform towerTargetUI;
    public RectTransform enemyTargetUI;
    public RectTransform upgradeTargetUI;

    [Header("Base HP (Event Trigger)")]
    public baseHp playerBase;

    private bool EnemyAttackPopupShown = false;
    private bool upgradePopupShown = false;

    public bool UpgradePopupShown => upgradePopupShown;

    void Start()
    {
        Invoke(nameof(ShowTowerBuilding), 1f);

        if (playerBase != null)
            playerBase.OnGetHp += CheckBaseHp;
    }

    void PositionHighlight(RectTransform highlight, RectTransform target)
    {
        highlight.position = target.position;
        highlight.sizeDelta = target.sizeDelta;
    }

    void ShowTowerBuilding()
    {
        Time.timeScale = 0f;
        tutorialRoot.SetActive(true);
        TowerBuilding.SetActive(true);
        towerHighlight.gameObject.SetActive(true);
        PositionHighlight(towerHighlight, towerTargetUI);
    }

    private void CheckBaseHp()
    {
        if (!EnemyAttackPopupShown && playerBase.GetCurrHp() < playerBase.GetMaxHp())
        {
            ShowEnemyAttack();
            EnemyAttackPopupShown = true;
        }
    }

    private void ShowEnemyAttack()
    {
        Time.timeScale = 0f;
        tutorialRoot.SetActive(true);
        EnemyAttack.SetActive(true);
        enemyHighlight.gameObject.SetActive(true);
        PositionHighlight(enemyHighlight, enemyTargetUI);
    }

    public void ShowTowerUpgradePopup()
    {
        if (tutorialRoot == null || TowerUpgradePopup == null || upgradeHighlight == null || upgradeTargetUI == null)
            return;

        if (upgradePopupShown) return;

        Time.timeScale = 0f;
        tutorialRoot.SetActive(true);
        TowerUpgradePopup.SetActive(true);
        upgradeHighlight.gameObject.SetActive(true);
        PositionHighlight(upgradeHighlight, upgradeTargetUI);

        upgradePopupShown = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        tutorialRoot.SetActive(false);
        towerHighlight.gameObject.SetActive(false);
        enemyHighlight.gameObject.SetActive(false);
        upgradeHighlight.gameObject.SetActive(false);
        TowerBuilding.SetActive(false);
        EnemyAttack.SetActive(false);
        TowerUpgradePopup.SetActive(false);
    }
}