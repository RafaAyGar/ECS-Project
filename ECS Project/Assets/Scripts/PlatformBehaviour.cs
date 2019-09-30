using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class PlatformBehaviour : ComponentSystem
{
        protected override void OnUpdate()
        {
            Entities.WithAll<PlatformData>().ForEach((PlatformData data) =>
            {
                data.platformActions = data.GetComponent<PlatformActions>();
                data.distancia = Vector3.Distance(data.transform.position, Vector3.zero);
                Entities.WithAll<ECS_Manager.Player>().ForEach((ref Translation translation) =>
                {
                    if (Vector3.Distance(translation.Value, data.transform.position) < 3)
                    {
                        data.platformActions.enabled = true;
                    }
                    else
                    {
                        data.platformActions.enabled = false;
                    }
                });
            });
        }
}

public class PlayerBehaviour : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<ECS_Manager.Player>().ForEach((ref Translation translation) =>
        {
            translation.Value = GameObject.FindObjectOfType<Player>().transform.position;
        });
    }
}
