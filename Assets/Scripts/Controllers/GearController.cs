using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void GetCollected()
    {
        ResourceManager.Instance.AddGear(1);
        PoolManager.Instance.ReturnObject("Gear", gameObject);
    }
}
