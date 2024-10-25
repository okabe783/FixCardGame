using Cysharp.Threading.Tasks;

public abstract class State
{
   //ToDo:PanelのSetUpをViewに任せる
   public abstract UniTask Enter();

   public abstract void OnUpdate();
   
   public abstract void Exit();
}