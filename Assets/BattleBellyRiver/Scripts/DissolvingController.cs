using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class DissolvingController : MonoBehaviour
{
    public Animator animator;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public VisualEffect VFXGraph;
    public float dissolveRate = 0.05f;
    public float refreshRate = 0.1f;
    public float dieDelay = 0.25f;

    readonly int dieHash=Animator.StringToHash("die");
    readonly int reviveHash = Animator.StringToHash("revive");
    int dissolveAmountID = Shader.PropertyToID("DissolveAmount_");
    private bool alive = true;
    private Material[] dissolveMaterials;

    //[SerializeField]ActionBasedController currentInteractorright; // for testing
    //[SerializeField] ActionBasedController currentInteractorleft; // for testing

    void Start()
    {
        if(VFXGraph != null)
        {
            VFXGraph.Stop();
            VFXGraph.gameObject.SetActive(false);
        }
        /*else
            Debug.Log("No VFX Graph assigned in the inspector.");*/

        if (skinnedMeshRenderer != null)
            dissolveMaterials = skinnedMeshRenderer.materials;
        /*else
            Debug.Log("No Skinned Mesh Renderer assigned in the inspector.");*/
    }
   /* private void Update()
    {
        //for testing
        if(alive)
        {
            //if (currentInteractorright.GetComponent<ActionBasedController>().activateAction.action.ReadValue<float>() > 0.5f) // for testing

            if(Input.GetKeyDown(KeyCode.Space)) // for testing
            {
                Dissolve();
            }
        }

        if (!alive)
        {
            //if (currentInteractorleft.GetComponent<ActionBasedController>().activateAction.action.ReadValue<float>() > 0.5f) // for testing with quest controller
              if (Input.GetKeyDown(KeyCode.R)) Revive();  // for testing
        }
    }*/

    IEnumerator DissolveCo ()
    {
        alive = false;

        if (animator != null)
            animator.SetTrigger(dieHash);
        else
        {
            //Debug.Log("No Animator assigned in the inspector.");
            alive = true;
            yield break;
        }

        yield return new WaitForSeconds(dieDelay);
        
        if(VFXGraph != null)
        {
            VFXGraph.gameObject.SetActive(true);
            VFXGraph.Play();
        }
        /*else
            Debug.Log("No VFX Graph assigned in the inspector.");*/

        float counter = 0; 

        if(dissolveMaterials.Length > 0)
        {
            WaitForSeconds wait = new(refreshRate);
            while (dissolveMaterials[0].GetFloat(dissolveAmountID) < 1)
            {
                counter += dissolveRate;
                for(int i=0; i< dissolveMaterials.Length; i++)
                    dissolveMaterials[i].SetFloat(dissolveAmountID, counter);
                yield return wait;
            }
        }
        else
        {
            //Debug.Log("No Dissolving Material assigned to model.");
            yield break;
        }
    }

    [ContextMenu("Dissolve")]
    public void Dissolve()
    {
        StartCoroutine(DissolveCo());
    }

    public void Revive ()
    {
        if (animator != null)
        {
            animator.SetTrigger(reviveHash);
            alive = true;
        }
        /*else
            Debug.Log("No Animator assigned in the inspector.");*/

        if (dissolveMaterials.Length > 0)
        {
            for (int i = 0; i < dissolveMaterials.Length; i++)
                dissolveMaterials[i].SetFloat(dissolveAmountID, 0);
        }
    }
}
