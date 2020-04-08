
using System;
using System.Collections.Generic;
using System.Linq;
using Entitas.CodeGeneration.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct EntityElement
{
   public GameEntity entity;
}

[Game,Unique,CreateAssetMenu(menuName = "Bubble", fileName = "Global")]
public class Global:ScriptableObject
{
   [BoxGroup("Prefabs")] public GameObject prefab;

   [BoxGroup("Grid Dimension")][HideLabel] public IntVec2 dimension;
   
   [TabGroup("Alignment & Spacing")][Title("Spacing")] public float xSpacing;
   [TabGroup("Alignment & Spacing")] public float ySpacing;
   [TabGroup("Alignment & Spacing")][Title("Offset")] public float xOffset;
   [TabGroup("Alignment & Spacing")] public float yOffset;

   [TabGroup("Bubble Types")] public float changingTypeRate;
   [TabGroup("Bubble Types")] public int maxGeneratedBubble;
   [TabGroup("Bubble Types")][TableList] public List<BubbleType> types;

   [TabGroup("Animations")] public float fallRange;
   [TabGroup("Animations")] public float fallSpeed;
   [TabGroup("Animations")] public AnimationData spawnAnim;
   [TabGroup("Animations")] public AnimationData connectAnim;
   [TabGroup("Animations")] public AnimationData fadeOutAnim;

   [BoxGroup("Grid View")][ShowInInspector] 
   public Color[,] bubbleGrid;
   
   //- private variables
   private List<int> _typePool;
   private System.Random _rnd;
   private int _lastType;

   public bool HasBubble(IntVec2 position)
   {
      if (position.x < 0 || position.x >= dimension.x) return true;
      if (position.y < 0 || position.y >= dimension.y) return true;
      return bubbleGrid[position.x, position.y].a > 0;
   }
   
   public void Initialize()
   {
      _rnd = new System.Random();
      this._typePool = new List<int>();
      GeneratePool();
   }

   private void GeneratePool()
   {
      //- generate type pool
      this._typePool.Clear();
      for (var i = 0; i < 100; i++) _typePool.Add(GetBubbleType());
      ShuffleList(_typePool);
      _typePool = _typePool.OrderBy(a => Guid.NewGuid()).ToList();
   }
   
   public BubbleType GetRandomType()
   {
      return types[GenerateRandomTypes()];
   }
   
   private int GenerateRandomTypes()
   {
      if(_typePool == null || _typePool.Count == 0) GeneratePool();
      var t = _typePool[0];
      _typePool.RemoveAt (0);
      return t;
   }
   
   private int GetBubbleType () {
      var random = Random.Range (0f, 1f);
      if (random > this.changingTypeRate) {
         _lastType = Random.Range (0, TypeCount);
      }
      return _lastType;
   }

   public BubbleType GetNextType(BubbleType current)
   {
      var index = types.IndexOf(current) + 1;
      return types[index];
   }
   private void ShuffleList<T>(IList<T> list)  {  
      var n = list.Count;  
      while (n > 1) {  
         n--;  
         var k = _rnd.Next(n + 1);  
         T value = list[k];  
         list[k] = list[n];  
         list[n] = value;  
      }  
   }
   
   private int TypeCount
   {
      get
      {
         if (types == null)
         {
            throw new Exception("Bubble type list is Empty or null");
         }
         return this.types.Where(t => t.value <= this.maxGeneratedBubble).ToList().Count;
      }
   }
}

[System.Serializable]
public struct BubbleType
{
   public int value;
   public string textValue;
   public Color color;
}