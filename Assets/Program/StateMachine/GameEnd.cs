using Cysharp.Threading.Tasks;

public class GameEnd : State
{
    private readonly Enemy _enemy;
    
    public GameEnd(Enemy enemy)
    {
        _enemy = enemy;
    }
    public override async UniTask Enter()
    {
        await InGameLogic.I.ShowGameEndPanel(_enemy.GetCurrentHp() <= 0 ? "WIN!" : "Lose!");
        Exit();
    }

    public override void Exit()
    {
        SceneController.I.SceneChange("Result");
    }
}