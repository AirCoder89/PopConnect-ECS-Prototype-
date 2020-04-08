
using Entitas;
using UnityEngine;

public class GridDebuggerSystem : IExecuteSystem
{
    private Contexts _contexts;
    private IGroup<GameEntity> bubblesInGrid;
    
    public GridDebuggerSystem(Contexts context)
    {
        this._contexts = context;
    }
    
    public void Execute()
    {
        var dimension = this._contexts.game.global.value.dimension;
        this._contexts.game.global.value.bubbleGrid = new Color[dimension.x, dimension.y];
        bubblesInGrid = _contexts.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Bubble, GameMatcher.BubbleView,
            GameMatcher.PositionComponenet));

        foreach (var bubble in bubblesInGrid.GetEntities())
        {
            if(bubble.isDestroyed || !bubble.isVisible) continue;
            var pos = bubble.positionComponenet.value;
            this._contexts.game.global.value.bubbleGrid[pos.x, pos.y] = bubble.bubble.value.color;
        }
    }
}