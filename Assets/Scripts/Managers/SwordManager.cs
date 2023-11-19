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
        Debug.Log(PlayerController.Instance);
        if (PlayerController.Instance.playersSword == null)
        {
            return;
        }

        if (textMeshProUGUI == null || swordDurabilitySlider == null)
        {
            textMeshProUGUI = GameObject.FindGameObjectWithTag("SwordName").GetComponent<TextMeshProUGUI>();
            swordDurabilitySlider = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
        }

        textMeshProUGUI.text = PlayerController.Instance.playersSword.name.ToString();
        swordDurabilitySlider.value = PlayerController.Instance.playersSword.GetComponent<SwordController>().durability;
    }

}
