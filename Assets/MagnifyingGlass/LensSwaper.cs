using UnityEngine;

public class LensSwaper : MonoBehaviour
{
    public GameObject lens;
    public GameObject lensRenderTexture;

    public void UseRenderTexture()
    {
        lens.SetActive(false);
        lensRenderTexture.SetActive(true);
    }

    public void UseNonRenderTexture()
    {
        lens.SetActive(true);
        lensRenderTexture.SetActive(false);
    }

}