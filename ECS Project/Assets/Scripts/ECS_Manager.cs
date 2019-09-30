using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;


public class ECS_Manager : MonoBehaviour
{
    private EntityManager entityManager;

    private void Start()
    {
        entityManager = World.Active.EntityManager;
        Entity playerEntity = entityManager.CreateEntity(
            typeof(Translation),
            typeof(Player)
        );
    }

    public struct Platform : IComponentData { };
    public struct Player : IComponentData{ };
}
