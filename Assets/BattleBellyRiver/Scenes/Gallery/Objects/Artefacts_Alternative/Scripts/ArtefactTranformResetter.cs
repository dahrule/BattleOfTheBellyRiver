using UnityEngine;
using System.Collections;

/// <summary>
/// Resets the position and rotation of the artifacts after a certain interval when they are not selected/held.
/// </summary>
public class ArtefactTranformResetter : MonoBehaviour
{
    [SerializeField] ArtefactGrabInteractable[] objectsToReset;
    public float resetTimeInSeconds = 30f;

    private IEnumerator Start()
    {
        if (objectsToReset == null || objectsToReset.Length <= 0) yield break;

        WaitForSeconds waitTime = new(resetTimeInSeconds);

        while (true)
        {
            yield return waitTime;

            foreach (ArtefactGrabInteractable obj in objectsToReset)
            {
                if (obj == null) continue;
                if (!obj.isSelected)
                {
                    obj.ResetTransform();
                }
            }
        }
    }
}

