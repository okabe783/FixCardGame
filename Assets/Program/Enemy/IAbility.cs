using Cysharp.Threading.Tasks;

public interface IAbility
{
    public string Name();
    
    int ActiveTurn { get; set; } 
    
    public UniTask SetAbility(Enemy enemy);
}