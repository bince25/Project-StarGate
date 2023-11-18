using System.Collections;
using UnityEngine;

public class ParticleSystemStoppedListener : MonoBehaviour
{
    private ParticleSystem particleSystem;
    public System.Action OnParticleSystemStoppedCallback;
    bool readyToDestroy = false;
    public void Initialize(ParticleSystem system)
    {
        particleSystem = system;
    }

    void Start()
    {
        readyToDestroy = false;
        StartCoroutine(DestroyAfterDelay(2.5f));
    }


    void Update()
    {
        if (!particleSystem.IsAlive() || readyToDestroy)
        {
            OnParticleSystemStoppedCallback?.Invoke();
            Destroy(gameObject); // Remove this listener component when no longer needed
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        readyToDestroy = true;
    }
}
