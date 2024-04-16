using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using System.Collections;

public class Spawnners : MonoBehaviour
{
    [SerializeField] private OVRSceneManagerAddons sceneManagerAddons;
    [SerializeField] private GameObject[] spawnners;
    [SerializeField] private float waitingTime = 2f;
    private Vector3 ceilingSpawn;
    private Quaternion ceilingRotation;
    private Vector3 frontwallSpwan;
    private Vector3 floorSpawn;

    private void Start()
    {
        waitForSeconds(waitingTime);

        ceilingSpawn = sceneManagerAddons.getCeiling().transform.position - new Vector3(0, 0.1f, 0);
        ceilingRotation = sceneManagerAddons.getCeiling().transform.rotation;
        spawnItem(spawnners[0], ceilingSpawn, ceilingRotation);

        frontwallSpwan = sceneManagerAddons.getFrontWall().transform.position - new Vector3(0, 0.1f, 0);
        spawnItem(spawnners[1], frontwallSpwan, sceneManagerAddons.getFrontWall().transform.rotation);

        floorSpawn = sceneManagerAddons.getFloor().transform.position + new Vector3(0.1f, 0, 0.1f);
        spawnItem(spawnners[2], floorSpawn, sceneManagerAddons.getFloor().transform.rotation);

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

    private IEnumerator waitForSeconds(float time){
        yield return new WaitForSeconds(time);
    }
}
