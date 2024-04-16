using UnityEngine;
using Oculus.Interaction;
using Sirenix.OdinInspector;

public class ButtonClass : MonoBehaviour
{
    public bool buttonSelected = false;
    // [SerializeField] private InteractableUnityEventWrapper button = new InteractableUnityEventWrapper();


    [Button("OnButtonPressed")]
    public void OnButtonPressed()
    {
        buttonSelected = true;
    }

    [Button("OnButtonReleased")]
    public void OnButtonReleased()
    {
        buttonSelected = false;
    }

}