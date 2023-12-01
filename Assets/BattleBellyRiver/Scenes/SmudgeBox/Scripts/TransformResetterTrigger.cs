using UnityEngine;
public class TransformResetterTrigger : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<TransformResetter>(out TransformResetter resetter))
        {
            resetter.ResetTransform();
        }
    }
}
