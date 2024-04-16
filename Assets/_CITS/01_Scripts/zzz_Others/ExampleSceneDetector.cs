using UnityEngine;

public class ExampleSceneDetector : MonoBehaviour
{
    [SerializeField] private float _radius = 1f;

    // Shoots a ray from the controller and determine whether or not this object is hit
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) // index finger trigger
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Hit multiple objects");
                    // drawing a tiny radius around the hit point, and collect all triangles with vertices within that radius
                    Collider[] hitColliders = Physics.OverlapSphere(hit.collider.transform.position, _radius);
                    // Physics.ClosestPoint(hit.collider.transform.position, hit.collider, hit.collider.transform.position, hit.collider.transform.rotation); // closest point on the collider to the hit point
                    foreach (var hitCollider in hitColliders)
                    {
                        // hitCollider.SendMessage("Exterminate", SendMessageOptions.DontRequireReceiver);
                        hitCollider.gameObject.GetComponent<MeshFilter>().mesh = null; // transform into some type of shaders/material/mesh
                        Destroy(hitCollider.gameObject); // tread with caution
                    }
                }
            }
        }
    }
    
}
