
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class RenderPositionSystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;
    
    public RenderPositionSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.CanvasPosition,GameMatcher.BubbleView));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasBubbleView && entity.hasCanvasPosition;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if(!e.hasCanvasPosition || !e.hasBubbleView) continue;
            var rectTransform = (RectTransform) e.bubbleView.value.transform;
            rectTransform.anchoredPosition = e.canvasPosition.value;
        }
    }
}