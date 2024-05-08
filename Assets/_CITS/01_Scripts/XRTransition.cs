using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class XRTransition : MonoBehaviour
{
    // public MRScenePermission MRScenePermission;
    [SerializeField] private Material _skyboxes;
    [SerializeField] private Material _VRfloor;
    [SerializeField] private MROcclusionControl _occlusionControl;
    [SerializeField] private Material _mrMaterial;
    [SerializeField] private Material _vrMaterial;
    [SerializeField] private Material _VRfurnitureMaterial;
    [SerializeField] private ToyPortal _portalVFX;
    [SerializeField] private SpawningAndVFX _spawningAndVFX;
    private AudioSource mainMusic;
    private GameObject ovrSceneVolume;
    private GameObject floor;
    private GameObject globalMesh;

    // private EnvironmentDepthTextureProvider _depthTextureProvider;

    // apply hands passthrough material
    // change the MR room materials (i.e. invisible plane and volume) to the VR materials

    // touch hand grab interactable for free grabbing objects with colliders included
    private void Start() {
        StartCoroutine(findFloorTag());
        StartCoroutine(findOVRSceneVolume());
        StartCoroutine(findGlobalMesh());
    }

    IEnumerator findFloorTag(){
        yield return new WaitForSeconds(1.1f);
        floor = GameObject.FindGameObjectWithTag("Floor").transform.GetChild(0).gameObject; // find the floor but a bit later
        Debug.Log("Found the floor here: " + floor.name);
    }

    IEnumerator findOVRSceneVolume(){
        yield return new WaitForSeconds(1.1f);
        ovrSceneVolume = GameObject.FindAnyObjectByType<OVRSceneVolumeMeshFilter>().gameObject; // find the OVRSceneVolume so we can change its mateiral
        // _OVRSceneVolume.layer = LayerMask.NameToLayer("Wall"); // set it to wall layer so it can be seen through the portal
        Debug.Log("Found the OVRSceneVolume here: " + ovrSceneVolume.name);
    }

    // this is the scanned room GlobalMesh, instead of having it displayed, I want it to be hidden but still utilized
    IEnumerator findGlobalMesh(){
        yield return new WaitForSeconds(1.1f);
        globalMesh = GameObject.FindGameObjectWithTag("GlobalMesh");
        if (globalMesh)
        {
            globalMesh.layer = LayerMask.NameToLayer("Wall");
            globalMesh.transform.GetChild(0).GetComponent<MeshRenderer>().material = _mrMaterial;
        }

    }

    [Button("Back to MR")]
    public void BackToMR()
    {
        // this is a bit sloppy, but it works... manually changing the skybox by tuggling it on and off
        // not going to use it for the time being because I need the portal to "see the skybox"
        // RenderSettings.skybox = null;
        // Camera.main.clearFlags = CameraClearFlags.SolidColor;
        // Color tempColour = Color.black;
        // tempColour.a = 0;
        // Camera.main.backgroundColor = tempColour;
        floor.GetComponent<MeshRenderer>().material = _mrMaterial;
        ovrSceneVolume.SetActive(true);

        // make virtual hands invisible
        _occlusionControl.isVR = false;
        // _occlusionControl.SwitchDepthOcclusionType(); // this should turn off the virtual hands
        _occlusionControl.ToggleHands();

        if (_portalVFX.spanwedToys.Count == 0)
        {
            Debug.LogWarning("No toys found in the portalVFX spanwedToys list");
        }else{
            Debug.Log("Toys found in the portalVFX spanwedToys list");
            foreach (var item in _portalVFX.spanwedToys)
            {
                item.SetActive(true);
            }
        }

        foreach(var item in _spawningAndVFX.keyItems)
        {
            item.SetActive(true);
        }

        // GameObject.FindObjectOfType<MRUKRoom>().transform.GetComponentInChildren;

        foreach (var item in GameObject.FindGameObjectsWithTag("Wall"))
        {
            foreach (Transform child in item.transform)
            {
                child.gameObject.GetComponent<MeshRenderer>().sharedMaterial = _mrMaterial;
            }
        }

        foreach (var item in GameObject.FindGameObjectsWithTag("Furniture"))
        {
            foreach (Transform child in item.transform)
            {
                child.gameObject.GetComponent<MeshRenderer>().sharedMaterial = _mrMaterial;
            }
        }

    }

    [Button("IntoVR")]
    public void IntoVR()
    {
        // an old fashion way to change skybox, just manually setting it with camera and passthrough
        // RenderSettings.skybox = _skyboxes;
        // Camera.main.clearFlags = CameraClearFlags.Skybox;
        floor.GetComponent<MeshRenderer>().material = _VRfloor;
        ovrSceneVolume.SetActive(false);

        _occlusionControl.isVR = true; // This should turn on the virtual hands
        // _occlusionControl.SwitchDepthOcclusionType(); // this turns off the depth occlusion
        _occlusionControl.ToggleHands();

        if (_portalVFX.spanwedToys.Count == 0)
        {
            Debug.LogWarning("No toys found in the portalVFX spanwedToys list");
        }
        else
        {
            Debug.Log("Toys found in the portalVFX spanwedToys list");
            foreach (var item in _portalVFX.spanwedToys)
            {
                item.SetActive(false);
            }
        }
        
        foreach(var item in _spawningAndVFX.keyItems)
        {
            item.SetActive(false);
        }
        
        foreach (var item in GameObject.FindGameObjectsWithTag("Wall"))
        {
            foreach (Transform child in item.transform)
            {
                child.gameObject.GetComponent<MeshRenderer>().sharedMaterial = _vrMaterial;
            }
        }

        // Need to think more about how to create a custom furniture material
        foreach (var item in GameObject.FindGameObjectsWithTag("Furniture"))
        {
            foreach (Transform child in item.transform)
            {
                child.gameObject.GetComponent<MeshRenderer>().sharedMaterial = _VRfurnitureMaterial;
            }
        }
    }

    public void startWaitAndPlayMusic()
    {
        StartCoroutine(waitAndPlayMusic());
    }

    IEnumerator waitAndPlayMusic(){
        if(mainMusic == null)
        {
            mainMusic = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        }
        mainMusic.Stop();
        yield return new WaitForSecondsRealtime(60f);
        mainMusic.volume = 0.1f;
        mainMusic.Play();
        while (mainMusic.volume < 0.6f)
        {
            mainMusic.volume += 0.05f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    
}
