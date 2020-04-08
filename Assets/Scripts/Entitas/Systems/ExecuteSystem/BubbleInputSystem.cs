
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Entitas;
using UnityEngine;
using UnityEngine.EventSystems;

public class BubbleInputSystem : IExecuteSystem,IInitializeSystem
{
    private Contexts _contexts;
    private bool _isMoouseDown;
    private PointerEventData _mPointerEventData;
    private Dictionary<string, GameObject> _collectedBubbles;
    private BubbleType _startType;
    private GameEntity _lastBubble;
    private IntVec2 _dimension;
    private List<IntVec2> _tmpNeighbors;
    private List<GameEntity> _bubblesEntities;
    public BubbleInputSystem(Contexts contexts)
    {
        this._contexts = contexts;
        this._dimension = _contexts.game.global.value.dimension;
    }
    
    public void Initialize()
    {
        this._collectedBubbles = new Dictionary<string, GameObject>();
        this._tmpNeighbors = new List<IntVec2>();
        this._bubblesEntities = new List<GameEntity>();
    }
    
    public void Execute()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastBubble = null;
            _contexts.game.bubbleConnector.value.ClearLine();
            _collectedBubbles.Clear();
            _bubblesEntities.Clear();
            _isMoouseDown = true;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            _isMoouseDown = false;
            CollectBubbles();
            _contexts.game.bubbleConnector.value.ClearLine();
            _contexts.game.bubbleConnector.value.UpdateLine();
        }

        SelectBubbles();
    }

    private void CollectBubbles()
    {
        if(_bubblesEntities.Count <= 1 || _lastBubble == null) return;
        
        _lastBubble.ReplaceBubble(_contexts.game.global.value.GetNextType(_startType));
        _lastBubble.AddAnimator(_contexts.game.global.value.spawnAnim);
        _contexts.game.audioManager.audioManager.Play(SoundList.Merge,Random.Range(0.7f,1.3f));
       for (var i = 0; i < _bubblesEntities.Count - 1; i++)
       {
            _bubblesEntities[i].AddAnimator(new AnimationData()
            {
                duration = 0.2f,
                selectedEase = Ease.OutBack,
                startValue = 1f,
                targetValue = 2f,
                type = AnimationType.FadeOut
            });
            
        }
    }
   
    private bool Filter(GameEntity entity)
    {
        return entity.hasBubble && entity.isVisible && entity.hasBubbleView;
    }
    
    private void SelectBubbles()
    {
        if(!_isMoouseDown) return;
        _mPointerEventData = new PointerEventData(_contexts.game.graphicRaycast.mEventSystem) {position = Input.mousePosition};
        var results = new List<RaycastResult>();
        _contexts.game.graphicRaycast.mRaycaster.Raycast(_mPointerEventData, results);
        var bubble = results.FirstOrDefault(b => b.gameObject.CompareTag("Bubble"));
        if (bubble.gameObject != null)
        {
            if (!_collectedBubbles.ContainsKey(bubble.gameObject.name))
            {
                var bubbleEntity = _contexts.game.GetEntitiesWithInputId(bubble.gameObject.name).First();
                if(!Filter(bubbleEntity)) return;
                //- set Line Color
                if (_collectedBubbles.Count == 0)
                {
                    //first bubble
                    _lastBubble = null;
                    _startType = bubbleEntity.bubble.value;
                    _contexts.game.bubbleConnector.value.SetLineColor(bubbleEntity.bubble.value.color);
                }

                if (CanConnect(bubbleEntity))
                {
                    _lastBubble = bubbleEntity;
                    _lastBubble.AddAnimator(_contexts.game.global.value.connectAnim);
                    _contexts.game.audioManager.audioManager.Play(SoundList.Connect,Random.Range(0.65f,1.5f));
                    _collectedBubbles.Add(bubbleEntity.inputId.value,bubbleEntity.bubbleView.value);
                    _bubblesEntities.Add(bubbleEntity);
                    _contexts.game.bubbleConnector.value.AddBubble(bubbleEntity.bubbleView.value.GetComponent<RectTransform>());
                }
                
            }
        }
    }

    private bool CanConnect(GameEntity targetEntity)
    {
        if (_lastBubble == null) return true;
        if (targetEntity.bubble.value.value != _lastBubble.bubble.value.value) return false;
        if (NeighborsPositions().Contains(targetEntity.positionComponenet.value))
        {
            return true;
        }
        return false;
    }
    
    private List<IntVec2> NeighborsPositions()
    {
        _tmpNeighbors.Clear();
        var pos = _lastBubble.positionComponenet.value;
        
        if (pos.x > 0)
            {
                //left neighbor
                _tmpNeighbors.Add(new IntVec2(pos.x - 1, pos.y));

                if (pos.y > 0)
                {
                    //top left neighbor
                    _tmpNeighbors.Add(new IntVec2(pos.x - 1, pos.y -1));
                }

                if (pos.y < (_dimension.y - 1))
                {
                    //down left neighbor
                   _tmpNeighbors.Add(new IntVec2(pos.x - 1, pos.y + 1));
                }
            }

            if (pos.y > 0)
            {
                //top neighbor
                _tmpNeighbors.Add(new IntVec2(pos.x, pos.y - 1));

                if (pos.x < (_dimension.x - 1))
                {
                    //top right neighbor
                   _tmpNeighbors.Add(new IntVec2(pos.x + 1, pos.y - 1));
                }
            }

            if (pos.y < (_dimension.y - 1))
            {
                //down neighbor
                _tmpNeighbors.Add(new IntVec2(pos.x, pos.y + 1));
            }

            if (pos.x < (_dimension.x - 1))
            {
                //right neighbor
                _tmpNeighbors.Add(new IntVec2(pos.x + 1, pos.y));

                if (pos.y < (_dimension.y - 1))
                {
                    //down right neighbor
                    _tmpNeighbors.Add(new IntVec2(pos.x + 1, pos.y + 1));
                }
            }

            return _tmpNeighbors;
    }
}