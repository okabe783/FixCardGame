using Cysharp.Threading.Tasks;

public class GameEnd : IState
{
    private readonly Enemy _enemy;
    
    public GameEnd(Enemy enemy)
    {
        _enemy = enemy;
    }
    public async UniTask Enter()
    {
        await InGameLogic.I.ShowGameEndPanel(_enemy.GetCurrentHp() <= 0 ? "WIN!" : "Lose!");
        Exit();
    }

    public void Exit()
    {
        SceneController.I.SceneChange("Result");
    }
}