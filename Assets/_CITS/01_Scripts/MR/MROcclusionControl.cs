using UnityEngine;
using Meta.XR.Depth;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class MROcclusionControl : MonoBehaviour
{
    public bool isVR = false;
    [SerializeField] private Material _handsMaterial;
    [SerializeField] private EnvironmentDepthOcclusionController _occlusionController;
    [SerializeField] private EnvironmentDepthTextureProvider _depthTextureProvider;
    private int currentOcclusionTypeIndex = (int)OcclusionType.NoOcclusion;
    private bool depthOn = false;

    private void Start() {
        // this should make all the objects depth occluded in MR environment 
        // occlusionController.EnableOcclusionType((OcclusionType)CurrentOcclusionTypeIndex);
        // _depthTextureProvider.RemoveHands(true);
        ToggleHands(); // just to make sure that even if the game setting has changed, it starts off having the toggle hands correctly
    }

    [Button("SwitchOcclusionType")]
    public void SwitchDepthOcclusionType()
    {
        if (_occlusionController == null || _depthTextureProvider == null)
        {
            _occlusionController = GameObject.FindGameObjectWithTag("DepthOcclusion").GetComponent<EnvironmentDepthOcclusionController>();
            _depthTextureProvider = GameObject.FindGameObjectWithTag("DepthOcclusion").GetComponent<EnvironmentDepthTextureProvider>();
        }
        if (depthOn)
        {
            currentOcclusionTypeIndex = (int)OcclusionType.NoOcclusion;
            _depthTextureProvider.SetEnvironmentDepthEnabled(false);
            depthOn = false;
        }
        else
        {
            currentOcclusionTypeIndex = (int)OcclusionType.HardOcclusion;
            _depthTextureProvider.SetEnvironmentDepthEnabled(true);
            depthOn = true;
        }
        _occlusionController.EnableOcclusionType((OcclusionType)currentOcclusionTypeIndex);
    }

    [Button("ToggleHands")]
    public void ToggleHands()
    {
        if (isVR)
        {
            _handsMaterial.renderQueue = 3000;
            _handsMaterial.SetFloat("_OutlineOpacity", 0.6f); // find the Outline of the hands and change the alpha
            _handsMaterial.SetFloat("_Opacity", 0.4f);
        }else{
            _handsMaterial.renderQueue = 1000;
            _handsMaterial.SetFloat("_OutlineOpacity", 0f);
            _handsMaterial.SetFloat("_Opacity", 0f);
        }
    }

    // enable experimental features for DepthAPI
    // currently in order to run this, you kinda need to run every restart cycle
    // cd .\platform-tools
    // .\adb.exe devices  // check if device is connected
    // .\adb.exe shell setprop debug.occlusion.experimentalEnabled 1
    // cd to D:\Tools\UnityHub\Editors\2022.3.10f1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools
}
