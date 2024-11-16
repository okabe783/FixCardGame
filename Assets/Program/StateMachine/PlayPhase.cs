using Cysharp.Threading.Tasks;

public class PlayPhase : IState
{
    public UniTask Enter()
    {
        InGameLogic.I.SetCardsDraggable(true);
        return UniTask.CompletedTask;
    }
    
    public void Exit()
    {
        InGameLogic.I.SetCardsDraggable(false);
    }
}