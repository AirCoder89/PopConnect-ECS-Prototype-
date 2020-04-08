
using System.Collections.Generic;
using DG.Tweening;
using Entitas;
using UnityEngine;
using UnityEngine.UI;

public class AnimatorSystem:ReactiveSystem<GameEntity>
{
    private Contexts _contexts;
    public AnimatorSystem(Contexts context) : base(context.game)
    {
        this._contexts = context;
    }
    
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(
            GameMatcher.Bubble, GameMatcher.BubbleView, GameMatcher.Visible, GameMatcher.Animator
        ));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasBubble && entity.hasBubbleView && entity.hasAnimator && entity.isVisible;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            var animation = entity.animator.value;
            entity.RemoveAnimator();
            switch (animation.type)
            {
                case AnimationType.Spawn : SpawnAnimation(entity, animation); break;
                case AnimationType.Connect : ConnectAnimation(entity, animation); break;
                case AnimationType.FadeOut : FadeOutAnimation(entity, animation); break;
                case AnimationType.MoveY : MoveYAnimation(entity, animation); break;
            }
        }
    }

    private void MoveYAnimation(GameEntity entity, AnimationData animation)
    {
        var rt = entity.bubbleView.value.GetComponent<RectTransform>();
        var spacing = _contexts.game.global.value;
        var desiredPos = animation.targetValue * spacing.ySpacing;
                
        //-apply offset
        desiredPos += spacing.yOffset;
        
        rt.DOAnchorPosY(desiredPos, animation.duration).SetEase(animation.selectedEase).OnComplete(() =>
        {
            entity.ReplacePositionComponenet(new IntVec2(entity.positionComponenet.value.x,(int)animation.targetValue));
            ClearEntity(entity);
        });
    }
    
    private void SpawnAnimation(GameEntity entity, AnimationData animation)
    {
        var rt = entity.bubbleView.value.GetComponent<RectTransform>();
        rt.localScale = new Vector2(animation.startValue,animation.startValue);
        rt.DOScale(new Vector2(animation.targetValue,animation.targetValue), animation.duration).SetEase(animation.selectedEase)
            .OnComplete(() =>  { ClearEntity(entity); });
    }
    
    private void ConnectAnimation(GameEntity entity, AnimationData animation)
    {
        var rt = entity.bubbleView.value.GetComponent<RectTransform>();
        rt.localScale =new Vector2(animation.startValue,animation.startValue);
        rt.DOScale(new Vector2(animation.targetValue,animation.targetValue) , animation.duration/2).OnComplete(() =>
        {
            rt.DOScale(new Vector2(animation.startValue,animation.startValue), animation.duration/2)
                .OnComplete(() => { ClearEntity(entity); });
        });
    }
    
    private void FadeOutAnimation(GameEntity entity, AnimationData animation)
    {
        var rt = entity.bubbleView.value.GetComponent<RectTransform>();
        var cGroup = entity.bubbleView.value.GetComponent<CanvasGroup>();
        cGroup.DOFade(0, animation.duration);
        rt.localScale =new Vector2(animation.startValue,animation.startValue);
        rt.DOScale(new Vector2(animation.targetValue,animation.targetValue), animation.duration).SetEase(animation.selectedEase)
            .OnComplete(() =>
            {
                ClearEntity(entity);
                entity.isVisible = false;
                entity.isDestroyed = true;
                GridLogic.UpdateFallBubbles(_contexts);
            });
    }

    private void ClearEntity(GameEntity entity)
    {
        if (entity.hasCallBack)
        {
            entity.callBack.value?.Invoke();
            entity.RemoveCallBack();
        }
        if(entity.hasAnimator)
        {
            entity.RemoveAnimator();
        }
    }
}
