using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class SkinnedMeshToMesh : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer skinnedMesh;
    [SerializeField] VisualEffect VFXGraph;
    [SerializeField] float refreshRate;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateVFXGraph());
    }

    // Update is called once per frame
    IEnumerator UpdateVFXGraph()
    {
        while(gameObject.activeSelf)
        {
            Mesh m = new Mesh();
            skinnedMesh.BakeMesh(m);

            Vector3[] vertices = m.vertices;
            Mesh m2 = new Mesh();
            m2.vertices = vertices;

            VFXGraph.SetMesh("Mesh",m2);

            yield return new WaitForSeconds(refreshRate);
        }
    }
}
