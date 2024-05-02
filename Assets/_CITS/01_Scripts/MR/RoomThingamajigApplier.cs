using UnityEngine;
using System.Linq;
using System.Collections;
using Meta.XR.MRUtilityKit;

public class RoomThingamajigApplier : MonoBehaviour
{
    private GameObject floor;
    private GameObject ceiling;

    private void Start() {
        StartCoroutine(GetRoomObjectAndApplyIDsCoroutine());
    }

    IEnumerator GetRoomObjectAndApplyIDsCoroutine(){
        yield return new WaitForSeconds(0.5f);
        GetRoomObjectAndApplyIDs();
    }
    
    // find the MRUKRoom object and apply the necessary tags and layers
    public void GetRoomObjectAndApplyIDs(){

        // forcing a loop to find the room objects
        while (floor == null)
            floor = GameObject.Find("FLOOR");

        // var floor = mrukComponent.GetRoomOutline();
        while (ceiling == null)
            ceiling = GameObject.Find("CEILING");

        GameObject mrukObj = GameObject.FindObjectOfType<MRUKRoom>().gameObject;
        // assign the mrukObj as the MRUKRoom script gameobject, so I won't be able to temper with the "found script object" directly
        // tempFloor.GetComponentInParent<MRUKAnchor>().gameObject.layer = LayerMask.NameToLayer("Wall");

        // else
        // {
        //     Debug.LogWarning("Temp floor has been found", tempFloor);
        //     mrukObj = tempFloor.GetComponentInParent<MRUKAnchor>().gameObject;
        // }
        Debug.Log("mrukObj: " + mrukObj.name + " tempFloor: " + floor.name);
        floor.layer = LayerMask.NameToLayer("Wall");
        floor.tag = "Floor";
        ceiling.layer = LayerMask.NameToLayer("Wall");
        ceiling.tag = "Ceiling";

        mrukObj.layer = LayerMask.NameToLayer("Wall");
        mrukObj.tag = "MainStructure";

        // apply the tag, layer, and potentially the colliders with convext on mrukObj
        ApplyLayer(mrukObj, "Wall");
        ApplyTag(mrukObj);
        ApplyWallTagToChildren();
    }

    private void ApplyLayer(GameObject item, string layerName)
    {
        item.layer = LayerMask.NameToLayer(layerName);
        foreach (Transform child in item.transform)
        {
            ApplyLayer(child.gameObject, layerName);
        }
    }

    private void ApplyWallTagToChildren(){
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Wall")){
            foreach(Transform child in item.transform){
                child.gameObject.tag = "Wall";
            }
        }
    }


    // Finds the numeric value of the layer name and applies it to the object
    private void ApplyTag(GameObject mrukobj){

        // GameObject.FindObjectsOfType<OVRSemanticClassification>();
        if (mrukobj.transform.GetComponentsInChildren<MRUKAnchor>().Length != 0)
        {
            
            foreach (var item in mrukobj.transform.GetComponentsInChildren<MRUKAnchor>()){
                if (item.name.Contains("CEILING"))
                {
                    item.gameObject.tag = "Ceiling";
                    // ApplyConvexColliders(item.gameObject, "Ceiling");
                }
                else if(item.name.Contains("FLOOR"))
                {
                    item.gameObject.tag = "Floor";
                    // ApplyConvexColliders(item.gameObject, "Floor");
                }
                else if(item.name.Contains("WALL_FACE") || item.name.Contains("INVISIBLE_WALL_FACE"))
                {
                    item.gameObject.tag = "Wall";
                    // ApplyColliders(item.gameObject, "Wall");
                }
                else
                {
                    item.gameObject.tag = "Furniture";
                    // ApplyColliders(item.gameObject, "Furniture");
                }
            }
        }
        else if (mrukobj.transform.GetComponentsInChildren<OVRSemanticClassification>().Length != 0)
        {
            foreach (var item in mrukobj.transform.GetComponentsInChildren<OVRSemanticClassification>()){
                
                if (item.Labels.Contains(OVRSceneManager.Classification.Ceiling.ToString())){
                    item.gameObject.tag = "Ceiling";
                }
                else if (item.Labels.Contains(OVRSceneManager.Classification.Floor.ToString())){
                    item.gameObject.tag = "Floor";
                }
                else if (item.Labels.Contains(OVRSceneManager.Classification.WallFace.ToString()) || 
                    item.Labels.Contains(OVRSceneManager.Classification.InvisibleWallFace.ToString()))
                {
                    item.gameObject.tag = "Wall";
                    // ApplyColliders(item.gameObject, "Wall");
                }
                else
                {
                    item.gameObject.tag = "Furniture";
                }
            }
        }
    }

    private void ApplyColliders(GameObject item, string tagName)
    {  
        // check and see if item has child/children
        if (item.transform.childCount == 0)
        {
            if (item.GetComponent<MeshFilter>() == null)
                item.AddComponent<MeshFilter>();

            if (item.GetComponent<MeshCollider>() == null)
                item.AddComponent<MeshCollider>();

            item.GetComponent<MeshCollider>().convex = true;
            item.tag = tagName;
        }
        else{
            foreach (Transform child in item.transform)
            {
                if (child.gameObject.GetComponent<MeshFilter>() != null && child.gameObject.tag == tagName)
                {
                    if(child.gameObject.GetComponent<MeshCollider>() == null)
                        child.gameObject.AddComponent<MeshCollider>();

                    child.gameObject.GetComponent<MeshCollider>().convex = true;
                    item.tag = tagName;
                }
                ApplyColliders(child.gameObject, tagName);
            }
        }

    }

    public GameObject getFloor(){
        return floor;
    }

    public GameObject getCeiling(){
        return ceiling;
    }
}