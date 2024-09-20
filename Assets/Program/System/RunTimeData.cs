//Dataを管理
public class RunTimeData : SingletonMonoBehaviour<RunTimeData>
{
    public InGameFlow CurrentPhase;

    //ここで召喚の処理を書く
    public bool CanSummon()
    {
        return true;
    }
}
