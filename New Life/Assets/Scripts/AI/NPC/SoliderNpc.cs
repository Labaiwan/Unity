using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoliderNpc : MonoBehaviour
{
    public turretObj turret;

    void Start()
    {

    }

    void Update()
    {
        if (turret.Lossvalue > 0)
        {
            Quaternion guntowerRotation = turret.gameObject.transform.rotation;
            guntowerRotation.eulerAngles = new Vector3(0, guntowerRotation.eulerAngles.y, 0);

            // ����ʿ��NPC��Ҫ��ת����λ�ã�ʹ��Y�ᣨ���ϣ���Guntower��Y�����
            Quaternion targetRotation = Quaternion.LookRotation(-Vector3.up, turret.gameObject.transform.up);

            // ʹ��Slerpƽ���ؽ�ʿ��NPC����ת��ֵ��Ŀ����ת
            // ��������ֻ����Y�����ת�����Խ�ʿ��NPC����ת��Y�����滻ΪĿ����ת��Y����
            Quaternion currentRotation = this.transform.rotation;
            currentRotation.eulerAngles = new Vector3(currentRotation.eulerAngles.x, targetRotation.eulerAngles.y, currentRotation.eulerAngles.z);

            // Ӧ��ƽ����ת
            this.transform.rotation = Quaternion.Slerp(transform.rotation, currentRotation, 15 * Time.deltaTime);
        }

    }
}
