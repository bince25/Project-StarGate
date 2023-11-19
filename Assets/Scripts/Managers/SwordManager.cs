using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwordManager : MonoBehaviour

{
    public static SwordManager Instance;
    public GameObject[] swords;
    public GameObject playerSword = null;
    public Slider swordDurabilitySlider;
    public TextMeshProUGUI textMeshProUGUI;

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
    public void UpdateDurabilityUI()
    {
        textMeshProUGUI.text = PlayerController.Instance.playersSword.name.ToString();
        swordDurabilitySlider.value = PlayerController.Instance.playersSword.GetComponent<SwordController>().durability;
    }

}
