using Controller;
using UnityEngine;

[RequireComponent(typeof(CreatureMover))]
public class PenguinWanderAI : MonoBehaviour
{
    [SerializeField] private float wanderRadius = 5f;
    [SerializeField] private float decisionInterval = 3f;

    private CreatureMover mover;
    private Vector3 targetPosition;
    private float decisionTimer;

    private void Awake()
    {
        mover = GetComponent<CreatureMover>();
        ChooseNewTarget();
    }

    private void Update()
    {
        decisionTimer -= Time.deltaTime;
        Vector3 toTarget = targetPosition - transform.position;
        toTarget.y = 0f;

        // Si proche de la cible ou temps écoulé → nouvelle destination
        if (toTarget.magnitude < 1f || decisionTimer <= 0f)
        {
            ChooseNewTarget();
        }

        Vector2 input = new Vector2(toTarget.x, toTarget.z).normalized;
        mover.SetInput(input, targetPosition, false, false);
    }

    private void ChooseNewTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        targetPosition = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
        decisionTimer = decisionInterval + Random.Range(-1f, 1f); // un peu de variation
    }
}
