using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanBone
{
    public HumanBodyBones bone;
    public float weight = 1.0f;
} 

public class WeaponIk : MonoBehaviour
{
    public Transform targetTransform; 
    public Transform aimTransform;
    public Vector3 targetOffset;

    public int iterations = 10;
    [Range(0,1)]
    public float weight = 1.0f;

    public float angleLimit = 90;
    public float distanceLimit = 1.5f;

    public HumanBone[] humanBones;
    private Transform[] boneTransforms;

    // ������ʱ��
    public float shootInterval = 0.8f;

    // �ϴ����ʱ��
    private float lastShootTime;// ��ʼ��Ϊ-1.2ȷ����һ�������������
    private EnemyVision vision;

    void Start()
    {
        vision = GetComponent<EnemyVision>();
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Animator animator = GetComponent<Animator>();
        boneTransforms = new Transform[humanBones.Length];
        for (int i = 0; i< boneTransforms.Length; i++)
        {
            boneTransforms[i] = animator.GetBoneTransform(humanBones[i].bone);
        }
    }

    private Vector3 GetTargetPosition()
    { 
        Vector3 targetDirection = (targetTransform.position + targetOffset) - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;
        float blendOut = 0.0f;

        float targetAngle = Vector3.Angle(targetDirection,aimDirection);
        if (targetAngle > angleLimit)
        {
            blendOut += (targetAngle - angleLimit) / 50.0f;
        }

        float targetDistance = targetDirection.magnitude;
        if (targetDistance < distanceLimit)
        {
            blendOut += distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return aimTransform.position + direction;
    }


    void LateUpdate()
    {
        if (aimTransform == null)
        {
            return;
        }

        if (targetTransform == null)
        {
            return;
        }
        if (vision.PlayerInSight)
        {
            Vector3 targetPosition = GetTargetPosition();
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < boneTransforms.Length; j++)
                {
                    Transform bone = boneTransforms[j];
                    float boneWeight = humanBones[j].weight * weight;
                    AimAtTarget(bone, targetPosition, boneWeight);
                }

            }
        }
        
    }

    private void AimAtTarget(Transform bone,Vector3 targetPosition,float weight)
    { 
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection,targetDirection);
        Quaternion blendRotation = Quaternion.Slerp(Quaternion.identity,aimTowards,weight);
        bone.rotation = aimTowards * bone.rotation;
    }

    public void SetTargetTransform(Transform target)
    { 
        targetTransform = target;
    }

    public void SetAimTransform(Transform aim)
    {
        aimTransform = aim;
    }
    // ������Shoot����
    public void Shoot()
    {
        // ��ȡ��ǰʱ��
        float currentTime = Time.time;

        // ����Ƿ�ﵽ��������
        if (currentTime - lastShootTime >= shootInterval && vision.PlayerInSight)
        {
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Else/AiBullet"), aimTransform.position, aimTransform.rotation);

            // �����ϴ����ʱ��
            lastShootTime = currentTime;
            Debug.Log("�����");
        }
    }
}
