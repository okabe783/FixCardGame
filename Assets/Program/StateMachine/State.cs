using Cysharp.Threading.Tasks;

public abstract class State
{
   public abstract UniTask Enter();

   public abstract void OnUpdate();
   
   public abstract void Exit();
}