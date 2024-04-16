using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class CRTControls : MonoBehaviour
{
    [SerializeField] private GameObject _CRTScreen;
    [SerializeField] private GameObject _wonderScreen;
    [SerializeField] private GameObject _mainMenuScreen;
    private bool _isOn = true;
    private XRPortal xrportal;
    // private bool _isPortalChancesReached = false;
    // private bool _locatedMenu = false;
    private GameObject _menu;

    
    private void Start() {
        _menu = GameObject.Find("MenuHolder(Clone)");
    }

    [Button ("Power Switch")]
    public void PowerSwitch()
    {
        _CRTScreen.transform.gameObject.SetActive(!_isOn); // first one is the letters on the screen
        _isOn = !_isOn;
    }

    [Button("Coin Toss")]
    public void CoinToss(){
        if(Random.Range(0, 3) == 1){
            _wonderScreen.SetActive(true);
            _mainMenuScreen.SetActive(false);
            Debug.Log("Portal Chances Reached");
        }else
        {
            _menu.transform.position = new Vector3(_CRTScreen.transform.position.x, _CRTScreen.transform.position.y + 0.25f, Camera.main.transform.position.z);
        }
    }

    [Button("Summon Portal")]
    public void OnSummonPortal()
    {
        xrportal = GameObject.Find("XRPortal").GetComponent<XRPortal>(); // since I'm destorying the portal everytime I call it, it will have to be "found" every time
        xrportal.SpawnPortal();
        Debug.Log("Portal Summoned");
        _wonderScreen.SetActive(false);
        _mainMenuScreen.SetActive(true);
    }

}