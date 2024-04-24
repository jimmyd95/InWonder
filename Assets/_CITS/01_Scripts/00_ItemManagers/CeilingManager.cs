using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using Meta.XR.MRUtilityKit;

public class CeilingManager : MonoBehaviour
{
    public GameObject ceilingObject;
    // public MeshRenderer ceilingCorners;
    [SerializeField] private GameObject _ceilingSpawnPoint;
    [SerializeField] private RoomThingamajigApplier _roomThingamajigApplier;
    private MRUKRoom mrukroom;
    private OVRSceneRoom ovrRoom;
    private EffectMesh _mrukEffectMesh;

    public float randomizeNumber(float min, float max)
    {
        return Random.Range(min, max);
    }

    // private void assignCorners(){
    //     ceilingCorners = ceilingObject.GetComponent<MeshRenderer>() ? ceilingObject.GetComponent<MeshRenderer>() : ceilingSpawnPoint.GetComponent<MeshRenderer>();
    // }

    [Button ("Locate Ceiling Boundaries")]
    public void LocateCeilingBoundaries(){
        // GLOBAL_MESH_EffectMesh
        // [BB] InvisiblePlane(Clone)
        // check if ceiling or mruk scanned ceiling is present
        // "CEILING_EffectMesh" is the name of the ceiling in the scene
        // var temp = GameObject.Find("[BB] InvisiblePlane(Clone)").TryGetComponent<OVRSceneAnchor>().Labels;
        // OVRSceneAnchor instance = new OVRSceneAnchor();
        // OVRSemanticClassification classification = instance.GetComponent<OVRSemanticClassification>();
        // classification.Contains(OVRSceneManager.Classification.Ceiling);
        // CustomInvisiblePlane(Clone)

        // ceilingObject.transform.position = new Vector3(, randomizeNumber(2, 5), randomizeNumber(-5, 5));

        // mruk = GameObject.Find("CEILING") ? GameObject.Find("CEILING").GetComponentInParent<MRUKRoom>() : null;
        var tempCeiling = _roomThingamajigApplier.getCeiling();
        Debug.Log("tempCeiling: " + tempCeiling);
        // mruk = tempCeiling?.GetComponentInParent<MRUKRoom>();
        // mruk = GameObject.FindObjectOfType<MRUKRoom>();
        ovrRoom = tempCeiling?.GetComponentInParent<OVRSceneRoom>();
        // Debug.Log("tempCeiling: " + tempCeiling.name + " mruk: " + mruk + " ovrRoom: " + ovrRoom);

        if (tempCeiling) // This is for MRUK
        {
            // MRUKRoom mrukComponent = FindObjectOfType<MRUKRoom>();
            // mrukComponent.GetCeilingAnchor();
            // MRUKAnchor[] mrukAnchors = FindObjectsOfType<MRUKAnchor>();
            // var mrukCeiling = mrukAnchors.Where(x => x.name.Contains("CEILING")).FirstOrDefault().gameObject;
            // var tempCeiling = GameObject.Find("CEILING");
            // ceilingSpawnPoint.transform.position = new Vector3(ceilingSpawnPoint.transform.position.x, tempCeiling.transform.position.y, ceilingSpawnPoint.transform.position.z);
            ceilingObject = Instantiate(_ceilingSpawnPoint, tempCeiling.transform.position, Quaternion.identity);
            // ceilingObject.transform.position = GameObject.Find("CEILING").transform.position;
            Debug.Log("Found ceiling");
        }
        else if(ovrRoom) // OVR Scene Manager
        {
            foreach (var item in ovrRoom.GetComponentsInChildren<OVRSemanticClassification>())
            {
                if (item.Labels.Contains(OVRSceneManager.Classification.Ceiling.ToString()))
                {
                    item.gameObject.layer = LayerMask.NameToLayer("Wall");
                    item.gameObject.tag = "Ceiling";
                    _ceilingSpawnPoint.transform.position = new Vector3(_ceilingSpawnPoint.transform.position.x, item.gameObject.transform.GetChild(0).gameObject.transform.position.y, _ceilingSpawnPoint.transform.position.z);
                    Debug.Log("Found customInvisiblePlane ceiling");
                }
            }
        }
        else
        {
            Debug.LogError("No ceiling found");
        }
    }

    [Button ("Randomize Ceiling Position")]
    public void randomizeCeilingPosition(){
        if(ceilingObject == null){
            LocateCeilingBoundaries();
        }
        else if (ceilingObject)
        {
            ceilingObject.GetComponent<FindSpawnPositions>().StartSpawn();
            Destroy(GameObject.FindGameObjectWithTag("SpawnPoint"));
            Debug.Log("Random spawnning is being called");
        }
        else if(ovrRoom)
        {
            var tempBounds = GameObject.FindGameObjectWithTag("Ceiling").GetComponent<MeshRenderer>().bounds;
            Debug.Log("OVR method with tempBounds: " + tempBounds);
        }
        else
        {
            Debug.LogError("Ceiling can not be located for whatever reason");
        }

        // Debug.LogError("ceilingPosition: " + ceilingPosition.transform.position.ToString());
        // if(ceilingObject.GetComponent<MeshRenderer>()) assignCorners();
    }

}
