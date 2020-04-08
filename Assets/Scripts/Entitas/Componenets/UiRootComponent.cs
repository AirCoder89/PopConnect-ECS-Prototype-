
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game,Unique]
public class UiRootComponent : IComponent
{
    public RectTransform value;
}