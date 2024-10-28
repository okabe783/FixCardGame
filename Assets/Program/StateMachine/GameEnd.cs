using Cysharp.Threading.Tasks;

public class GameEnd : State
{
    private InGameView _inGameView;
    
    public GameEnd(InGameView view)
    {
        _inGameView = view;
    }
    public override async UniTask Enter()
    {
        
    }

    public override void Exit()
    {
        
    }
}