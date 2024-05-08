using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using Meta.XR.MRUtilityKit;

public class CeilingManager : MonoBehaviour
{
    public GameObject ceilingObject;
    // public MeshRenderer ceilingCorners;
    [SerializeField] private GameObject _ceilingSpawnPoint;
    [SerializeField] private RoomThingamajigApplier _roomThingamajigApplier;
    private GameObject tempCeiling;

    private void Start() {
        // LocateCeilingBoundaries();
        StartCoroutine(findCeilingManager());
    }

    IEnumerator findCeilingManager(){
        yield return new WaitForSeconds(1.1f);
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
        tempCeiling = _roomThingamajigApplier.getCeiling();
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
            ceilingObject = Instantiate(_ceilingSpawnPoint, tempCeiling.transform.position - new Vector3(0f, -0.125f, 0f), Quaternion.identity);
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
            var tempBoundaryList = tempCeiling.GetComponent<MRUKAnchor>().PlaneBoundary2D;
            var tempCenter = tempCeiling.GetComponent<MRUKAnchor>().GetAnchorCenter();
            var tempRandomVector = Vector2.zero; // stores the final randomized vector of the ceiling spawn

            // randomly generate the spawn point within the boundary and then repeats itself again if it is not
            do{
                var tempX = Random.Range(tempBoundaryList[0].x, tempBoundaryList[tempBoundaryList.Count - 1].x); // randomly pick an x val in the list
                var tempY = Random.Range(tempBoundaryList[0].y, tempBoundaryList[tempBoundaryList.Count - 1].y);
                Debug.Log("tempX: " + tempX + " tempY: " + tempY);
                tempRandomVector = new Vector2(Random.Range(tempCenter.x, tempX), Random.Range(tempCenter.x, tempY));
            }while(!tempCeiling.GetComponent<MRUKAnchor>().IsPositionInBoundary(tempRandomVector));
            // tempCeiling.transform.GetChild(0).GetComponent<MeshRenderer>().localBounds();

            ceilingObject.transform.position = new Vector3(tempRandomVector.x, ceilingObject.transform.position.y, tempRandomVector.y);

            // ceilingObject.GetComponent<FindSpawnPositions>().StartSpawn();
            // // I'm too lazy to build a different method based off of StartSpawn, so I assign the spawn point after StartSpawn
            // Destroy(GameObject.FindWithTag("SpawnPoint"));
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
