
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine.UI.Extensions;

[Game, Unique]
public class BubbleConnector : IComponent
{
    public UILineConnector value;
}