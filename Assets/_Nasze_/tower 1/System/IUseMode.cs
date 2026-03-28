public interface IUseMode
{
    Mode GetMode();
    void EnterMode();
    void ExitMode();
    void PrewMode();
   // void ActionMode();
    bool ActionMode();
}
