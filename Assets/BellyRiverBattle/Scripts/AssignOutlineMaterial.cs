using UnityEngine;
public class AssignOutlineMaterial: MonoBehaviour
{
    [SerializeField] Renderer rendererComponent;
    [SerializeField] Material outlineMaterial;
    public Material[] materials;

    private void Awake()
    {
        if (rendererComponent == null) rendererComponent=GetComponent<Renderer>();
        materials = rendererComponent.materials;
    }

    [ContextMenu("Outline Object")]
    public void Outline()
    {

        // Assign the ouline material to the last slot in materials
        materials[^1] =outlineMaterial;

        rendererComponent.materials = materials;
    }

    public void RemoveOutline()
    {
        // Assign the ouline material to the last slot in materials
        materials[^1] = null;

        rendererComponent.materials = materials;
    }

}

