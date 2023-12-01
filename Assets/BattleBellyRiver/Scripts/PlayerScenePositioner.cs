using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Sets the player's position and rotation according to the scene he is coming from.
/// scene 0 is the hub (tepee camp).
/// </summary>
public class PlayerScenePositioner : MonoBehaviour
{
    [Tooltip("Player's landing points when coming from scene n.")]
    [SerializeField] Transform pointFrom1;
    [SerializeField] Transform pointFrom2; 
    [SerializeField] Transform pointFrom3;
    [SerializeField] Transform pointFrom4;
    
    [SerializeField] GameObject player;

    //[SerializeField] Transform cameraOffset;
    //[SerializeField] Transform target;

    [SerializeField] PlayerPositioner playerController;
    int previousSceneIndex;

    private void Awake()
    {
        if(player==null) player = GameObject.FindGameObjectWithTag("Player");

        playerController = player.GetComponent<PlayerPositioner>();
        
        previousSceneIndex = SceneTransitionManager.LastSceneIndex;

        playerController._resetTransform = GetNewPosition();
    }
    void Start()
    {
        
        //playerController.ResetPosition();
    }

    private Transform GetNewPosition()
    {
        switch (previousSceneIndex)
        {
            case 0:
                //player.SetPositionAndRotation(pointFrom1.position, pointFrom1.rotation);

                return pointFrom1;
            case 1:
                //player.SetPositionAndRotation(pointFrom1.position, pointFrom1.rotation);

                return pointFrom2;
            case 2:
                //player.SetPositionAndRotation(pointFrom2.position, pointFrom2.rotation);
                return pointFrom2;
            case 3:
                //player.SetPositionAndRotation(pointFrom3.position, pointFrom3.rotation);
                return pointFrom3;
            case 4:
                //player.SetPositionAndRotation(pointFrom4.position, pointFrom4.rotation);
                return pointFrom4;
            default:
                return pointFrom4;
        }
    }

    private void SetCameraOrientation()
    {
        Vector3 camRotation = new Vector3(0,180,0);
        //Camera.main.transform.localEulerAngles=camRotation;

        //Camera.main.transform.LookAt(cameraOffset);

        //cameraOffset.LookAt(target);

        //cameraOffset.localEulerAngles = camRotation;
       


    }
}
