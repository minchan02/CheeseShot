using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct PrefabEntityComponent : IComponentData
{

    public Entity prefab;
    public float acceleration;
    public float targetRadius;

}
