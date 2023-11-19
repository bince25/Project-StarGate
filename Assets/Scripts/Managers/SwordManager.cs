using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : MonoBehaviour

{
    public static SwordManager Instance;
    public GameObject[] swords;
    public GameObject playerSword = null;
    // Start is called before the first frame update

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void BindPlayerSword(GameObject playerSword)
    {
        this.playerSword = playerSword;
    }

    public GameObject GetPlayerSword()
    {
        return playerSword;
    }
}
