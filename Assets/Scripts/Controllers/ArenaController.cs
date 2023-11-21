using UnityEngine;

public class ArenaController : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("Arena"))
        {
            if (PlayerPrefs.GetInt("Arena") == 1)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}