using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMController : MonoBehaviour
{
    public GameObject[] charges;
    bool canUse = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeSwordsDirection();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ActivateCharge();
        }
    }
    void ChangeSwordsDirection()
    {
        if (charges.Length > 0 && canUse)
        {
            GameObject[] swords = GameObject.FindGameObjectsWithTag("Sword");
            foreach (GameObject sword in swords)
            {
                sword.GetComponent<SwordController>().ReverseRotation();
            }

            // Disable the last item in charges
            DisableCharge();
        }
    }

    void DisableCharge()
    {
        if (charges.Length > 0)
        {
            for (int i = charges.Length - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    canUse = false;
                }
                if (charges[i].activeSelf)
                {
                    charges[i].SetActive(false);
                    break;
                }

            }
        }
    }
    void ActivateCharge()
    {
        if (charges.Length > 0)
        {
            for (int i = 0; i < charges.Length; i++)
            {
                if (!charges[i].activeSelf)
                {
                    charges[i].SetActive(true);
                    canUse = true;
                    break;
                }
            }
        }
    }
}
