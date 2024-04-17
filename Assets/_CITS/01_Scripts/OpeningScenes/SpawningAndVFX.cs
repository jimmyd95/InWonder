using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class SpawningAndVFX : MonoBehaviour
{
    public List<GameObject> keyItems = new List<GameObject>();
    [SerializeField] private GameObject _menuHolder;
    [SerializeField] private GameObject _CRT_holder;
    [SerializeField] private GameObject _OldRadio;
    [SerializeField] private GameObject _CCTV;
    [SerializeField] private ToyPortal _portalVFX;
    [SerializeField] private CeilingManager _ceilingSpawn;
    [SerializeField] private XRTransition _XRTransition;
    private GameObject _menu;
    private GameObject _menu_dissolveMask;
    private GameObject _preGameMenu;
    private GameObject _postGameMenu;
    private GameObject _CRT;
    private GameObject _CRT_dissolveMask;

    private void Start()
    {
        SpawnMenu();
    }

    [Button ("Spawn Menu")]
    public void SpawnMenu()
    {
        // provide menuHolder position by either finding the ceilingObject or cheat by providing where the player camera is at, and add height to it
        // apparently the menu might spawn on the top of the ceiling, so now let's make it spawn few inches in front of the player
        var tempMenu = Instantiate(_menuHolder, Camera.main.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        // these long ternry operators are to check in case the gameobject couldn't offer the child order
        _menu = tempMenu.transform.GetChild(0).gameObject ? tempMenu.transform.GetChild(0).gameObject : GameObject.Find("Menu");
        _menu_dissolveMask = tempMenu.transform.GetChild(1).gameObject ? tempMenu.transform.GetChild(1).gameObject : GameObject.Find("FakeDissolveMask");
        
        _preGameMenu = _menu.transform.GetChild(0).gameObject ? _menu.transform.GetChild(0).gameObject : GameObject.Find("PreGame");
        _postGameMenu = _menu.transform.GetChild(1).gameObject ? _menu.transform.GetChild(1).gameObject : GameObject.Find("PostGame");
        keyItems.Add(tempMenu);
    }

    [Button ("Spawn CRT")]
    public void SapwnCRT(){
        SpawnKeyItem(_CRT_holder, Quaternion.identity);
    }

    [Button ("Spawn Radio")]
    public void SapwnRadio(){
        SpawnKeyItem(_OldRadio, Quaternion.identity);
    }

    [Button ("Spawn CCTV")]
    public void SapwnCCTV(){
        SpawnKeyItem(_CCTV, Quaternion.Euler(180f, 0f, 0f));
    }

    // spawn the item based on where the ceiling is at
    public void SpawnKeyItem(GameObject item, Quaternion rotation){
        keyItems.Add(SpawnItem(item, rotation));
    }

    public GameObject SpawnItem(GameObject item, Quaternion rotation){
        _ceilingSpawn.randomizeCeilingPosition();
        var tempPosition = GameObject.FindWithTag("SpawnPoint") 
            ? GameObject.FindWithTag("SpawnPoint").transform.position + new Vector3(0, -0.25f, 0) : 
            Camera.main.transform.position + new Vector3(0, 1f, 0);
        return Instantiate(item, tempPosition, rotation);
    }
    public GameObject SpawnItem(GameObject item, Vector3 position, Quaternion rotation){
        _ceilingSpawn.randomizeCeilingPosition();
        return Instantiate(item, position, rotation);
    }

    [Button ("Dissolve items")]
    public void MenuDissolveSequence()
    {
        StartCoroutine(dissolveMenu());
        // StartCoroutine(DissolveCRT());
        StartCoroutine(undissolveItems());
    }

    [Button ("Undissolve item sequences")]
    public void UndissolveItemSequence(){
        undissolveItems();
    }

    [Button ("Pre Game Menu")]
    public void PreGameMenu(){
        _preGameMenu.SetActive(true);
        _postGameMenu.SetActive(false);
    }

    [Button ("Post Game Menu")]
    public void PostGameMenu(){
        _preGameMenu.SetActive(false);
        _postGameMenu.SetActive(true);
    }

    IEnumerator dissolveMenu(){
        _menu_dissolveMask.SetActive(true);
        yield return new WaitForSecondsRealtime(0.01f); // adding a tiny amount of wait time so the magical transition can be seen smoothly
        _menu_dissolveMask.GetComponent<DissolveController>().StartDissolve();
        _menu.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        _menuHolder.SetActive(false);
    }

    IEnumerator dissolveCRT(){
        _CRT_dissolveMask.SetActive(true);
        yield return new WaitForSecondsRealtime(0.01f); // adding a tiny amount of wait time so the magical transition can be seen smoothly
        _CRT_dissolveMask.GetComponent<DissolveController>().StartDissolve();
        _CRT.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        _CRT_holder.SetActive(false);
    }
    
    IEnumerator undissolveItems()
    {
        yield return new WaitForSecondsRealtime(Random.Range(2f, 5f));
        _menuHolder.SetActive(true);
        _menu_dissolveMask.GetComponent<DissolveController>().StartUndissolve();
        _menu_dissolveMask.SetActive(false);
        _menu.SetActive(true);
        // _CRT_holder.SetActive(true);
        // _CRT_dissolveMask.GetComponent<DissolveController>().StartUndissolve();
        // _CRT_dissolveMask.SetActive(false);
        // _CRT.SetActive(true);
    }

}