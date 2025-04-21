using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BotMove : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Animator animator;

    public GameObject losePanel; // Assign your UI panel in the Inspector
    private bool hasLost = false;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        StartCoroutine(FollowNearestAnimal());
    }

    IEnumerator FollowNearestAnimal()
    {
        while (!hasLost)
        {
            GameObject nearestAnimal = FindNearestAnimal();

            if (nearestAnimal != null)
            {
                navAgent.SetDestination(nearestAnimal.transform.position);
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    GameObject FindNearestAnimal()
    {
        GameObject[] animals = GameObject.FindGameObjectsWithTag("animal");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject animal in animals)
        {
            float dist = Vector3.Distance(transform.position, animal.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = animal;
            }
        }

        return closest;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasLost && other.CompareTag("animal"))
        {
            hasLost = true;

            // Stop bot
            navAgent.isStopped = true;

            // Play animation
            if (animator != null)
            {
                animator.SetTrigger("Eat"); // Make sure your Animator has this trigger
            }

            // Hide animal
            Destroy(other.gameObject);

            // Show losing UI after a short delay (for animation)
            StartCoroutine(ShowLoseUI());
        }
    }

    IEnumerator ShowLoseUI()
    {
        yield return new WaitForSeconds(1.5f); // adjust based on animation length
        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }
    }
}
