//Dataを管理
public class RunTimeData : SingletonMonoBehaviour<RunTimeData>
{
    public InGamePhase CurrentPhase;

    //ここで召喚の処理を書く
    public bool CanSummon()
    {
        return true;
    }
}
