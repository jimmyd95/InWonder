using UnityEngine;
using Sirenix.OdinInspector;

public class MenuManager : MonoBehaviour
{

    // [SerializeField] private InteractableUnityEventWrapper _button = new InteractableUnityEventWrapper();
    [SerializeField] private XRPortal _spawnningManager;
    private OpeningProceedure openingProceedure;
    private ToyPortal portalVFX;
    private XRPortal xrportal;
    private bool hasbeenPressed = false;

    private void Start() {
        openingProceedure = GameObject.FindGameObjectWithTag("Opening").GetComponent<OpeningProceedure>();
        portalVFX = GameObject.FindGameObjectWithTag("Portal").GetComponent<ToyPortal>();
        xrportal = GameObject.FindGameObjectWithTag("Portal").GetComponent<XRPortal>(); // since I'm destorying the portal everytime I call it, it will have to be "found" every time
    }

    [Button("Press first time")]
    public void OnButtonPressedFirstTime()
    {
        if (hasbeenPressed == false)
        {
            openingProceedure.StartOpening();
            hasbeenPressed = true;
        }
        else
        {
            Debug.Log("Button has already been pressed");
        }
        // _button.WhenUnselect.Invoke();
        // _button.WhenUnselect.RemoveListener(openingProceedure.StartOpening);
        // _button.WhenUnselect.AddListener(portalVFX.produceToy);
    }

    [Button("Coin Toss")]
    private void CoinToss(){
        if(Random.Range(0, 3) == 1){
            OnSummonPortal();
        } else {
            OnButtonPressedForToys();
        }
    }

    [Button("Press for toys")]
    private void OnButtonPressedForToys()
    {
        portalVFX.produceToy();
        // _button.WhenUnselect.Invoke();
    }

    [Button("Summon Portal")]
    private void OnSummonPortal()
    {
        xrportal.SpawnPortal();
    }

}
