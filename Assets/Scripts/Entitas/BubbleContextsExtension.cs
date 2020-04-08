using System.Collections.Generic;
using System.Linq;

namespace Entitas
{
    public static class BubbleContextsExtension
    {
        private static System.Random _random = new System.Random();
        
        public static GameEntity GetDefaultBubble(Contexts context,IntVec2 position, BubbleType type)
        {
            var bubble = context.game.CreateEntity();
            bubble.AddPositionComponenet(position); //pos in grid
            bubble.AddBubble(type);
            bubble.AddInputId(RandomString(16));
            bubble.isVisible = true;
            return bubble;
        }
        
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}