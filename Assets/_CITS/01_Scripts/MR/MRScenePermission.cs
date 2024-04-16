using UnityEngine;
using Meta.XR.MRUtilityKit;

public class MRScenePermission : MonoBehaviour
{
    public MRUK mruk;
    [SerializeField] private EffectMesh _mrukEffectMesh;
    [SerializeField] private RoomThingamajigApplier _roomThingamajigApplier;

    void Denied(string permission)  => Debug.Log($"{permission} Denied");
    void Granted(string permission) => Debug.Log($"{permission} Granted");
    void Awake()
    {
        const string spatialPermission = "com.oculus.permission.USE_SCENE";

        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(spatialPermission))
        {
            var callbacks = new UnityEngine.Android.PermissionCallbacks();
            callbacks.PermissionDenied += Denied;
            callbacks.PermissionGranted += Granted;

            // avoid callbacks.PermissionDeniedAndDontAskAgain. PermissionDenied is
            // called instead unless you subscribe to PermissionDeniedAndDontAskAgain.

            UnityEngine.Android.Permission.RequestUserPermission(spatialPermission, callbacks);
            Debug.Log("Permission has been requested: " + callbacks);
        }
        else if (UnityEngine.Android.Permission.HasUserAuthorizedPermission(spatialPermission))
        {
            mruk = FindObjectOfType<MRUK>();
            // Utils.SetupEnvironmentDepth(EnvironmentDepthCreateParams createParams);
            // isSceneDepthSupported = Utils.GetEnvironmentDepthSupported() ? true : false;
            _mrukEffectMesh.CreateMesh();
            _mrukEffectMesh.AddColliders(); // add colliders to the mesh

            _roomThingamajigApplier.GetRoomObjectAndApplyIDs();
            Debug.Log("Permission has been granted");
        }
    }

}
