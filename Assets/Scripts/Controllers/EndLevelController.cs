using UnityEngine;

public class EndLevelController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EndLevel();
        }
    }

    public void EndLevel()
    {
        if (ResourceManager.Instance.gearCount > 0)
        {
            LoadManager.Instance.LoadSceneWithTransition(Levels.Level2);
        }
    }
}