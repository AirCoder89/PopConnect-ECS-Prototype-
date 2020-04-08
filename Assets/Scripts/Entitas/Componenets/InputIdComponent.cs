
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
public class InputIdComponent: IComponent
{
       [EntityIndex] public string value;
}