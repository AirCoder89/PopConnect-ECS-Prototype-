using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game]
public class PositionComponenet : IComponent
{
    [EntityIndex]
    public IntVec2 value;
}
