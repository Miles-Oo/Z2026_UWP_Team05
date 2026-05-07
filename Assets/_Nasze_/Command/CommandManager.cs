using UnityEngine;
using System.Collections.Generic;

public class CommandManager : MonoBehaviour
{
    private const int max_stack = 5;
    private Stack<ICommand> undoStack = new();
    private Stack<ICommand> redoStack = new();

    public void ExecuteCommand(ICommand command)
    {
        Debug.Log($"EXECUTE COMMANDos: {command.GetType().Name}");

        command.Execute();
        if (command is ICommandWithHistory historyCommand && !historyCommand.WasSkipped)
        {
            undoStack.Push(command);
            redoStack.Clear();

            TrimUndoStack();
        }

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
        command.Redo();
        undoStack.Push(command);
    }

    private void TrimUndoStack()
    {
        while (undoStack.Count > max_stack)
            RemoveBottomElement(undoStack);
    }

    private void RemoveBottomElement(Stack<ICommand> stack)
    {
        var temp = new Stack<ICommand>();

        while (stack.Count > 0)
            temp.Push(stack.Pop());

        temp.Pop();

        while (temp.Count > 0)
            stack.Push(temp.Pop());
    }
}