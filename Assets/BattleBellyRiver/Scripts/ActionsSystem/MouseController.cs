using UnityEngine;

/// <summary>
/// Moves the navmesh agent by clicking on screen.
/// </summary>
public class MouseController : MonoBehaviour
{
    [SerializeField]GameObject followCameraPrefab;
    public GameObject followCameraObject;

    private void Awake()
    {
        //Try Find the main camera
       GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
       followCameraObject = mainCamera;
       if (mainCamera == null)
       {
            CreateFollowCamera();
       }
    }

    private void OnEnable()
    {
        if (followCameraObject == null) return;
        followCameraObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (followCameraObject == null) return;
        followCameraObject.SetActive(false);
    }

    void Update()
    {
       if(InteractWithCombat()) return;
        if(InteractWithMovement()) return;
    }

    private bool InteractWithCombat()
    {
        RaycastHit[] hits=Physics.RaycastAll(GetMouseRay());
        foreach(RaycastHit hit in hits)
        {
            CombatTarget target = hit.transform.GetComponent<CombatTarget>();
            if (target == null) continue;

            if(Input.GetMouseButtonDown(0))
            {
               GetComponent<Attacker>().Attack(target);
            }
            return true;
        }
        return false;
    }

    private bool InteractWithMovement()
    {
        bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);
        if(hasHit)
        {
            if(Input.GetMouseButtonDown(0))
            {
                GetComponent<NavmeshAgentMover>().StartMoveAction(hit.point);
            }
            return true;
        }
        return false;
    }

    private static Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    void CreateFollowCamera()
    {
        // Main camera not found, so create a follow camera
        followCameraObject = Instantiate(followCameraPrefab);
        followCameraObject.transform.parent = transform;
        // Reset the transform of the follow camera object
        followCameraObject.transform.SetLocalPositionAndRotation(Vector3.zero,Quaternion.identity);
        followCameraObject.transform.localScale = Vector3.one;

       // Set the follow camera's target to the parent of the follow camera object
        FollowCamera followCamera = followCameraObject.GetComponent<FollowCamera>();
        followCamera.Target=transform;//TODO: check this line

        // Set the follow camera's position and rotation relative to the target
        //followCamera._distance = followCameraDistance;
        //followCamera._height = followCameraHeight;

        // Make sure the follow camera object is named appropriately
        followCameraObject.name = "Follow Camera";
    }
}
