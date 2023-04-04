using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(Animator))]
public class DeerAIController : MonoBehaviour
{
    [Header("Behavior triggers")]
    [SerializeField] float _elusiveDistance = 5f;
    [SerializeField] float _alertDistance = 10f;
    [SerializeField] float _foragingDistance = 1.5f;

    [Header("External Dependecies")]
    [SerializeField] WayPointPath _path;
    [SerializeField] Transform _player;

     Mover _mover;
     Animator _animController;

    float _waypointTolerance = 0.5f; 
    public int _currentWaypointIndex = 0;
    bool _grazingCoroutineRunning=false;

    IEnumerator grazing;

    private void Awake()
    {
        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player").transform;
        if (_path == null) _path = GameObject.FindObjectOfType<WayPointPath>(); //To do: Change to create a path gameobject
        _mover = GetComponent<Mover>();
        _animController= GetComponent<Animator>();
    }
    private void Start()
    {
        _waypointTolerance = _foragingDistance;
        ElusiveBehavior();
    }
    void Update()
    {
        if (!AtWaypoint()) return;

        if (AtEndOfPath())
        {
            FadeBehavior();
            return;
        }
            
        /*if (DistanceToPlayer() < _elusiveDistance)
        {
            StopCoroutine(GrazingCoroutine());
            _currentWaypointIndex = _path.GetNextIndex(_currentWaypointIndex);
            ElusiveBehavior();
        }*/
        //else if (DistanceToPlayer() < _alertDistance) AlertBehavior();
        //else GrazingBehavior(); 
    }
    private void GrazingBehavior()
    {
        //To Delete
        if (gameObject.TryGetComponent<Renderer>(out Renderer component))
            component.material.color = Color.blue;

        if (_grazingCoroutineRunning) return;

        StartCoroutine(GrazingRoutine()); 
    }
    private IEnumerator GrazingRoutine()
    {
        Debug.Log("Coroutine started");
        
        _grazingCoroutineRunning = true;

        int timeToMove = Random.Range(10, 20);
        yield return new WaitForSeconds(timeToMove);

        float minDistanceFromSelf = 0.2f;
        
        float x = Random.Range(minDistanceFromSelf, _foragingDistance);
        float z = Random.Range(minDistanceFromSelf, _foragingDistance);
        Vector3 foragingDestination = new Vector3(x, z, z);
        Debug.Log("fORAGIING DESTINATION: " + foragingDestination);
        _mover.MoveTo(foragingDestination);
        yield return new WaitForSeconds(timeToMove);
        _grazingCoroutineRunning = false;
        
        Debug.Log("Coroutine ended");
    }
    private void ElusiveBehavior()
    {
        if (gameObject.TryGetComponent<Renderer>(out Renderer component))
            component.material.color = Color.red;//To Dele
       
        Transform target = _path.GetWaypoint(_currentWaypointIndex);
        _mover.MoveTo(target);        
    }
    private void AlertBehavior()
    {
        if (gameObject.TryGetComponent<Renderer>(out Renderer component))
            component.material.color = Color.yellow;//To Delete
        //set alert anim state
         _animController.SetTrigger("Alert");
    }
    private void FadeBehavior()
    {
        if (gameObject.TryGetComponent<Renderer>(out Renderer component))
            component.material.color = Color.green;//To Delete
    }
    private float DistanceToPlayer()
    {
        return Vector3.Distance(_player.position, this.transform.position);
    }
    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, _path.GetWaypointPos(_currentWaypointIndex));
        return distanceToWaypoint<_waypointTolerance;
    }
    private bool AtEndOfPath()
    {
        return _currentWaypointIndex == _path.NumberOfWaypoints-1;
    }
    private void OnDrawGizmos()
    {
        //Shows elusive and alert radial distances on the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,_elusiveDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _alertDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _foragingDistance);
    }
}
