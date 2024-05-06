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
    [SerializeField] private CeilingManager _ceilingSpawn;
    private GameObject _menu;
    private GameObject _menu_dissolveMask;
    private GameObject _preGameMenu;
    private GameObject _postGameMenu;
    private GameObject _CRT;
    private GameObject _CRT_dissolveMask;


    private void Start() {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine(){
        yield return new WaitForSeconds(2f);
        // _ceilingSpawn = GameObject.FindObjectOfType<CeilingManager>();
        SpawnCRT();
    }

    [Button ("Spawn Menu")]
    public void SpawnMenu()
    {
        // provide menuHolder position by either finding the ceilingObject or cheat by providing where the player camera is at, and add height to it
        // apparently the menu might spawn on the top of the ceiling, so now let's make it spawn few inches in front of the player
        SpawnKeyItem(_menuHolder,  Camera.main.transform.position + new Vector3(0, 0.5f, 0.5f), Quaternion.identity);
        // var tempMenu = Instantiate(_menuHolder, Camera.main.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        // these long ternry operators are to check in case the gameobject couldn't offer the child order
        var lastItem = keyItems.Count - 1;
        _menu = keyItems[lastItem].transform.GetChild(1).gameObject ? keyItems[lastItem].transform.GetChild(1).gameObject : GameObject.Find("Menu");
        Debug.Log("Houston we have an issue: " + _menu.name);
        _menu_dissolveMask = keyItems[lastItem].transform.GetChild(2).gameObject ? keyItems[lastItem].transform.GetChild(2).gameObject : GameObject.Find("FakeDissolveMask");
        
        _preGameMenu = _menu.transform.GetChild(0).gameObject ? _menu.transform.GetChild(0).gameObject : GameObject.Find("PreGame");
        _postGameMenu = _menu.transform.GetChild(1).gameObject ? _menu.transform.GetChild(1).gameObject : GameObject.Find("PostGame");
        Debug.Log("Pre and post Game Menu: " + _preGameMenu.name + " " + _postGameMenu.name);
        // keyItems.Add(tempMenu);
    }

    [Button ("Spawn CRT")]
    public void SpawnCRT(){
        SpawnKeyItem(_CRT_holder, Camera.main.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
    }

    [Button ("Spawn Radio")]
    public void SpawnRadio(){
        SpawnKeyItem(_OldRadio, Quaternion.identity);
    }

    [Button ("Spawn CCTV")]
    public void SpwanCCTV(){
        SpawnKeyItem(_CCTV, Quaternion.AngleAxis(180, Vector3.forward));
    }

    // spawn the item based on where the ceiling is at
    public void SpawnKeyItem(GameObject item, Quaternion rotation){
        keyItems.Add(SpawnItem(item, rotation));
    }
    public void SpawnKeyItem(GameObject item, Vector3 position, Quaternion rotation){
        keyItems.Add(SpawnItem(item, position, rotation));
    }

    public GameObject SpawnItem(GameObject item, Quaternion rotation){
        if(_ceilingSpawn) _ceilingSpawn.randomizeCeilingPosition();
        var tempPosition = GameObject.FindWithTag("SpawnPoint") 
            ? GameObject.FindWithTag("SpawnPoint").transform.position + new Vector3(0, -0.25f, 0) : 
            Camera.main.transform.position + new Vector3(0, 1f, 0);
        return Instantiate(item, tempPosition, rotation);
    }
    public GameObject SpawnItem(GameObject item, Vector3 position, Quaternion rotation){
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
        yield return new WaitForSecondsRealtime(Random.Range(5f, 10f));
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