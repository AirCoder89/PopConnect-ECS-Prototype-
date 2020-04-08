using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IntVec2
{
   public int x;
   public int y;

   public IntVec2(int x,int y)
   {
      this.x = x;
      this.y = y;
   }

   public Vector2 GetPosition()
   {
      return new Vector2(this.x,this.y);
   }

   public bool Equals(IntVec2 vec)
   {
      return vec.x == this.x && vec.y == this.y;
   }
   
}
