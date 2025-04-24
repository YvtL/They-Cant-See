using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CatchingAnimals : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Animator animator;

    public GameObject losePanel; // Assign in Inspector
    private GameObject currentTarget;

    public Material outlineMaterial;
    private Material originalMaterial;
    private Renderer animalRenderer;

    private int animalsEaten = 0;
    public int maxAnimalsAllowedToDie = 5;

    private bool gameOver = false;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (gameOver) return;

        currentTarget = FindNearestAnimal();

        if (currentTarget != null)
        {
            navAgent.SetDestination(currentTarget.transform.position);
        }
    }

    GameObject FindNearestAnimal()
    {
        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");
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

    // void OnTriggerEnter(Collider other)
    // {
    //     if (gameOver || !other.CompareTag("Animal")) return;

    //     animalsEaten++;
    //     Destroy(other.gameObject); // Remove the animal

    //     if (animator != null)
    //     {
    //         animator.SetTrigger("Eat"); // Trigger eating animation
    //     }

    //     if (animalsEaten >= maxAnimalsAllowedToDie)
    //     {
    //         gameOver = true;
    //         navAgent.isStopped = true;
    //         StartCoroutine(ShowLoseUI());
    //     }
    // }

    void OnTriggerEnter(Collider other)
    {
    if (gameOver || !other.CompareTag("Animal")) return;

    Debug.Log("Monster touched an animal: " + other.gameObject.name);

    animalsEaten++;
    Destroy(other.gameObject); // Remove the animal

    if (animator != null)
    {
        animator.SetTrigger("Eat"); // Trigger eating animation
    }

    if (animalsEaten >= maxAnimalsAllowedToDie)
    {
        gameOver = true;
        navAgent.isStopped = true;
        StartCoroutine(ShowLoseUI());
    }
    }


    // IEnumerator ShowLoseUI()
    // {
    //     yield return new WaitForSeconds(1.5f); // Optional delay for animation
    //     if (losePanel != null)
    //     {
    //         losePanel.SetActive(true);
    //     }

    //     Time.timeScale = 0f; // Optional: pause game
    // }

    IEnumerator ShowLoseUI()
    {
        yield return new WaitForSeconds(1.5f); // Optional delay for animation
        SceneManager.LoadScene("Losing Scene");
    }


    void TargetAnimal(GameObject animal)
    {
        animalRenderer = animal.GetComponent<Renderer>();
        originalMaterial = animalRenderer.material;
        animalRenderer.material = outlineMaterial;
    }

    void ClearTarget()
    {
        if (animalRenderer != null)
            animalRenderer.material = originalMaterial;
    }
}
