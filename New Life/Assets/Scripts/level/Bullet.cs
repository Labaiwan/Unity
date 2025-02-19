using Invector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // �ӵ����ٶ�
    public float speed = 30f;
    public string tagName;
    public int Atk;
    private void Start()
    {
        // �����ӵ��ĳ�ʼ�ٶ�
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
