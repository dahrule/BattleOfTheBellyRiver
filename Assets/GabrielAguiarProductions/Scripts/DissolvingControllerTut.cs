using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DissolvingControllerTut : MonoBehaviour
{
    public Animator animator;
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect VFXGraph;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    private Material[] skinnedMaterials;

    void Start()
    {
        if (VFXGraph != null)
        {
            VFXGraph.Stop();
            VFXGraph.gameObject.SetActive(false);
        }
            

        if (skinnedMesh != null)
            skinnedMaterials = skinnedMesh.materials;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown (KeyCode.Space))
        {
            StartCoroutine(DissolveCo());
        }
    }

    IEnumerator DissolveCo ()
    {
        if(VFXGraph != null)
        {
            VFXGraph.Play();
        }

        if(skinnedMaterials.Length >0)
        {
            float counter = 0;

            while(skinnedMaterials[0].GetFloat("DissolveAmount_") < 1)
            {
                counter += dissolveRate;
                for(int i=0; i<skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("DissolveAmount_", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
