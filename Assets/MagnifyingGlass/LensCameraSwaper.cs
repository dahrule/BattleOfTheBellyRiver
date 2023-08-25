using UnityEngine;

public class LensCameraSwaper : MonoBehaviour
{

    [SerializeField] Transform playerCamera;
    [SerializeField] Transform[] lensCameras;
    [SerializeField] float directionThreshold = 0.8f;


    // Update is called once per frame
    void Update()
    {
        foreach(Transform camera in lensCameras)
        {
            float dotProduct = Vector3.Dot(playerCamera.forward, camera.forward);

            if (dotProduct > directionThreshold)
            {
                camera.gameObject.SetActive(true);
            }
            else
            {
                camera.gameObject.SetActive(false);
            }
        }
        
    }
}
