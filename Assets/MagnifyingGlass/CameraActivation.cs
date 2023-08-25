using UnityEngine;

public class CameraActivation : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject camera1;
    public GameObject camera2;

    private void Update()
    {
        // Calculate the dot product between the player's forward vector and the camera's forward vector
        float dot1 = Vector3.Dot(playerCamera.transform.forward, camera1.transform.forward);
        float dot2 = Vector3.Dot(playerCamera.transform.forward, camera2.transform.forward);

        // Activate the camera that has a higher dot product, indicating a closer direction match
        if (dot1 > dot2)
        {
            camera1.SetActive(true);
            camera2.SetActive(false);
        }
        else
        {
            camera1.SetActive(false);
            camera2.SetActive(true);
        }
    }
}

