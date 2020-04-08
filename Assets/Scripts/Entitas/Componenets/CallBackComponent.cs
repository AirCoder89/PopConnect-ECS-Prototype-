using System;
using Entitas;
[Game]
public class CallBackComponent: IComponent
{
  public Action value;
}