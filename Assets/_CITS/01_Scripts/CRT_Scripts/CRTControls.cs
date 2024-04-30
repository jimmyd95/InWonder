using UnityEngine;
using Sirenix.OdinInspector;

public class CRTControls : MonoBehaviour
{
    [SerializeField] private GameObject _CRTScreen;
    [SerializeField] private GameObject _wonderScreen;
    [SerializeField] private GameObject _mainMenuScreen;
    private bool _isOn = true;
    // private bool _isPortalChancesReached = false;
    // private bool _locatedMenu = false;
    private GameObject menu;
    private SpawningAndVFX spawningAndVFX;

    
    private void Start() {
        spawningAndVFX = GameObject.FindObjectOfType<SpawningAndVFX>();
    }

    [Button ("Power Switch")]
    public void PowerSwitch()
    {
        _CRTScreen.transform.gameObject.SetActive(!_isOn); // first one is the letters on the screen
        _isOn = !_isOn;
    }

    [Button("Coin Toss")]
    public void CoinToss(){
        // if(Random.Range(0, 3) == 1){
        //     _wonderScreen.SetActive(true);
        //     _mainMenuScreen.SetActive(false);
        //     Debug.Log("Portal Chances Reached");
        // }else
        // {
        // }
        if (menu == null)
        {
            spawningAndVFX.SpawnMenu();
            menu = spawningAndVFX.keyItems[spawningAndVFX.keyItems.Count - 1]; // get the last item from keyItems, that's should be menu
        }
        else
        {
            // CRT's center point is on the left of the entire model, hence position + 0.25f makes the menu spawn on top of the CRT model
            menu.transform.position = new Vector3(_CRTScreen.transform.position.x + 0.25f, _CRTScreen.transform.position.y + 0.25f, Camera.main.transform.position.z);
        }
    }

    // [Button("Summon Portal")]
    // public void OnSummonPortal()
    // {
    //     xrportal = GameObject.Find("XRPortal").GetComponent<XRPortal>(); // since I'm destorying the portal everytime I call it, it will have to be "found" every time
    //     xrportal.SpawnPortal();
    //     Debug.Log("Portal Summoned");
    //     _wonderScreen.SetActive(false);
    //     _mainMenuScreen.SetActive(true);
    // }

}