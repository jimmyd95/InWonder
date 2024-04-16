using UnityEngine;

public class PlayerPortalController : MonoBehaviour
{
    [SerializeField] private Camera headsetCamera;
    [SerializeField] private GameObject rightHand;
    // [SerializeField] private GameObject leftHand;
    // private Vector3 headsetForwards;
    // private Vector3 rightHandForwards;
    // private Vector3 leftHandForwards;
    // private PortableObject portalableObject;

    // // private void Awake() {
    // //     // get the headset camera
    // //     headsetForwards = headsetCamera.transform.forward;
    // //     rightHandForwards = rightHand.transform.position;
    // //     leftHandForwards = leftHand.transform.position;
    // // }

    // private void Start()
    // {
    //     portalableObject = GetComponent<PortableObject>();
    //     portalableObject.HasTeleported += PortalableObjectOnHasTeleported;
    // }

    // private void PortalableObjectOnHasTeleported(Portal sender, Portal destination, Vector3 newposition, Quaternion newrotation)
    // {
    //     // For character controller to update
    //     Physics.SyncTransforms();
    // }

    // private void OnDestroy()
    // {
    //     portalableObject.HasTeleported -= PortalableObjectOnHasTeleported;
    // }


    private CharacterController characterController;
    private PortableObject portalableObject;

    public float moveSpeed = 5;
    public float turnSpeed = 5;

    private float turnRotation;

    private void Start()
    {
        // characterController = GetComponent<CharacterController>();
        portalableObject = GetComponent<PortableObject>();
        portalableObject.HasTeleported += PortalableObjectOnHasTeleported;
    }

    private void PortalableObjectOnHasTeleported(SeeThroughPortal sender, SeeThroughPortal destination, Vector3 newposition, Quaternion newrotation)
    {
        // For character controller to update
        
        Physics.SyncTransforms();
    }

    private void FixedUpdate()
    {
        // // Turn player
        
        // transform.Rotate(Vector3.up * turnRotation * turnSpeed);
        // turnRotation = 0; // Consume variable
        
        // // Move player
        
        // characterController.SimpleMove(
        //     transform.forward * Input.GetAxis("Vertical") * moveSpeed +
        //     transform.right * Input.GetAxis("Horizontal") * moveSpeed);

    }

    private void Update()
    {
        // turnRotation += Input.GetAxis("Mouse X");
    }

    private void OnDestroy()
    {
        portalableObject.HasTeleported -= PortalableObjectOnHasTeleported;
    }
}
