

using System.Collections.Generic;
using Entitas;
using UnityEngine;
using UnityEngine.UI;

public class TypeSetterSystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;
    public TypeSetterSystem(Contexts context) : base(context.game)
    {
        this._contexts = context;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Bubble, GameMatcher.BubbleView,
            GameMatcher.PositionComponenet,GameMatcher.Visible));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasBubble && entity.hasBubbleView && entity.hasPositionComponenet && entity.isVisible;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var bubble in entities)
        {
            bubble.bubbleView.value.GetComponent<Image>().color = bubble.bubble.value.color;
            bubble.bubbleView.value.GetComponentInChildren<Text>().text = bubble.bubble.value.textValue;
        }
    }
}