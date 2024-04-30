
using UnityEngine;
using Sirenix.OdinInspector;

public class XRPortal : MonoBehaviour
{
    [SerializeField] private GameObject _portal;
    [SerializeField] private float portalToPlayerDistance = 0.5f;
    [SerializeField] private float portalToGroundPosition = 0.55f;
    private GameObject tempPortal;

    // rotate the portal towards player direction, but only in the Y axis, and spawn it in front of the player with x distance
    [Button ("SpawnPortal")]
    public void SpawnPortal()
    {
        if (tempPortal != null)
            Destroy(tempPortal);
        tempPortal = Instantiate(_portal);
        var tempRotation = Camera.main.transform.rotation.eulerAngles;
        tempPortal.transform.rotation = Quaternion.Euler(0, tempRotation.y, 0);
        // portal.transform.LookAt(Camera.main.transform); // I only need the portal to rotate in the Y axis
        tempPortal.transform.position = Camera.main.transform.position + Camera.main.transform.forward * portalToPlayerDistance;
        tempPortal.transform.position = new Vector3(tempPortal.transform.position.x, portalToGroundPosition, tempPortal.transform.position.z);
    }

    // same method but with a different position
    [Button ("SpawnPortal")]
    public void SpawnPortal(Vector3 position)
    {
        if (tempPortal != null)
            Destroy(tempPortal);
        
        tempPortal = Instantiate(_portal, position, Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0));
        // var tempRotation = Camera.main.transform.rotation.eulerAngles;
        // tempPortal.transform.rotation = Quaternion.Euler(0, tempRotation.y, 0);
    }

    [Button ("DestroyPortal")]
    public void DestroyPortal()
    {
        Destroy(tempPortal);
    }

}
