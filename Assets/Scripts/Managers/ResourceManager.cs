using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public GameObject itemNotification;
    public int gearCount = 0;
    public static ResourceManager Instance;

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
    }

    public void UseGear(int amount)
    {
        gearCount -= amount;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
