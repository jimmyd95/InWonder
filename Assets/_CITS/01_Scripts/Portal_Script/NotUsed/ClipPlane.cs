using UnityEngine;

public class ClipPlane : MonoBehaviour
{
    public float nearClipPlane = 0.0001f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().nearClipPlane = nearClipPlane;
    }
}
