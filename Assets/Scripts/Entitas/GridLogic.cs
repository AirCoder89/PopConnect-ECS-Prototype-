
using System.Linq;
using DG.Tweening;
using Entitas;

public static class GridLogic
{
    public static void UpdateFallBubbles(Contexts contexts)
    {
        var dimension = contexts.game.global.value.dimension;
        
        for (var x = 0; x < dimension.x; x++)
        {
            for (var y = 0; y < dimension.y; y++)
            {
                var position = new IntVec2(x, y);
                var e = contexts.game.GetEntitiesWithPositionComponenet(position).FirstOrDefault();
                if (e != null)
                {
                    Fall(e, position,contexts);
                }
            }
        }
    }
    
    private static void Fall(GameEntity e, IntVec2 position,Contexts contexts)
    {
        var nextRowPos = GetNextEmptyRow(contexts, position);
        if (nextRowPos != position.y)
        {
            if(e.hasMove && !e.isMoveComplete) return;
            e.AddMove(new IntVec2(position.x, nextRowPos));
           /*e.AddAnimator(new AnimationData()
           {
               duration = 0.2f,
               selectedEase = Ease.OutBack,
               targetValue = nextRowPos,
               type = AnimationType.MoveY
           });*/
        }
    }
    
    public static int GetNextEmptyRow(Contexts contexts, IntVec2 position)
    {
        position.y += 1;
        while (position.y <= contexts.game.global.value.dimension.y-1 && !contexts.game.global.value.HasBubble(position))
        {
            position.y += 1;
        }

        return position.y - 1;
    }
    
}