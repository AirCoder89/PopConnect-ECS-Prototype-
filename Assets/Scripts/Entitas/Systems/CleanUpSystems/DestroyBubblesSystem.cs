
using Entitas;
using UnityEngine;

public class DestroyBubblesSystem: ICleanupSystem
{
    private Contexts _contexts;
    private IGroup<GameEntity> _bubblesToDestroy;

    public DestroyBubblesSystem(Contexts contexts)
    {
        this._contexts = contexts;
        _bubblesToDestroy = this._contexts.game.GetGroup(GameMatcher.Destroyed);
    }
    
    public void Cleanup()
    {
        foreach (var entity in _bubblesToDestroy.GetEntities())
        {
            if (entity.hasBubbleView)
            {
                GameObject.Destroy(entity.bubbleView.value);
                entity.RemoveBubbleView();
            }
            entity.RemoveAllComponents();
            entity.Destroy();
        }
        GridLogic.UpdateFallBubbles(_contexts);
    }
}