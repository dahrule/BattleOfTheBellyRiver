using UnityEngine;
using System.Collections;
using System;

public class BurningMatch : MonoBehaviour {

    [SerializeField] GameObject flames;
    [SerializeField] Renderer matchStickMeshRenderer;
    [SerializeField] Animation matchStickAnimation;
    [SerializeField] Animation flamePositionAnimation;
    [SerializeField] ParticleSystem[] smokeParticles;
    [SerializeField] AudioSource matchIgniteSfx;

    [Tooltip("Time to restore to the unburnt state. If the value is smaller than or equal to zero, no restoration occurs.")]
    [SerializeField] [Range(1,10)] readonly float secondsToUnlit = 5f;

    [field: SerializeField] public bool CanResetToUnlit { set; get; } = true;

    float textureOffset = 0;
    Vector2 initialTextureOffset;

    public event Action<bool> OnIsLitChanged;
  
    private bool isLit;
    public bool IsLit
    {
        get => isLit;
        private set
        {
            if (isLit != value)
            {
                isLit = value;
                OnIsLitChanged?.Invoke(value);
            }
        }
    }

    private void Awake()
    {
        initialTextureOffset = matchStickMeshRenderer.material.GetTextureOffset("_DetailAlbedoMap");
    }

    void Start ()
    { 
        ResetToUnlit();
    }

    //Called from Unity event
    [ContextMenu("LightMatch")]
    public void LightMatch()
    {
        if (IsLit == false)
        {
            matchIgniteSfx.Play();
            StartCoroutine(Fuse());
        }
    }

    IEnumerator Fuse()
    {
        IsLit = true;

        // Start flame particle effects
        flames.SetActive(true);

        // Play animations
	    matchStickAnimation.Play ();
        flamePositionAnimation.Play ();

        yield  return new WaitForSeconds (0.2f);//time to wait before texture offset begins

        while (textureOffset < 0.1f)
        {
            textureOffset += (Time.deltaTime * 0.025f);
            SetTextureOffset(textureOffset);

            yield return null;
        }

        yield return new WaitForSeconds (5);
           
	    while (textureOffset < 0.43f)
        {
            textureOffset += (Time.deltaTime * 0.0165f);
            SetTextureOffset(textureOffset);

            CheckAndPlaySmoke(textureOffset, 0.22f, 0);
            CheckAndPlaySmoke(textureOffset, 0.27f, 1);
            CheckAndPlaySmoke(textureOffset, 0.43f, 2);

            yield return null;
        }

        textureOffset = 0;
        flames.SetActive(false);

        //Reset to the unburnt state. 
        if(CanResetToUnlit) 
            Invoke(nameof(ResetToUnlit), secondsToUnlit);
    }

    private void CheckAndPlaySmoke(float offset, float threshold, int index)
    {
        if (offset > threshold && !smokeParticles[index].isPlaying)
        {
            smokeParticles[index].Play();
        }
    }

    private void SetTextureOffset(float value)
    {
        Vector2 offsetVector = new Vector2(value, value);
        matchStickMeshRenderer.material.SetTextureOffset("_DetailAlbedoMap", offsetVector);
    }

    private void StopSmokeParticles()
    {
        foreach (ParticleSystem particle in smokeParticles)
        {
            particle.Stop();
        }
    }

    private void StopAudio(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    private void ResetMatchStickAnimation()
    {
        matchStickAnimation.Rewind();
        matchStickAnimation.Play();
        matchStickAnimation.Sample();
        matchStickAnimation.Stop();
    }

    [ContextMenu("ResetToUnlit")]
    void ResetToUnlit()
    {
        StopCoroutine(Fuse());
        textureOffset = 0;
        SetTextureOffset(initialTextureOffset.x);
        StopSmokeParticles();
        StopAudio(matchIgniteSfx);
        ResetMatchStickAnimation();
        IsLit = false;
        flames.SetActive(false);
    }
}