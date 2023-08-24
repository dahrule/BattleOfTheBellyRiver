using UnityEngine;

public class ZoomShaderScreenPos : MonoBehaviour
{
    [SerializeReference] Material material;

   

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPixels = Camera.main.WorldToScreenPoint(transform.position);
        screenPixels = new Vector2(screenPixels.x / Screen.width, screenPixels.y / Screen.height);
        material.SetVector("_ObjectScreenPosition", screenPixels);
    }
}
