using UnityEngine;

public class BehaviourModels : MonoBehaviour
{
    public float detectionRadius = 10f; // Радиус обнаружения цели
    public float escapeRadius = 3f; // Радиус избегания

    public GameObject target; // Цель для преследования
    public GameObject dangerousObject; // Объект, которого избегаем

    private Wander wanderBehavior;
    private Arrive arriveBehavior;
    private Leave leaveBehavior;

    private bool isArriving = false;  // Преследование цели
    private bool isLeaving = false;   // Избегание опасного объекта
    private bool wasArriving = false; // До избегания объект преследовал
    private bool wasWandering = true; // До избегания объект блуждал
    private Vector3 lastPosition;

    private void Awake()
    {
        wanderBehavior = GetComponent<Wander>();
        arriveBehavior = GetComponent<Arrive>();
        leaveBehavior = GetComponent<Leave>();

        // Начинаем с блуждания
        SetWanderState();
    }

    private void Update()
    {
        CheckDangerousObject();
        CheckTargetPresence();

        if (isLeaving)
        {
            SetLeaveState();
        }
        else if (isArriving)
        {
            SetArriveState();
        }
        else
        {
            SetWanderState();
        }
    }

    private void SetWanderState()
    {
        if (!wanderBehavior.enabled) // Только при первом переходе
        {
            lastPosition = transform.position;
            wanderBehavior.startPosition = lastPosition;
            wanderBehavior.ResetWander();
        }

        wanderBehavior.enabled = true;
        arriveBehavior.enabled = false;
        leaveBehavior.enabled = false;
    }

    private void SetArriveState()
    {
        wanderBehavior.enabled = false;
        arriveBehavior.enabled = true;
        leaveBehavior.enabled = false;
        arriveBehavior.target = target;
    }

    private void SetLeaveState()
    {
        wasArriving = isArriving;
        wasWandering = !isArriving;

        wanderBehavior.enabled = false;
        arriveBehavior.enabled = false;
        leaveBehavior.enabled = true;
        leaveBehavior.target = dangerousObject;
    }

    private void CheckTargetPresence()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        isArriving = distanceToTarget < detectionRadius && !isLeaving;
    }

    private void CheckDangerousObject()
    {
        if (dangerousObject == null) return;

        float distanceToDangerous = Vector3.Distance(transform.position, dangerousObject.transform.position);
        isLeaving = distanceToDangerous < escapeRadius;
    }
}
