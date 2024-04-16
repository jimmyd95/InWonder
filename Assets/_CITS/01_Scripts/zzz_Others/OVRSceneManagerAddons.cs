using System.Linq;
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEditor;

public class OVRSceneManagerAddons : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnners;
    [SerializeField] private GameObject player;

    private GameObject frontWall;
    private GameObject ceiling;
    private GameObject floor;
    protected OVRSceneManager sceneManager {get; private set;}

    private Ray ceilingRay;
    private Ray floorRay;
    private Ray wallray;

    private Vector3 ceilingSpawn;
    private Quaternion ceilingRotation;
    private Vector3 frontwallSpwan;
    private Vector3 floorSpawn;


    private void Awake()
    {
        sceneManager = GetComponent<OVRSceneManager>();
    }

    private void Start() {
        sceneManager.SceneModelLoadedSuccessfully += OnSceneModelLoadedSuccessfully;
        SpaceWarp(); // better performance & graphics
    }

    private void Update() {

        ceilingRay = new Ray(new Vector3(0, 10, 0), new Vector3(0, 10, 0) - player.transform.position); // the box might not be detectable from the inside, so let's try this
        ceiling = rayDetection(ceilingRay);
        if (ceiling != null){
            ceiling.GetComponent<GameObject>().tag = "Ceiling";
            
            ceilingSpawn = ceiling.GetComponent<Transform>().position - new Vector3(0, 0.1f, 0);
            ceilingRotation = ceiling.GetComponent<Transform>().rotation;
            spawnItem(spawnners[0], ceilingSpawn, ceilingRotation);
        }
        Debug.Log(ceiling.GetComponent<Transform>().position);

        wallray = new Ray(player.transform.forward * 2, player.transform.forward * 2 - player.transform.position);
        frontWall = rayDetection(wallray);
        if (frontWall != null){
            frontWall.GetComponent<GameObject>().tag = "FrontWall";

            frontwallSpwan = frontWall.GetComponent<Transform>().position - new Vector3(0, 0.1f, 0);
            spawnItem(spawnners[1], frontwallSpwan, Quaternion.Euler(0, 0, 0));
        }

        floorRay = new Ray(new Vector3(0, -1, 0), new Vector3(0, -1, 0) - player.transform.position);
        floor = rayDetection(floorRay);
        if (floor != null)
        {
            floor.GetComponent<GameObject>().tag = "Floor";
    
            floorSpawn = floor.GetComponent<Transform>().position + new Vector3(0.1f, 0, 0.1f);
            spawnItem(spawnners[2], floorSpawn, Quaternion.Euler(0, 0, 0));
        }

    }

    private void OnSceneModelLoadedSuccessfully()
    {
        StartCoroutine(sceneDecorations());
    }

    private IEnumerator sceneDecorations()
    {
        yield return new WaitForEndOfFrame();

        OVRSemanticClassification[] allWallClassifications = FindObjectsOfType<OVRSemanticClassification>()
            .Where(x => x.Contains(OVRSceneManager.Classification.WallFace))
            .ToArray();

        ceiling = FindObjectsOfType<OVRSemanticClassification>()
            .Where(x => x.Contains(OVRSceneManager.Classification.Ceiling))
            .FirstOrDefault()?.gameObject;
        
        floor = FindObjectsOfType<OVRSemanticClassification>()
            .Where(x => x.Contains(OVRSceneManager.Classification.Floor))
            .FirstOrDefault()?.gameObject;

        foreach (var wall in allWallClassifications){
            if (Vector3.Dot(Vector3.forward, wall.GetComponent<Transform>().position) > 0)
            {
                frontWall = wall.gameObject; // honestly just randomly pick a wall that is "in front of the player will do"
                break; // calling break here because this script only needs one wall
            }
        }
    }

    private void spawnItem(GameObject spawnner, Vector3 position,  Quaternion rotation){
        // check if instantiate is possible, if not, return null
        if (spawnner != null && position != null && rotation != null)
        {
            Instantiate(spawnner, position, rotation);
        }else{
            Debug.LogError("Something is wrong, either the spawnner isn't working or the position or the rotation");
        }
    }

    private GameObject rayDetection(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Raycast hit: " + hit.transform.gameObject.name);
            return hit.collider.gameObject;
        }
        else
        {
            Debug.Log("Raycast didn't hit anything" + ray);
            return null;
        }
    }
    private void SpaceWarp()
    {
        OVRManager.SetSpaceWarp(true);
    }
    private void OnDestroy()
    {
        sceneManager.SceneModelLoadedSuccessfully -= OnSceneModelLoadedSuccessfully;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ceilingRay);
        Gizmos.DrawWireSphere(ceilingRay.origin, 0.2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(floorRay);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(wallray);
    }

    public GameObject getCeiling()
    {
        return ceiling;
    }

    public GameObject getFrontWall()
    {
        return frontWall;
    }

    public GameObject getFloor()
    {
        return floor;
    }

}
