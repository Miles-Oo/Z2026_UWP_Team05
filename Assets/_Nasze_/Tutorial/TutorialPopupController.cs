using UnityEngine;

public class TutorialPopupController : MonoBehaviour
{
    [Header("Popups")]
    public GameObject tutorialRoot;      // cały canvas tutorialu
    public GameObject TowerBuilding;     // pierwszy popup
    public GameObject EnemyAttack;       // drugi popup

    [Header("Highlight Panels (każdy popup ma własny)")]
    public RectTransform towerHighlight;  // highlight dla TowerBuilding
    public RectTransform enemyHighlight;  // highlight dla EnemyAttack

    [Header("Target UI")]
    public RectTransform towerTargetUI;   // UI pod TowerBuilding
    public RectTransform enemyTargetUI;   // UI pod EnemyAttack

    [Header("Base HP (Event Trigger)")]
    public baseHp playerBase;            // obiekt bazy
    private bool secondPopupShown = false;

    void Start()
    {
        // Pokazanie pierwszego popupu po 1s
        Invoke(nameof(ShowTowerBuilding), 1f);

        // Rejestracja eventu zmiany HP bazy
        if (playerBase != null)
            playerBase.OnGetHp += CheckBaseHp;
    }

    /// <summary>
    /// Ustawienie highlightu na konkretny element UI
    /// </summary>
    void PositionHighlight(RectTransform highlight, RectTransform target)
    {
        highlight.position = target.position;
        highlight.sizeDelta = target.sizeDelta;
    }

    /// <summary>
    /// Pokazuje pierwszy popup i jego highlight
    /// </summary>
    void ShowTowerBuilding()
    {
        Time.timeScale = 0f;          // pauza gry
        tutorialRoot.SetActive(true);
        TowerBuilding.SetActive(true);
        towerHighlight.gameObject.SetActive(true);
        PositionHighlight(towerHighlight, towerTargetUI);
    }

    /// <summary>
    /// Sprawdza HP bazy i wywołuje drugi popup, jeśli baza straciła HP po raz pierwszy
    /// </summary>
    private void CheckBaseHp()
    {
        if (!secondPopupShown && playerBase.GetCurrHp() < playerBase.GetMaxHp())
        {
            ShowEnemyAttack();
            secondPopupShown = true;
        }
    }

    /// <summary>
    /// Pokazuje drugi popup i jego highlight
    /// </summary>
    private void ShowEnemyAttack()
    {
        Time.timeScale = 0f;          // pauza gry
        tutorialRoot.SetActive(true);
        EnemyAttack.SetActive(true);
        enemyHighlight.gameObject.SetActive(true);
        PositionHighlight(enemyHighlight, enemyTargetUI);
    }

    /// <summary>
    /// Wznowienie gry i ukrycie tutorialu oraz highlightów
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        tutorialRoot.SetActive(false);
        towerHighlight.gameObject.SetActive(false);
        enemyHighlight.gameObject.SetActive(false);
    }
}