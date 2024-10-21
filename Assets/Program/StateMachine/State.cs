using Cysharp.Threading.Tasks;

public abstract class State
{
   //ToDo:PanelのSetUpをViewに任せる
   public virtual async UniTask Enter(string panelName)
   {
      await ShowPanel(panelName);
   }

   public abstract void OnUpdate();
   
   public abstract void Exit();

   protected async UniTask ShowPanel(string panelName)
   {
      await InGameLogic.I.ActivePhasePanel(panelName);
   }
}