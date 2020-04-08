using Entitas;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class GameController : MonoBehaviour
{
    public Global global;
    public RectTransform canvas;
    public UILineConnector bubbleConnector;
    public GraphicRaycaster mRaycaster;
    public EventSystem mEventSystem;
    public AudioManager audioManager;
    
   private Systems _systems;
   private Contexts _context;

   private void Awake()
   {
       Application.targetFrameRate = 60;
   }

   void Start()
    {
        this._context = Contexts.sharedInstance;
        this._context.game.SetGlobal(this.global);
        this._context.game.SetUiRoot(this.canvas);
        this._context.game.SetBubbleConnector(this.bubbleConnector);
        this._context.game.SetGraphicRaycast( this.mRaycaster, this.mEventSystem );
        this._context.game.SetAudioManager(this.audioManager);
        
        this._systems = CreateSystems(this._context);
        this._systems.Initialize();
    }

    private void Update()
    {
        this._systems.Execute();
        this._systems.Cleanup();
    }

    private Systems CreateSystems(Contexts contexts)
    {
        return new Feature("Game")
            .Add(new InitializeGridSystem(contexts))
            .Add(new AddBubbleViewSystem(contexts))
            .Add(new TypeSetterSystem(contexts))
            .Add(new MoveSystem(contexts))
            .Add(new TranslatePositionSystem(contexts))
            .Add(new RenderPositionSystem(contexts))
            
            .Add(new BubbleInputSystem(contexts))
            .Add(new AnimatorSystem(contexts))
            .Add(new GridDebuggerSystem(contexts))
            .Add(new DestroyBubblesSystem(contexts))
            .Add(new BubbleGeneratorSystem(contexts))
            ;

    }
}
