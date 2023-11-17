using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearController : MonoBehaviour
{

    public void GetCollected()
    {
        SoundManager.Instance.PlayGearCollectSound();
        ResourceManager.Instance.AddGear(1);
        PoolManager.Instance.ReturnObject("Gear", gameObject, 0.05f);
    }
}
