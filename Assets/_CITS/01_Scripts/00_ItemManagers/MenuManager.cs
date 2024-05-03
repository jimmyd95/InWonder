using UnityEngine;
using Sirenix.OdinInspector;

public class MenuManager : MonoBehaviour
{

    // [SerializeField] private InteractableUnityEventWrapper _button = new InteractableUnityEventWrapper();
    private OpeningProceedure openingProceedure;
    private ToyPortal portalVFX;
    private XRPortal xrportal;

    private void Start() {
        openingProceedure = GameObject.FindGameObjectWithTag("Opening").GetComponent<OpeningProceedure>();
        portalVFX = GameObject.FindObjectOfType<ToyPortal>();
        xrportal = GameObject.FindObjectOfType<XRPortal>(); // since I'm destorying the portal everytime I call it, it will have to be "found" every time
    }

    [Button("Press first time")]
    public void OnButtonPressedFirstTime()
    {
        openingProceedure.StartOpening();
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
    public void OnButtonPressedForToys()
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
