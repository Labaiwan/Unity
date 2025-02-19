using Invector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int atk;
    private GameObject player;
    public Transform[] DamagePoint;
    private GameObject wall;

    void Start()
    {
        player = GameObject.Find("Player");
        wall = GameObject.Find("DefendTower");
        if (wall == null)
        {
            return;
        }
       
    }
    public void AtkEvent()
    {
        for (int i = 0; i < DamagePoint.Length; i++)
        {
            AtkPosition(DamagePoint[i]);
        }
    }

    private void AtkPosition(Transform attackPoint)
    {
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, 1, 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Wall"));
        vHealthController damage;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject == player)
            {
                damage = player.GetComponent<vHealthController>();
            }
            else
            {
                if (wall != null)
                {
                    damage = wall.GetComponent<vHealthController>();
                }
                else 
                {
                    continue;
                }    
                
            }

            if (damage != null && damage.currentHealth > 0)
            {
                damage.TakeDamage(new vDamage(atk));
            }
        }
    }
}