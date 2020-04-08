
using Entitas;
using UnityEngine;


public class InitializeGridSystem : IInitializeSystem
{
    private Contexts _contexts;
    
    public InitializeGridSystem(Contexts contexts)
    {
        this._contexts = contexts;
    }
    
    public void Initialize()
    {
        var global = _contexts.game.global.value;
        global.Initialize();
        var dimension = global.dimension;
        for (var y = 0; y < dimension.y ; y++)
        {
            for (var x = 0; x < dimension.x; x++)
            {
                BubbleContextsExtension.GetDefaultBubble(
                    _contexts,
                    new IntVec2(x, y),
                    global.GetRandomType()
                );
            }
        }
    }
}
