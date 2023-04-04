
using UnityEngine;

/// <summary>
/// Displays a connected path in the editor built out of waypoints.
/// </summary>
public class WayPointPath : MonoBehaviour
{
    [SerializeField] bool _isLoop;

    public int NumberOfWaypoints { private set; get; }

    private void Awake()
    {
        NumberOfWaypoints = transform.childCount;
    }

    private void OnDrawGizmos()
    {
        const float waypointGizmoRadius =0.3f;

        for(int i=0; i< transform.childCount; i++)
        {
            int j=GetNextIndex(i);

            //Draw spheres to show waypoints in the editor.
            Gizmos.DrawSphere(GetWaypointPos(i), waypointGizmoRadius);

            //Draw Lines to connect waypoints in the editor.
            Gizmos.DrawLine(GetWaypointPos(i),GetWaypointPos(j));
        }
    }

    public int GetNextIndex(int index)
    {
        if (index + 1 == transform.childCount) return _isLoop?0:index;
        else return  index + 1;
    }

    public Transform GetWaypoint(int index)
    {
        return transform.GetChild(index);
    }

   public Vector3 GetWaypointPos(int index)
    {
        return transform.GetChild(index).position;
    }
}
