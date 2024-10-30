using Cysharp.Threading.Tasks;

public class GameEnd : State
{
    private readonly InGameView _inGameView;
    private readonly Enemy _enemy;
    
    public GameEnd(InGameView view,Enemy enemy)
    {
        _inGameView = view;
        _enemy = enemy;
    }
    public override async UniTask Enter()
    {
        await _inGameView.ShowGameEndPanel(_enemy.GetCurrentHp() <= 0 ? "WIN!" : "Lose!");
        Exit();
    }

    public override void Exit()
    {
        SceneController.I.SceneChange("Result");
    }
}