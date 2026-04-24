using UnityEngine;
using System.Collections;
using System;

public class Samurai : MonoBehaviour
{
    public event Action<Vector3> OnAttackStarted;

    //애니메이션 Parameter (int State)
    //State가 의미하는 것 : 0- Idle, 1- Attack, 2- Run
    [SerializeField] private float runSpeed = 3.5f;
    [SerializeField] private float stoppingDistance = 0.01f;
    [SerializeField] private bool runToCenterOnStart = true;
    [SerializeField] private float attackDuration = 0.45f;
    [SerializeField] private Transform slashSpawnPoint;

    private Animator animator;
    private Coroutine moveRoutine;
    private Coroutine attackRoutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (runToCenterOnStart)
        {
            moveRoutine = StartCoroutine(Move(Vector3.zero));
        }
    }

    public IEnumerator Move(Vector3 tpos)
    {
        SetRunAnimation();

        while (Vector3.Distance(transform.position, tpos) > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, tpos, runSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = tpos;
        SetIdleAnimation();
        moveRoutine = null;
    }

    public void MoveToCenter()
    {
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
        }

        moveRoutine = StartCoroutine(Move(Vector3.zero));
    }

    public void Attack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }

        attackRoutine = StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }

        SetAttackAnimation();

        Vector3 fxPosition = slashSpawnPoint != null ? slashSpawnPoint.position : transform.position;
        OnAttackStarted?.Invoke(fxPosition);

        yield return new WaitForSeconds(attackDuration);

        SetIdleAnimation();
        attackRoutine = null;
    }

    private void SetRunAnimation()
    {
        if (animator == null)
        {
            return;
        }

        animator.Play("Samurai_Run", 0, 0f);

        if (HasStateParameter())
        {
            animator.SetInteger("State", 2);
        }
    }

    private void SetIdleAnimation()
    {
        if (animator == null)
        {
            return;
        }

        animator.Play("Samurai_Idle", 0, 0f);

        if (HasStateParameter())
        {
            animator.SetInteger("State", 0);
        }
    }

    private void SetAttackAnimation()
    {
        if (animator == null)
        {
            return;
        }

        if (HasStateParameter())
        {
            animator.SetInteger("State", 1);
        }
    }

    private bool HasStateParameter()
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name == "State" && parameter.type == AnimatorControllerParameterType.Int)
            {
                return true;
            }
        }

        return false;
    }
}
