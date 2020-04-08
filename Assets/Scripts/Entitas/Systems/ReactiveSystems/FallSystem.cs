
using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class FallSystem: ReactiveSystem<GameEntity>
{
    private Contexts _contexts;
    private IGroup<GameEntity> _gridBubbles;

    public FallSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
       return context.CreateCollector(GameMatcher.Destroyed);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isDestroyed;
    } 

    protected override void Execute(List<GameEntity> entities)
    {
        _gridBubbles = _contexts.game.GetGroup(GameMatcher.AllOf(
        GameMatcher.Bubble, GameMatcher.Visible,GameMatcher.BubbleView, GameMatcher.PositionComponenet
            ));
        var dimension = _contexts.game.global.value.dimension;
        
        for (var x = 0; x < dimension.x; x++)
        {
            for (var y = 0; y < dimension.y; y++)
            {
                var position = new IntVec2(x, y);
                var e = _contexts.game.GetEntitiesWithPositionComponenet(position).FirstOrDefault();
                if (e != null)
                {
                    Fall(e, position);
                }
            }
        }
    }

    private void Fall(GameEntity e, IntVec2 position)
    {
        var nextRowPos = GridLogic.GetNextEmptyRow(_contexts, position);
        Debug.Log("Fall : current=" + position.y + " | y To =" + nextRowPos);
        if (nextRowPos != position.y)
        {
            Debug.Log("replace pos");
            e.ReplacePositionComponenet(new IntVec2(position.x, nextRowPos));
        }
    }
}