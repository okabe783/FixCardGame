using Cysharp.Threading.Tasks;

public class PlayPhase : IState
{
    public UniTask Enter()
    {
        InGameSystem.I.SetCardsDraggable(true);
        return UniTask.CompletedTask;
    }
    
    public void Exit()
    {
        InGameSystem.I.SetCardsDraggable(false);
    }
}