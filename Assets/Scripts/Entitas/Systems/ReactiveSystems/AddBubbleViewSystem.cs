
using System.Collections.Generic;
using Entitas;
using UnityEngine;
using UnityEngine.UI;

public class AddBubbleViewSystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;
    private IGroup<GameEntity> _groupView;
    
    public AddBubbleViewSystem(Contexts context) : base(context.game)
    {
        this._contexts = context;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Bubble, GameMatcher.PositionComponenet, GameMatcher.Visible));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasBubble && entity.hasPositionComponenet && entity.isVisible;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        var prefab = _contexts.game.global.value.prefab;
        var canvas = _contexts.game.uiRoot.value;

        foreach (var bubbleEntity in entities)
        {
            if(bubbleEntity.isDestroyed || !bubbleEntity.isVisible) continue;
            if (!bubbleEntity.hasBubbleView)
            {
                var bubble = GameObject.Instantiate(prefab, canvas);
                bubble.name = bubbleEntity.inputId.value;
                bubbleEntity.AddBubbleView(bubble);
            }
        }
    }
}