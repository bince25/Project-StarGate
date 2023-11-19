using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEnemyController : MonoBehaviour
{
    public static BossEnemyController Instance;
    public int totalLegs = 4;
    public int legsDestroyed = 0;
    public float bossHealth = 100f;
    public GameObject meteorPrefab;
    public GameObject particleSystemPrefab;

    public int killedPlayers = 0;

    public Transform[] legs;

    public bool isPhase1, isPhase2, isSwordsActivated;

    [SerializeField]
    private int totalMeteorThrow;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        // Initialize the boss with all legs intact
        totalMeteorThrow = 0;
        legsDestroyed = 0;
        isPhase1 = true;
    }

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (players.Length <= 0 || player.GetComponent<Player>().isDead || player.activeInHierarchy == false)
            {
                killedPlayers++;
            }
        }

        if (players.Length <= 0 || killedPlayers >= 2)
        {
            LoadLevel((Levels)SceneManager.GetActiveScene().buildIndex);
        }

        if (totalMeteorThrow >= 20 && isPhase1)
        {
            isPhase1 = false;
            isPhase2 = true;
        }

        // Check conditions to decide when to launch a meteor attack
        if (ShouldLaunchMeteor() && isPhase1)
        {
            LaunchMeteor();
        }
        else if (isPhase2)
        {
            StartCoroutine(ActivateLegSwordsForDuration(10f));
        }


    }
    public void LoadLevel(Levels level)
    {
        Debug.Log("Loading level " + (int)level);
        LoadManager.Instance.LoadSceneWithTransition(level);
    }

    bool ShouldLaunchMeteor()
    {
        // Implement your logic to determine when the boss should launch a meteor
        // For example, you could check a timer, health thresholds, or other conditions
        // Here, we'll just randomly decide to launch a meteor for demonstration purposes
        return Random.Range(0f, 1f) < 0.005f; // Adjust the probability as needed
    }

    void LaunchMeteor()
    {
        // Instantiate a meteor and set its target position

        Vector3 playerPos = GetRandomPlayerPosition();

        GameObject particleSystemObject = Instantiate(particleSystemPrefab, playerPos, Quaternion.identity);
        ParticleSystem particleSystem = particleSystemObject.GetComponent<ParticleSystem>();

        ParticleSystemStoppedListener listener = particleSystemObject.AddComponent<ParticleSystemStoppedListener>();
        listener.Initialize(particleSystem);

        // Spawn enemy after the particle system stops
        listener.OnParticleSystemStoppedCallback += () =>
        {
            GameObject meteor = Instantiate(meteorPrefab, playerPos, Quaternion.identity);
            totalMeteorThrow++;
        };



        // Implement any additional logic for the meteor, such as setting its speed or damage
    }

    Vector3 GetRandomPlayerPosition()
    {
        // Get all player GameObjects in the scene
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 0)
        {
            // Choose a random player and return its position
            GameObject randomPlayer = players[Random.Range(0, players.Length)];
            return randomPlayer.transform.position;
        }

        // If no players are found, return a default position
        return Vector3.zero;
    }
    IEnumerator ActivateLegSwordsForDuration(float duration)
    {
        isSwordsActivated = true; // Set the flag to prevent multiple activations

        // Iterate through all legs and activate their swords
        foreach (Transform leg in legs)
        {
            if (leg == null)
            {
                continue;
            }
            Transform sword = leg.GetChild(0); // Assuming "Sword" is the name of the sword GameObject
            if (sword != null)
            {
                sword.gameObject.SetActive(true);
            }
        }

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Deactivate swords after the duration
        DeactivateLegSwords();

        // Transition back to Phase 1
        totalMeteorThrow = 0;
        isPhase1 = true;
        isPhase2 = false;
    }
    public void DamageBoss(float damage)
    {
        // Reduce boss health
        bossHealth -= damage;

        // Check if the boss is defeated
        if (bossHealth <= 0)
        {
            DefeatBoss();
        }
    }
    void DeactivateLegSwords()
    {
        // Iterate through all legs and deactivate their swords
        foreach (Transform leg in legs)
        {
            if (leg == null)
            {
                continue;
            }
            Transform sword = leg.GetChild(0); // Assuming "Sword" is the name of the sword GameObject
            if (sword != null)
            {
                sword.gameObject.SetActive(false);
            }
        }
    }

    void DefeatBoss()
    {
        // Implement boss defeat logic, e.g., play an animation, show a victory screen, etc.
        Debug.Log("Boss defeated!");
        LoadLevel((Levels)SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void DestroyLeg()
    {
        // This method is called when a player destroys one of the boss's legs
        legsDestroyed++;

        // Check if all legs are destroyed
        if (legsDestroyed >= totalLegs)
        {
            DefeatBoss();
        }
    }
}
