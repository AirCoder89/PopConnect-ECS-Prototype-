using DG.Tweening;
using UnityEngine;

[System.Serializable]
public struct AnimationData
{
    public AnimationType type;
    public Ease selectedEase;
    public float duration;
    public float startValue;
    public float targetValue;
}

public enum AnimationType
{
    Spawn, FadeOut, Connect, MoveY
}