using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Manually teleport the player to a specific anchor
/// </summary>
public class TeleportPlayer : MonoBehaviour
{
    [Tooltip("The area the player is teleported to")]
    public TeleportationArea area = null;

    [Tooltip("The provider used to request the teleportation")]
    public TeleportationProvider provider = null;

    public void Teleport()
    {
        if(area && provider)
        {
            TeleportRequest request = CreateRequest();
            provider.QueueTeleportRequest(request);
        }
    }

    private TeleportRequest CreateRequest()
    {
        Transform areaTransform = area.transform;

        TeleportRequest request = new TeleportRequest()
        {
            requestTime = Time.time,
            matchOrientation = area.matchOrientation,

            destinationPosition = areaTransform.position,
            destinationRotation = areaTransform.rotation
        };

        return request;
    }
}
