using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class CeilingManager : MonoBehaviour
{
    public GameObject ceilingObject;
    // public MeshRenderer ceilingCorners;
    [SerializeField] private GameObject _ceilingSpawnPoint;
    [SerializeField] private RoomThingamajigApplier _roomThingamajigApplier;

    private void Start() {
        // LocateCeilingBoundaries();
        StartCoroutine(findCeilingManager());
    }

    IEnumerator findCeilingManager(){
        yield return new WaitForSeconds(1.5f);
        LocateCeilingBoundaries();
    }

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
        // ovrRoom = tempCeiling?.GetComponentInParent<OVRSceneRoom>();
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
        else
        {
            Debug.LogError("Ceiling can not be located for whatever reason");
        }

        // Debug.LogError("ceilingPosition: " + ceilingPosition.transform.position.ToString());
        // if(ceilingObject.GetComponent<MeshRenderer>()) assignCorners();
    }

}
