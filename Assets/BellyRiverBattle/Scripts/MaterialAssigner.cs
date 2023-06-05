using UnityEngine;
public class MaterialAssigner: MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;

    private void Awake()
    {
        if (skinnedMeshRenderer == null) skinnedMeshRenderer=GetComponent<SkinnedMeshRenderer>();
    }

    public void UpdateMaterial(int materialIndex, Material material)
    {
        // Get the current materials array
        Material[] materials = skinnedMeshRenderer.materials;

        // Check if the material index is valid
        if (materialIndex < 0 || materialIndex >= materials.Length)
        {
            Debug.LogWarning("Invalid material index.");
            return;
        }

        materials[materialIndex] = material;
    }

}

