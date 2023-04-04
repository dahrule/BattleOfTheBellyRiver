
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using System.Collections;
using System;
/// <summary>
///The context of the deer's state machine.
/// *Provides access to objects in the scene necessary for the states to work.
/// *Exposes fields in the editor to adapt the behavior of the states.
/// *The initial state is set here.
/// *The current state is updated every frame within the parent class.
/// </summary>
[RequireComponent(typeof(Mover), typeof(Animator), typeof(SkinnedMeshDissolver))]
public class DeerStateMachine : StateMachine
{
    #region Scene objects dependencies
    [field: SerializeField] public Logger Logger { private set; get; } //Logs the current running state for debugging purposes.
    [field: SerializeField] public WayPointPath DeerPath { private set; get; }
    [field: SerializeField] public WayPointPath GrazingPath { private set; get; }
    [field: SerializeField] public Transform Player { private set; get; }
    [field: SerializeField] public Rig RigControl { private set; get; }
    [field: SerializeField] public GameObject LookAtTarget { private set; get; }
    [field: SerializeField] public Transform DeerHead { private set; get; }
    [field: SerializeField] public SkinnedMeshDissolver SkinMeshDissolver { private set; get; }
    public Mover Mover { private set; get; }
    public Animator AnimController { private set; get; }
    #endregion

    #region Serializable member fields
    [Header("Behavior")]

    [Tooltip("Deer enters flee state when player crosses this distance threshold. Radial distance in meters. ")]
    [SerializeField] float _elusiveRadius = 5f;

    [Tooltip("Deer enters alert state when player crosses this distance threshold. Radial distance in meters.")]
    [SerializeField] float _alertRadius = 10f;

    [Tooltip("How fast the deer turns its head to look at player under stress.")]
    [SerializeField] [Range(1f, 10f)] float _headRotSpeed_stress = 5f;

    [Tooltip("How fast the deer turns its head to look at player when relaxed.")]
    [SerializeField] [Range(1f, 10f)] float _headRotSpeed_relax= 5f;
    #endregion

    #region Properties
    public float ElusiveDistance { get { return _elusiveRadius;}}
    public float AlertDistance { get { return _alertRadius;}}
    public float HeadRotationSpeed_stress { get { return _headRotSpeed_stress; } }
    public float HeadRotationSpeed_relax { get { return _headRotSpeed_relax; } }
    public int CurrentWaypointindex { set; get; }
    #endregion

    #region Events
    [Header("Events")]

    [Tooltip("Event is invoked each time the deer reaches a new waypoint in the path.")]
    public UnityEvent OnPathWaypointReached;
    #endregion

    private void Awake()
    {
        GetDependecies();
    }
    private void Start()
    {
        //Initialize member variables
        CurrentWaypointindex = 0;
        if (RigControl) RigControl.weight = 0; //weight=0 completely deactivates procedural animations under the object with a Rig component.

        //Set the initial state.
        Switch(new DeerGrazingState(this, GrazingPath));
        //Switch(new DeerTestState(this)); //ToDelete
        //Switch(new DeerAlertState(this,60)); //ToDelete
    }
    private void GetDependecies()
    {
        if (AnimController == null) AnimController = GetComponent<Animator>();
        if (Mover == null) Mover = GetComponent<Mover>();
        if (RigControl == null) RigControl = GetComponentInChildren<Rig>();
        if (Player == null)
        {
            try { Player = GameObject.FindGameObjectWithTag("Player").transform; }
            catch 
            {
               object message="No object with Tag: Player was found";
               string hexColor = "#" + ColorUtility.ToHtmlStringRGB(Color.red);
               Debug.Log($"<color={hexColor}> Error: {message}</color>");
            }
        }
        if (SkinMeshDissolver == null) SkinMeshDissolver = GetComponent<SkinnedMeshDissolver>();
        if (DeerPath == null)
        {
            return;//TODO
        }
        if (GrazingPath == null)
        {
            return;//TODO
        }
        if (LookAtTarget == null)
        {
            return;//TODO
        }
    }
    public float DistanceToPlayer()
    {
        return Vector3.Distance(Player.position, this.transform.position);
    }
    private void OnDrawGizmos()
    {
        //Shows elusive and alert radial distances on the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _elusiveRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _alertRadius);
    }

}
