using System;

[Flags]
public enum EnemyAttribute
{
    Red = 1 << 0,
    Green = 1 << 1,
    Blue = 1 << 2,
    Yellow = 1 << 3,
    Black = 1 << 4,
    White = 1 << 5
}