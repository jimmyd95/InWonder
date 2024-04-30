using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class OpeningProceedure : MonoBehaviour
{

    // [SerializeField] private GameObject randomSpawnPoint;
    [SerializeField] private SpawningAndVFX _spawnningManager;
    [SerializeField] private GameObject _title;
    [SerializeField] private float _musicVolume = 0.15f;
    [SerializeField] private float _titleDisappearTime = 10f;
    [SerializeField] private float _dissolveTime = 1f;
    private AudioSource openingMusic;
    private AudioSource alarmSound;
    private GameObject cameraMount;
    private GameObject tempTitle;
    // private bool isSpawning = false;

    [Button ("StartOpening")]
    public void StartOpening()
    {
        StartCoroutine(Opening());
    }
    void Update()
    {
        if(tempTitle != null){
            tempTitle.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - tempTitle.transform.position);
        }
    }

    private void LateUpdate() {
        // find "Camera Mount(Clone)" face towards player
        if (cameraMount != null)
        {
            cameraMount.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - cameraMount.transform.position);
        }
    }

    IEnumerator Opening()
    {
        Debug.Log("Opening Sequence Started");
        _spawnningManager.MenuDissolveSequence();
        // get the position of the player camera
        var camPosition = Camera.main.transform.position;

        // spawn title on top of the menu item, initiate the undissolve process
        tempTitle = Instantiate(_title, camPosition + new Vector3(0f, 1.5f, 2f), Quaternion.identity);

        // need to fine tune the bloody animation + vector graph of the title
        // foreach (var letter in tempTitle.GetComponentsInChildren<DissolveController>())
        // {
        //     letter.StartUndissolve();
        // }

        yield return new WaitForSecondsRealtime(_titleDisappearTime);

        // remove the title now and destroy it
        foreach (var letter in tempTitle.GetComponentsInChildren<DissolveController>())
        {
            letter.StartDissolve();
        }
        Destroy(tempTitle, _dissolveTime);


        openingMusic = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        // gradually decresase the background music
        while (openingMusic.volume > _musicVolume)
        {
            openingMusic.volume -= 0.05f;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        // alarmSound.Play(); // play the alarm sound
        openingMusic.Stop(); // stop the opening music
        // set play on awake to false
        openingMusic.playOnAwake = false;

        // _spawnningManager.SpawnCRT();
        _spawnningManager.SpawnRadio();
        // find the location of the audio source from Radio
        openingMusic = _spawnningManager.keyItems[_spawnningManager.keyItems.Count - 1].transform.GetChild(_spawnningManager.keyItems[2].transform.childCount - 1).GetComponent<AudioSource>();

            
        // this should find the Music from Menu

        _spawnningManager.SpwanCCTV();
        cameraMount = _spawnningManager.keyItems[3] ? 
            _spawnningManager.keyItems[3].transform.GetChild(0).GetChild(0).GetChild(_spawnningManager.keyItems[3].transform.childCount - 1).gameObject : 
            GameObject.FindGameObjectWithTag("CCTV").transform.GetChild(0).GetChild(0).GetChild(_spawnningManager.keyItems[3].transform.childCount - 1).gameObject;
        // cameraMount.transform.rotation = Quaternion.Euler(0, 180, 0);

        // alarmSound = _spawnningManager.keyItems[3] ? 
        //     _spawnningManager.keyItems[3].transform.GetChild(_spawnningManager.keyItems[3].transform.childCount - 1).GetComponent<AudioSource>() 
        //     : GameObject.FindGameObjectWithTag("CCTV").GetComponent<AudioSource>();


        // openingMusic = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

        while (openingMusic.volume < 0.3f)
        {
            openingMusic.volume += 0.05f;
            yield return new WaitForSecondsRealtime(0.1f);
        }


        // wait for x seconds so users can see the title
        yield return new WaitForSecondsRealtime(_titleDisappearTime);
        
        // secretly summons the menu back
        _spawnningManager.UndissolveItemSequence();
        _spawnningManager.PostGameMenu();
    }

}
