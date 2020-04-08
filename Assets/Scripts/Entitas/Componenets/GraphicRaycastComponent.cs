
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Game,Unique]
public class GraphicRaycastComponent:IComponent
{
   public GraphicRaycaster mRaycaster;
   public EventSystem mEventSystem;
}