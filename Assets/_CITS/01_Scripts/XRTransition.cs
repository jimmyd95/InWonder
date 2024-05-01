using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class XRTransition : MonoBehaviour
{
    // public MRScenePermission MRScenePermission;
    [SerializeField] private Material _skyboxes;
    [SerializeField] private GameObject _floor;
    [SerializeField] private Material _VRfloor;
    [SerializeField] private MROcclusionControl _occlusionControl;
    [SerializeField] private Material _mrMaterial;
    [SerializeField] private Material _vrMaterial;
    [SerializeField] private Material _customRoomboxMaterial;
    [SerializeField] private GameObject _invisiblePlane;
    [SerializeField] private GameObject _invisibleVolume;
    [SerializeField] private ToyPortal _portalVFX;
    [SerializeField] private SpawningAndVFX _spawningAndVFX;
    private AudioSource mainMusic;
    private Renderer rendererVolume;
    private Renderer rendererPlane;

    // private EnvironmentDepthTextureProvider _depthTextureProvider;

    // apply hands passthrough material
    // change the MR room materials (i.e. invisible plane and volume) to the VR materials

    // private void Awake()
    // {
        // // remove hands from depth map
        // _depthTextureProvider.RemoveHands(true);

        // // restore hands in depth map
        // _depthTextureProvider.RemoveHands(false);
    // }

    // touch hand grab interactable for free grabbing objects with colliders included
    private void Start() {
        // Get the Renderer component of the GameObject
        // rendererVolume = _invisibleVolume.transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
        // rendererPlane = _invisiblePlane.transform.GetChild(0).GetComponent<Renderer>();
        _floor = GameObject.FindGameObjectWithTag("Floor").transform.GetChild(0).gameObject;
    }

    [Button("Back to MR")]
    public void BackToMR()
    {
        // this is a bit sloppy, but it works... manually changing the skybox by tuggling it on and off
        RenderSettings.skybox = null;
        _floor.GetComponent<MeshRenderer>().material = _mrMaterial;
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Color tempColour = Color.black;
        tempColour.a = 0;
        Camera.main.backgroundColor = tempColour;

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

        // // Set the material of the Renderer
        // rendererVolume.sharedMaterial = _MRMaterial;
        // rendererPlane.sharedMaterial = _MRMaterial;
        
        // _invisiblePlane.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial = _mrMaterial;
        // // get the child of the child of the gameobject. This is pain
        // _invisibleVolume.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().sharedMaterial = _mrMaterial;
        // _effectMesh.MeshMaterial = _mrMaterial;

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
        RenderSettings.skybox = _skyboxes;
        _floor.GetComponent<MeshRenderer>().material = _VRfloor;
        Camera.main.clearFlags = CameraClearFlags.Skybox;

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

        // _invisiblePlane.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial = _vrMaterial;
        // // get the child of the child of the gameobject. This is pain
        // _invisibleVolume.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().sharedMaterial = _vrMaterial;
        // // rendererVolume.sharedMaterial = _VRMaterial;
        // // rendererPlane.sharedMaterial = _VRMaterial;
        // _effectMesh.MeshMaterial = _vrMaterial;
        
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
                child.gameObject.GetComponent<MeshRenderer>().sharedMaterial = _customRoomboxMaterial;
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
