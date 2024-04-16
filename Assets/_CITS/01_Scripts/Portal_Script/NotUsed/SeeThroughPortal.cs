using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeeThroughPortal : MonoBehaviour
{
    public bool portalIsOpen = true;
    public SeeThroughPortal targetPortal;
    public Transform normalVisible;
    public Transform normalInvisible;

    // the camera that will render the view through the portal
    public Camera portalCam;
    public Renderer viewThroughRenderer;
    [SerializeField] private Camera mainCam;
    private RenderTexture viewthroughRenderTexture;
    private Material viewthroughMaterial;
    private Vector4 clipPlane;

    // allows you to randomly remove items from hashset
    private HashSet<PortableObject> objInPortal = new HashSet<PortableObject>();
    private HashSet<PortableObject> objInPortalToRemove = new HashSet<PortableObject>();

    private void Start() {
        // create render texture, the size of the screen is 24-bit, but we can change it to 16-bit, etc. DefaultHDR allows bloom effect
        viewthroughRenderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.DefaultHDR);
        viewthroughRenderTexture.Create(); // it automatically runs on GPU from previous line, but calling it again to prevent any unexpected lag

        // clone portal material while leaving other portal materials intact
        viewthroughMaterial = viewThroughRenderer.material;
        viewthroughMaterial.mainTexture = viewthroughRenderTexture;

        // let portal camera render on the render texture instead of player's viewpoint
        portalCam.targetTexture = viewthroughRenderTexture;

        // get the main camera, changed it so I aquire the camera from the player
        // mainCam = Camera.main;

        // set the clip plane to the portal's normal visible so it doesn't "clip" through the portal camera view
        var plane = new Plane(normalVisible.forward, normalVisible.position);
        clipPlane = new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance);

        StartCoroutine(WaitForFixedUpdate());
    }

    private IEnumerator WaitForFixedUpdate() {
        var waitForFixedUpdate = new WaitForFixedUpdate();
        while (portalIsOpen){ // run an infinite loop until the portal is closed
            yield return waitForFixedUpdate;
            try
            {
                CheckForPortalCrossing();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }


    private void CheckForPortalCrossing(){

        objInPortalToRemove.Clear(); // set clear allows to remove all the items in the hashset. new hashset will create a new hashset and that's never too good

        Debug.Log("Checking for portal crossing " + objInPortal.Count);
        foreach (var portalItem in objInPortal)
        {
            // remove all the null items in the hashset
            if (portalItem == null)
            {
                objInPortalToRemove.Add(portalItem);
                continue;
            }
            // vector to check if there's any item that is crossing the portal
            var vectorOfItemToPortal = portalItem.transform.position - transform.position;
            vectorOfItemToPortal.Normalize();

            if(Vector3.Dot(vectorOfItemToPortal, normalVisible.forward) > 0) continue;
            
            // if the item is crossing the portal, then teleport the item to the target portal
            var newPosition = TransformPositionBetweenPortals(this, targetPortal, portalItem.transform.position);
            var newRotation = TransformRotationBetweenPortals(this, targetPortal, portalItem.transform.rotation);
            portalItem.transform.SetPositionAndRotation(newPosition, newRotation);
            portalItem.OnHasTeleported(this, targetPortal, newPosition, newRotation);
            
            // add the item to the hashset to remove it later
            objInPortalToRemove.Add(portalItem);
        }

        foreach (var item in objInPortalToRemove)
        {
            objInPortal.Remove(item);
        }


        // // check if the main camera is crossing the portal
        // if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out var hit, 1000f)){
        //     // if the main camera is crossing the portal, then teleport the player to the target portal
        //     if(hit.transform.CompareTag("Portal")){
        //         // teleport the player to the target portal
        //         mainCam.transform.position = TransformPositionBetweenPortals(this, targetPortal, mainCam.transform.position);
        //         mainCam.transform.rotation = TransformRotationBetweenPortals(this, targetPortal, mainCam.transform.rotation);
        //     }
        // }
    }

    // transform world to local space via sender's visible side of the portal and then transformPoint from local to world space
    public static Vector3 TransformPositionBetweenPortals(SeeThroughPortal sender, SeeThroughPortal target, Vector3 position){
        return
            target.normalInvisible.TransformPoint(
                sender.normalVisible.InverseTransformPoint(position));
    }

    // transform rotation from one portal to antohrer
    // apply rotation from the sender's normalvisible rotation, allow it to be the local roation
    // then reverse it by multiplying the target normalInvisible rotation
    public static Quaternion TransformRotationBetweenPortals(SeeThroughPortal sender, SeeThroughPortal target, Quaternion rotation){
        return
            target.normalInvisible.rotation *
            Quaternion.Inverse(sender.normalVisible.rotation) *
            rotation;
    }

    // updating the items before the portal so it will reduce the potential lag behind any movement
    void LateUpdate()
    {
        // passting the main camera's position and rotation to the portal camera
        var virtualPosition = TransformPositionBetweenPortals(this, targetPortal, mainCam.transform.position);
        var virtualRotation = TransformRotationBetweenPortals(this, targetPortal, mainCam.transform.rotation);

        // set the portal camera's position and rotation to the virtual position and rotation
        portalCam.transform.SetPositionAndRotation(virtualPosition, virtualRotation);

        // black magic of the projection matrix
        // this is the matrix transformation that forces the targetPortal view to be rendered on the portalCam from world view to local view
        var clipThroughSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(portalCam.worldToCameraMatrix)) * targetPortal.clipPlane;

        // inherits main camera near/far clip plane and FOV
        // set the portal camera's clip plane to the portal's normal visible
        portalCam.projectionMatrix = mainCam.CalculateObliqueMatrix(clipThroughSpace);
    }

    private void OnTriggerEnter(Collider other) {
        var portalableObj = other.GetComponent<PortableObject>();
        if(portalableObj){
            objInPortal.Add(portalableObj);
        }
    }

    private void OnTriggerExit(Collider other) {
        var portalableObj = other.GetComponent<PortableObject>();
        if(portalableObj){
            objInPortal.Remove(portalableObj);
        }
    }

    // destroy everything because if not, there will be memory leak
    private void OnDestroy() {
        viewthroughRenderTexture.Release(); // release the render texture from GPU
        // destroy all the clones
        Destroy(viewthroughMaterial);
        Destroy(viewthroughRenderTexture);
    }

}
