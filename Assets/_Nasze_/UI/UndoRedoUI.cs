using UnityEngine;
using UnityEngine.UI;

public class UndoRedoUI : MonoBehaviour
{
    [SerializeField] private Button undoButton;
    [SerializeField] private Button redoButton;
    [SerializeField] private CommandManager commandManager;

    private void Start()
    {
        undoButton.onClick.AddListener(() => commandManager.Undo());
        redoButton.onClick.AddListener(() => commandManager.Redo());
    }
}