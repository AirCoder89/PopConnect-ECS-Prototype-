
using Entitas;
using UnityEngine;

public class BubbleGeneratorSystem : ICleanupSystem
{
    private readonly IGroup<GameEntity> _bubbleMove;
    private readonly IGroup<GameEntity> _allBubbles;
    private Contexts _contexts;
    private int _gridCount;
    public BubbleGeneratorSystem(Contexts context)
    {
        this._contexts = context;
        _gridCount = this._contexts.game.global.value.dimension.x * this._contexts.game.global.value.dimension.y;
        _bubbleMove = context.game.GetGroup(GameMatcher.Move);
        _allBubbles = context.game.GetGroup(GameMatcher.AllOf(GameMatcher.Visible,GameMatcher.Bubble,
            GameMatcher.CanvasPosition));
    }
    
    public void Cleanup()
    {
        if (_bubbleMove.GetEntities().Length == 0 && _allBubbles.GetEntities().Length < _gridCount)
        {
            _contexts.game.audioManager.audioManager.Play(SoundList.Spawn,Random.Range(0.7f,1.3f));
            SpawnNewBubbles(_contexts);
        }
    }
    
    private void SpawnNewBubbles(Contexts contexts)
    {
        var dimension = contexts.game.global.value.dimension;
        
        for (var y = 0; y < dimension.y; y++)
        {
            for (var x = 0; x < dimension.x; x++)
            {
                var position = new IntVec2(x, y);
                if (!contexts.game.global.value.HasBubble(position))
                {
                    var bubble = BubbleContextsExtension.GetDefaultBubble(
                        contexts,
                        position,
                        contexts.game.global.value.GetRandomType()
                    );
                    
                    bubble.AddAnimator(_contexts.game.global.value.spawnAnim);
                }
            }
        }
    }
}