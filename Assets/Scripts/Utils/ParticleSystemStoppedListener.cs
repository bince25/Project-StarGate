using UnityEngine;

public class ParticleSystemStoppedListener : MonoBehaviour
{
    private SpawnManager spawnManager;
    private ParticleSystem particleSystem;
    public System.Action OnParticleSystemStoppedCallback;

    public void Initialize(SpawnManager controller, ParticleSystem system)
    {
        spawnManager = controller;
        particleSystem = system;
    }

    void Update()
    {
        if (!particleSystem.IsAlive())
        {
            OnParticleSystemStoppedCallback?.Invoke();
            Destroy(gameObject); // Remove this listener component when no longer needed
        }
    }
}
