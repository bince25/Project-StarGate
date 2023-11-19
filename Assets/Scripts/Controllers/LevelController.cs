using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{

    public AudioClip levelMusic;
    public AudioSource audioSource;

    public int requiredGearCount = 10;
    public GameObject[] unitsToBeKilled = null;
    public TrapController[] traps = null;

    public Levels level;

    void Start()
    {
        audioSource = SoundManager.Instance.PlayMusic(levelMusic, 1.5f);

        switch (level)
        {
            case Levels.Level1:
                PlayLoopingBounceAnimation();
                break;
            case Levels.Level2:
                break;
        }

    }

    void Update()
    {
        Debug.Log(unitsToBeKilled);
        if (unitsToBeKilled != null && unitsToBeKilled.Length > 0)
        {
            foreach (GameObject unit in unitsToBeKilled)
            {
                if (unit != null)
                {
                    return;
                }
            }
            foreach (TrapController trap in traps)
            {
                trap.isActive = false;
                trap.disabled = true;
                trap.animator.SetBool("isAlwaysActive", false);
                trap.animator.SetBool("isActivated", false);
                StartCoroutine(trap.DeactivateTrap());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.StopMusic(audioSource, 0.5f);
            StartCoroutine(EndLevelCoroutine());
        }
    }

    public void EndLevel()
    {
        if (ResourceManager.Instance.gearCount >= requiredGearCount)
        {
            LoadManager.Instance.LoadSceneWithTransition((Levels)(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    IEnumerator EndLevelCoroutine()
    {
        switch (level)
        {
            case Levels.Level1:
                if (ResourceManager.Instance.gearCount >= requiredGearCount)
                {
                    yield return new WaitForSeconds(.7f);
                    LoadManager.Instance.LoadSceneWithTransition((Levels)(SceneManager.GetActiveScene().buildIndex + 1));
                }
                break;
            case Levels.Level2:
                yield return new WaitForSeconds(.7f);
                LoadManager.Instance.LoadSceneWithTransition((Levels)(SceneManager.GetActiveScene().buildIndex + 1));
                break;
        }
    }

    void PlayLoopingBounceAnimation()
    {
        // Use DoTween to create a looping bounce animation
        Sequence bounceSequence = DOTween.Sequence();

        bounceSequence.Append(transform.DOScale(new Vector3(5.5f, 5.5f, 1.0f), 0.5f)
            .SetEase(Ease.OutQuad));

        bounceSequence.Append(transform.DOScale(new Vector3(5f, 5f, 1.0f), 0.5f)
            .SetEase(Ease.InQuad));

        // Set the animation to loop infinitely
        bounceSequence.SetLoops(-1);
    }
}