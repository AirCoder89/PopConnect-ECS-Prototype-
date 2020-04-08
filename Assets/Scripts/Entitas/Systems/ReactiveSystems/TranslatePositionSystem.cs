using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class TranslatePositionSystem: ReactiveSystem<GameEntity>
{
    private Contexts _contexts;
    public TranslatePositionSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.PositionComponenet);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasPositionComponenet;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if(!e.hasPositionComponenet) continue;
            
            var spacing = _contexts.game.global.value;
            var startPos = e.positionComponenet.value.GetPosition();
            var desiredPos = new Vector2(startPos.x * spacing.xSpacing ,startPos.y * spacing.ySpacing);
                
            //-apply offset
            desiredPos.y += spacing.yOffset;
            desiredPos.x += spacing.xOffset;
                
            //- apply type
            if (e.hasCanvasPosition)
            {
                e.ReplaceCanvasPosition(desiredPos);
            }
            else
            {
                e.AddCanvasPosition(desiredPos);
            }
        }
    }
}