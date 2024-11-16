using Cysharp.Threading.Tasks;

public interface  IState
{
   public UniTask Enter();
   
   public void Exit();
}