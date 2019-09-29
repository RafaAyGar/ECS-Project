using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public class ECS_Manager : MonoBehaviour
{
    [SerializeField] private Transform t;

    private EntityManager entityManager;
   

    private void Start()
    {
        entityManager = World.Active.EntityManager;
        Entity entity = entityManager.CreateEntity(
            typeof(Translation)
        );

        entityManager.SetComponentData(entity, new Translation { Value = t.position});
    }

    private void Update()
    {

    }
}
