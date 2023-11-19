using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public GameObject itemNotification;
    public int gearCount = 0;
    public static ResourceManager Instance;

    [SerializeField]
    private TMP_Text gearCountText;

    public int requiredGearCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        itemNotification.SetActive(false);
    }
    public void ActivationOfItemNotification(bool active)
    {
        itemNotification.SetActive(active);
    }

    public void AddGear(int amount)
    {
        gearCount += amount;
        UpdateGearCountUI();
    }

    public void UseGear(int amount)
    {
        gearCount -= amount;
        UpdateGearCountUI();
    }

    public void UpdateGearCountUI()
    {
        gearCountText.text = gearCount.ToString() + "/" + requiredGearCount.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateGearCountUI();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
