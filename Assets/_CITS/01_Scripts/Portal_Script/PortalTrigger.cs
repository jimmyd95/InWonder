using UnityEngine;

public class PortalTrigger : MonoBehaviour
{

    [SerializeField] private AudioSource _firstEncounterMusic;
    // [SerializeField] private float time = 0.1f;
    private bool firstEncounter = true;
    private XRTransition _XRTransition;
    private XRPortal _XRPortal;
    private bool inVR = false;

    private void Start() {
        _XRTransition = GameObject.FindGameObjectWithTag("MRManager").GetComponent<XRTransition>();
        _XRPortal = GameObject.FindGameObjectWithTag("XRPortal").GetComponent<XRPortal>();
    }

    // detects if the player has entered the trigger, play music too if it's the first time, or just trigger the skybox/VR change if not
    private void OnTriggerEnter(Collider other) {
        // check if the children of the gameobject is the main camera or the player
        if(other.gameObject.CompareTag("MainCamera") || other.gameObject.CompareTag("Player"))
        {
            if(firstEncounter){
                _firstEncounterMusic.Play();
                _XRTransition.startWaitAndPlayMusic();
                firstEncounter = false;
                _XRTransition.IntoVR();
                inVR = true;
            }
            else
            {
                if(inVR)
                {
                    _XRTransition.BackToMR();
                    inVR = false;
                }
                else
                {
                    _XRTransition.IntoVR();
                    inVR = true;
                }
            }
            Debug.Log("Player exited the trigger");
            _XRPortal.DestroyPortal();
        }
    }
}
