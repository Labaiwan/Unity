using Invector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 子弹的速度
    public float speed = 30f;
    public string tagName;
    public int Atk;
    private void Start()
    {
        // 设置子弹的初始速度
        GetComponent<Rigidbody>().velocity = transform.forward * speed;

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(tagName))
        {
            vHealthController damage = collision.gameObject.GetComponent<vHealthController>();
            if (damage.currentHealth > 0)
            {
                damage.TakeDamage(new vDamage(50));
            }
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }

    }

}
