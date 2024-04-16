using UnityEngine;

public class PortableObject : MonoBehaviour
{

    public delegate void HasTeleportedHandler(SeeThroughPortal sender, SeeThroughPortal destination, Vector3 newposition, Quaternion newrotation);
    public event HasTeleportedHandler HasTeleported;

    public void OnHasTeleported(SeeThroughPortal sender, SeeThroughPortal destination, Vector3 newPosition, Quaternion newRotation)
    {
        HasTeleported?.Invoke(sender, destination, newPosition, newRotation);
    }

}