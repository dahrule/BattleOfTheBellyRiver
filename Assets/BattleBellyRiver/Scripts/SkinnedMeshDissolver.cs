using System.Collections;
using UnityEngine;

public class SkinnedMeshDissolver : MonoBehaviour
{
    [SerializeField] float dissolveRate = 0.02f;
    [SerializeField] float refreshRate = 0.05f;

    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] Material[] dissolveMaterials;

    IEnumerator _dissolve;

    // Start is called before the first frame update
    void Start()
    {
        if (skinnedMeshRenderer != null)
            dissolveMaterials = skinnedMeshRenderer.materials;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dissolve();     
        }
    }

    public void Dissolve()
    {
        if (_dissolve != null) return;

        _dissolve = DissolveRoutine();
        StartCoroutine(_dissolve);
    }
   IEnumerator DissolveRoutine()
    {
        float counter = 0;
        if(dissolveMaterials.Length>0)
        {
            while(dissolveMaterials[0].GetFloat("DissolveAmount_")<1)
            {
                counter += dissolveRate;

                foreach(Material mat in dissolveMaterials)
                    mat.SetFloat("DissolveAmount_", counter);
               
                yield return new WaitForSeconds(refreshRate);    
            }
        }
    }
}
