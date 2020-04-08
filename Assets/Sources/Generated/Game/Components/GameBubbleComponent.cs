//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public BubbleComponent bubble { get { return (BubbleComponent)GetComponent(GameComponentsLookup.Bubble); } }
    public bool hasBubble { get { return HasComponent(GameComponentsLookup.Bubble); } }

    public void AddBubble(BubbleType newValue) {
        var index = GameComponentsLookup.Bubble;
        var component = CreateComponent<BubbleComponent>(index);
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceBubble(BubbleType newValue) {
        var index = GameComponentsLookup.Bubble;
        var component = CreateComponent<BubbleComponent>(index);
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveBubble() {
        RemoveComponent(GameComponentsLookup.Bubble);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherBubble;

    public static Entitas.IMatcher<GameEntity> Bubble {
        get {
            if (_matcherBubble == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Bubble);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherBubble = matcher;
            }

            return _matcherBubble;
        }
    }
}