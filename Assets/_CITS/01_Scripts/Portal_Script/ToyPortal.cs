using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class ToyPortal : MonoBehaviour
{
    public List<GameObject> spanwedToys = new List<GameObject>();
    [SerializeField] private int _storedToys = 5;
    [SerializeField] private GameObject _portalVFX;
    [SerializeField] private Vector3 _portalSize;
    [SerializeField] private Vector3 _portalRotation;
    [SerializeField] private GameObject[] _toys;
    [SerializeField] private CeilingManager _ceilingSpawn;
    // private CeilingManager _ceilingSpawn;
    private AudioSource portalSound;
    private GameObject tempPortalVFX;
    private bool canProduceToy = false;
    // private Vector3 portalPosition;
    private bool isSpawningPortal = false;
    private bool isSpawningToy = false;

    private void Start() {
        _portalSize = new Vector3(1f, 1f, 1f);
        _portalRotation = new Vector3(90f, 0f, 0f);
    }


    private void LateUpdate(){
        if (canProduceToy)
        {
            // if (GameObject.Find("RandomSpawnPoint(Clone)") != null){
            //     Destroy(GameObject.Find("RandomSpawnPoint(Clone)"));
            // }
            // _ceilingSpawn.randomizeCeilingPosition();
            // portalPosition = _ceilingSpawn.ceilingObject.transform.position;
            SpawnPortalVFX();
            StartCoroutine(SpawnToysNDestoryPortal());
            canProduceToy = false;
            isSpawningToy = false;
        }
    }

    [Button("Produce Toy")]
    public void produceToy(){
        canProduceToy = true;
    }

    [Button("Spawn PortalVFX")]
    public void SpawnPortalVFX()
    {
        // portalVFX = GameObject.Find("Portal");
        if (tempPortalVFX == null && isSpawningPortal == false)
        {
            isSpawningPortal = true;
            // tempPortalVFX = _spawningAndVFX.SpawnItem(_portalVFX, Quaternion.Euler(_portalRotation));
            // these three line can probably be readjusted and merged with SpawnningAndVFX script...
            _ceilingSpawn.randomizeCeilingPosition();
            var tempPosition = GameObject.FindWithTag("SpawnPoint").transform.position;
            tempPortalVFX = Instantiate(_portalVFX, tempPosition + new Vector3(0, -0.125f, 0f), Quaternion.Euler(_portalRotation));
            // tempPortalVFX = Instantiate(_portalVFX, new Vector3(portalPosition.x, portalPosition.y - 0.2f, portalPosition.z), Quaternion.Euler(_portalRotation));
            portalSound = tempPortalVFX.transform.GetChild(0).GetChild(0).GetComponent<AudioSource>();
            StartCoroutine(playSong());
            // slowly increase the portal size until it reaches to vector3(1f, 1f, 1f);
            StartCoroutine(IncreasePortalSize());
        }
        else
        {
            Debug.LogWarning("PortalVFX is not assigned");
        }
    }

    // destory the portal VFX
    [Button("Destroy PortalVFX")]
    public void DestroyPortalVFX()
    {
        if (tempPortalVFX != null)
        {
            StartCoroutine(stopSong());
            StartCoroutine(DecreasePortalSize());
            isSpawningPortal = false;
        }
        else
        {
            Debug.LogWarning("PortalVFX is not assigned");
        }
    }

    IEnumerator playSong(){
        portalSound.volume = 0.5f;
        portalSound.Play();
        while (portalSound.volume < 1f)
        {
            portalSound.volume += 0.1f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    IEnumerator stopSong(){
        while (portalSound.volume > 0f)
        {
            portalSound.volume -= 0.2f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        portalSound.Stop();
    }

    IEnumerator IncreasePortalSize()
    {
        while (tempPortalVFX.transform.localScale.x < _portalSize.x)
        {
            // Debug.Log("This is what we have: " + tempPortalVFX.transform.localScale);
            tempPortalVFX.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    IEnumerator DecreasePortalSize()
    {
        while (tempPortalVFX.transform.localScale.x > 0f)
        {
            // Debug.Log("Now we decrese: " + tempPortalVFX.transform.localScale);
            tempPortalVFX.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        Destroy(tempPortalVFX);
    }

    IEnumerator SpawnToysNDestoryPortal(){
        // wait for random secons and spawn toy from the portal
        yield return new WaitForSecondsRealtime(Random.Range(1f, 3f));
        // easy to remove when needed with the list
        if (spanwedToys.Count >= _storedToys){
            Destroy(spanwedToys[0]);
            spanwedToys.RemoveAt(0);
        }

        if (!isSpawningToy)
        {
            // -0.125f is the offset to make sure the toy is not clipping through the floor
            spanwedToys.Add(Instantiate(_toys[Random.Range(0, _toys.Length)], tempPortalVFX.transform.position + new Vector3(0f, -0.5f, 0f), Quaternion.identity));
            isSpawningToy = true;
        }
        yield return new WaitForSecondsRealtime(Random.Range(0.5f, 2f));
        DestroyPortalVFX();
    }

    private void OnDestroy() {
        Destroy(GameObject.Find("RandomSpawnPoint(Clone)"));
    }

    // public Vector3 getPortalPosition(){
    //     return portalPosition;
    // }

    // public void setPortalPosition(Vector3 position){
    //     portalPosition = position;
    // }
}