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

            // 计算士兵NPC需要旋转到的位置，使其Y轴（向上）与Guntower的Y轴对齐
            Quaternion targetRotation = Quaternion.LookRotation(-Vector3.up, turret.gameObject.transform.up);

            // 使用Slerp平滑地将士兵NPC的旋转插值到目标旋转
            // 这里我们只关心Y轴的旋转，所以将士兵NPC的旋转的Y部分替换为目标旋转的Y部分
            Quaternion currentRotation = this.transform.rotation;
            currentRotation.eulerAngles = new Vector3(currentRotation.eulerAngles.x, targetRotation.eulerAngles.y, currentRotation.eulerAngles.z);

            // 应用平滑旋转
            this.transform.rotation = Quaternion.Slerp(transform.rotation, currentRotation, 15 * Time.deltaTime);
        }

    }
}
