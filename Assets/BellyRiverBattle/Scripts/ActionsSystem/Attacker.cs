using UnityEngine;
using UnityEngine.AI;

public class Attacker : MonoBehaviour,IAction
{
    [SerializeField] float _attackRange = 2f;
    [SerializeField] float _timeBetweenAttacks = 3f;
  
    public Transform Target { get;  set; }

    float _timeSinceLastAttack = 0;
    readonly int _attackHash = Animator.StringToHash("attack");
    readonly int _stopAttackHash = Animator.StringToHash("stopAttack");
    NavmeshAgentMover _mover;
    Animator _animator;
    NavMeshAgent _agent;

    private void Awake()
    {
        _mover=GetComponent<NavmeshAgentMover>();
        _animator=GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _timeSinceLastAttack+= Time.deltaTime;

        if (Target == null) return;

        if (!GetIsInRange())
        {
            _mover.MoveTo(Target.position);
        }
        else
        {
            _mover.Cancel();
            AttackBehaviour();
        }
    }

    private void AttackBehaviour()
    {
        //transform.LookAt(Target);
        if(_timeSinceLastAttack>_timeBetweenAttacks)
        {
            _animator.ResetTrigger(_stopAttackHash);
            _animator.SetTrigger(_attackHash);
            _timeSinceLastAttack = 0;
        }
    }

    private bool GetIsInRange()
    {
        return Vector3.Distance(transform.position, Target.position) <= _attackRange;
    }

    public void Attack(CombatTarget combatTarget)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        _agent.autoBraking = false;
        Target = combatTarget.transform;
        _animator.runtimeAnimatorController = combatTarget._attackOverride;
    }

    public void Cancel()
    {
        _animator.ResetTrigger(_attackHash);
        _animator.SetTrigger(_stopAttackHash);
        _agent.autoBraking = true;
        Target = null;
    }

    //Animation event callback
    public void Hit()
    {
        if (Target == null) return;
        if (Target.TryGetComponent<DissolvingController>(out DissolvingController targetDissolve))
            targetDissolve.Dissolve();
    }

    public void StartAction(Vector3 destination)
    {
        throw new System.NotImplementedException();
    }
}
