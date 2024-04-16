using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using Sirenix.OdinInspector;

public class DissolveController : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private VisualEffect dissolveVisualEffect;
    [SerializeField] private Material[] dissolveMaterials;    
    [SerializeField] private float dissolveRate = 1f;


    // Start is called before the first frame update
    void Start()
    {
        // this is to make sure that both basic mesh and skinned mesh are covered
        if (skinnedMeshRenderer != null)
        {
            dissolveMaterials = skinnedMeshRenderer.materials;
        }
        else if (meshRenderer != null)
        {
            dissolveMaterials = meshRenderer.materials;
        }
    }

    [Button("StartDissolve")]
    public void StartDissolve()
    {
        StartCoroutine(DissolveItem());
    }

    [Button("StartUndissolve")]
    public void StartUndissolve()
    {
        StartCoroutine(UndissolveItem());
    }

    IEnumerator DissolveItem()
    {
        if (dissolveVisualEffect != null)
        {
            dissolveVisualEffect.Play();
        }

        if (dissolveMaterials.Length > 0)
        {
            float count = 0;

            while(dissolveMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                count += Time.deltaTime * dissolveRate; // this is to make sure that the dissolve is not instant
                foreach (Material mat in dissolveMaterials)
                {
                    mat.SetFloat("_DissolveAmount", count);
                }
                yield return null;
            }
        }
    }

    IEnumerator UndissolveItem()
    {
        // if (dissolveVisualEffect != null)
        // {
        //     dissolveVisualEffect.Play();
        // }

        if (dissolveMaterials.Length > 0)
        {
            float count = 1;

            while (dissolveMaterials[0].GetFloat("_DissolveAmount") > 0)
            {
                count -= Time.deltaTime * dissolveRate; // this is to make sure that the dissolve is not instant
                foreach (Material mat in dissolveMaterials)
                {
                    mat.SetFloat("_DissolveAmount", count);
                }
                yield return null;
            }
        }
    }

}
