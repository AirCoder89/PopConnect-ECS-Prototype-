

using Entitas;
using UnityEngine;

public class MoveSystem : IExecuteSystem, ICleanupSystem
{
    readonly  IGroup<GameEntity> _bubblesOnMove;
    readonly  IGroup<GameEntity> _bubblesMovedComplete;
    private float _speed = 1000f;
    private Contexts _contexts;
    
    public MoveSystem(Contexts contexts)
    {
        _contexts = contexts;
        _bubblesOnMove = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Move,GameMatcher.CanvasPosition));
        _bubblesMovedComplete = contexts.game.GetGroup(GameMatcher.MoveComplete);
    }
    
    public void Execute()
    {
        foreach (var e in _bubblesOnMove.GetEntities())
        {
            if(!e.hasMove || !e.hasCanvasPosition) continue;
            var spacing = _contexts.game.global.value;
            var startPos = e.move.value.GetPosition();
            var desiredPos = new Vector2(startPos.x * spacing.xSpacing ,startPos.y * spacing.ySpacing);
            //-apply offset
            desiredPos.y += spacing.yOffset;
            desiredPos.x += spacing.xOffset;
            
            var dir = desiredPos - e.canvasPosition.value;
            var newPosition = e.canvasPosition.value + dir.normalized * _contexts.game.global.value.fallSpeed * Time.deltaTime;
            e.ReplaceCanvasPosition(newPosition);

            var dist = dir.magnitude;
            if (dist <= _contexts.game.global.value.fallRange)
            {
                e.ReplacePositionComponenet(e.move.value);
                e.RemoveMove();
                e.isMoveComplete = true;
            }
        }
    }

    public void Cleanup()
    {
        foreach (var e in _bubblesMovedComplete.GetEntities())
        {
            e.isMoveComplete = false;
        }
    }
}