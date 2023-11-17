using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private bool isReflected = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall")) ReturnToPool();
    }

    public void Reflect()
    {
        if (isReflected)
        {
            return;
        }
        isReflected = true;

        GetComponent<Rigidbody2D>().velocity = -GetComponent<Rigidbody2D>().velocity;
    }

    public void ReturnToPool()
    {
        PoolManager.Instance.ReturnObject("Bullet", gameObject);
    }
}
