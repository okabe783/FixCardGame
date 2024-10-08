using System;
using UnityEngine;

[Serializable]
public class CardTransform
{
    public Vector3 WorldPos;
    public Vector3 Rotation;
    public float Scale;

    public CardTransform(Vector3 worldPos, Vector3 rotation, float scale)
    {
        WorldPos = worldPos;
        Rotation = rotation;
        Scale = scale;
    }

    public void Set(CardTransform cardTransform)
    {
        WorldPos = cardTransform.WorldPos;
        Rotation = cardTransform.Rotation;
        Scale = cardTransform.Scale;
    }
}