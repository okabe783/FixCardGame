using Cysharp.Threading.Tasks;

public interface IAbility
{
    public string Name();
    public UniTask SetAbility(Enemy enemy);
}