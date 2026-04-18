using UnityEngine;
using System.Collections.Generic;

public class CommandManager : MonoBehaviour
{
    private Stack<ICommand> undoStack = new();
    private Stack<ICommand> redoStack = new();

    public void ExecuteCommand(ICommand command)
    {
        Debug.Log($"EXECUTE COMMANDos: {command.GetType().Name}");

        command.Execute();
        undoStack.Push(command);
        redoStack.Clear();

        Debug.Log($"UNDO ROZMIARRRRRRRRRRRR: {undoStack.Count}");
    }

    public void Undo()
    {
        if (undoStack.Count == 0) return;

        var command = undoStack.Pop();
        command.Undo();
        redoStack.Push(command);
    }

    public void Redo()
    {
        if (redoStack.Count == 0) return;

        var command = redoStack.Pop();
        command.Execute();
        undoStack.Push(command);
    }
}