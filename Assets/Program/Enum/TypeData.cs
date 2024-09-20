//攻撃が成功したか
public enum TurnResult
{
    None,
    Success1,
    Success2,
    Success3,
    Failure1,
    Failure2,
    Failure3,
    GameWin,
    GameLose
}

//InGame中のPhase管理
public enum InGameFlow
{
    Mulligan,
    TurnStart,
    Play,
    TurnEnd,
    Wait,
}

public enum Area
{
    None,
    Field,
    Hand,
    Deck,
    Grave,
    Hell,
}
