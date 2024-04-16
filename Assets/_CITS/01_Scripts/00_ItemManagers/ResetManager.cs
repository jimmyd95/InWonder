using UnityEngine;
using Sirenix.OdinInspector;

public class ResetManager : MonoBehaviour
{

    [SerializeField] private XRTransition _xrTransition;
    [SerializeField] private SpawningAndVFX _spawningAndVFX;
    [SerializeField] private ToyPortal _toyPortal;

    // set it to it's original state where there's only the menu, MR environment, and scene understanding
    [Button("Reset Game")]
    public void ResetGame()
    {
        _xrTransition.BackToMR();
        // destory the items in the scene
        foreach (var item in _spawningAndVFX.keyItems)
            Destroy(item);

        // clear the list of items
        // _spawningAndVFX.keyItems.RemoveRange(1, _spawningAndVFX.keyItems.Count - 1);
        _spawningAndVFX.keyItems.Clear();
        _spawningAndVFX.SpawnMenu(); // then respawn the menu and start everything anew
        _spawningAndVFX.keyItems[0].GetComponentInChildren<AudioSource>().Play();
        _spawningAndVFX.PreGameMenu();
        // destory all the toys within the list and then clear the list completely
        foreach (var item in _toyPortal.spanwedToys)
            Destroy(item);
        _toyPortal.spanwedToys.Clear();
    }

}
