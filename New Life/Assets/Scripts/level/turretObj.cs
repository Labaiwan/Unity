using Invector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class turretObj : MonoBehaviour
{
    //开火点
    public Transform[] gunPoint;
    //旋转的点
    public Transform Gunhead;
    //炮台旋转速度
    private float roundSpeed = 5;
    private Monster targetObj;
    //用于计时的 用来判断攻击间隔时间
    private float nowTime;
    //射程
    public int shotRange = 120;
    //间隔值
    public float interval = 0.8f;

    public int Lossvalue = 100;

    public int Recovervalue = 100;
    //损坏提示
    private bool isDamaged = false;

    public GunSlider towerController;
    //修理值的显示
    private bool isSlider = false;

    public GameObject tipArrow;

    private GameObject effobj;
    private void Start()
    {
        if (Gunhead == null) 
        {
            Gunhead = this.transform;
        }
        towerController = towerController.GetComponent<GunSlider>();
    }

    void Update()
    {    
        //每次更新时都寻找最近的敌人
        Monster nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            targetObj = nearestEnemy;
        }

        if (targetObj == null)
            return;


        if (Lossvalue <= 0) 
        {           
            tipArrow.SetActive(true);
            if (!isSlider)
            {
                towerController.StopCharge();
                isSlider = true;
            }
            if (Input.GetKey(KeyCode.M) && UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.activeSelf) 
            {
                if (!towerController.isEnter)
                {
                    towerController.StartCharge();
                }
            }
            //如果玩家没有按住E键
            else
            {
                //如果进度条已满
                if (towerController.IsFull()) 
                {
                    isSlider = false;
                    isDamaged = false;
                    tipArrow.SetActive(false);
                    Lossvalue = Recovervalue; // 损失值恢复到初始值
                    Destroy(effobj);
                    UIDataMgr.Instance.GetPanel<GamePanel>().ShowTipText(this.gameObject.name + "炮台已修复");
                }
                else
                {
                    towerController.StopCharge();
                }
            }
        }

        //检查Lossvalue是否小于等于0且未显示过损坏提示
        if (Lossvalue <= 0 && !isDamaged)
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().ShowTipText(this.gameObject.name + "炮台已损坏");
            effobj = Instantiate(Resources.Load<GameObject>("Eff/GunSmoke"),this.transform.position + this.transform.forward * 1.5f, Quaternion.Euler(-90, this.transform.eulerAngles.y, 0));
            effobj.GetComponent<ParticleSystem>().Play();
            Lossvalue = 0;
            isDamaged = true; // 标记为已显示损坏提示
            return;
        }

        if (Lossvalue > 0 && Vector3.Distance(Gunhead.position, targetObj.transform.position) < shotRange)
        {
            // 炮塔转向目标
            Gunhead.rotation = Quaternion.Slerp(Gunhead.rotation, Quaternion.LookRotation(targetObj.transform.position - Gunhead.position), roundSpeed * Time.deltaTime);
            if (Time.time - nowTime >= interval)
            {
                Fire();
                nowTime = Time.time;
                Lossvalue--;
            }
        }

        if (Chapter2Mgr.Instance.checkWin())
        {
            tipArrow.SetActive(false);
        }
    }

    // 寻找最近的敌人
    private Monster FindNearestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Monster nearestEnemy = null;

        foreach (Monster monster in Chapter2Mgr.Instance.monsterList)
        {
            if (!monster.isDead && Vector3.Distance(Gunhead.position, monster.transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(Gunhead.position, monster.transform.position);
                nearestEnemy = monster;
            }
        }

        return nearestEnemy;
    }


    private void Fire()
    {
        foreach (Transform gun in gunPoint)
        {
            RaycastHit hit;
            if (Physics.Raycast(gun.position, (targetObj.transform.position + targetObj.transform.up * 0.3f + targetObj.transform.right * 0.6f + targetObj.transform.forward * 1.3f - gun.position).normalized, out hit, shotRange))
            {
                //实例化子弹
                GameObject g = Instantiate(Resources.Load<GameObject>("Else/Bullet"), gun.position, Quaternion.LookRotation(hit.point - gun.position), gun);
                //实例化音频
                GameDataMgr.Instance.PlaySound("gunshot", 1, volume: 0.1f);
                //实例化特效
                GameObject eff = Instantiate(Resources.Load<GameObject>("Eff/GunFire"), gun.transform.position, gun.transform.rotation);
                eff.GetComponent<ParticleSystem>().Play();
                Destroy(eff, 2); 
                Physics.IgnoreCollision(g.GetComponent<Collider>(), g.GetComponent<Collider>());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Lossvalue <= 0 && other.CompareTag("Player"))
        {
            towerController.gameObject.SetActive(true);
            towerController.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 0.5f,this.transform.localPosition.z);
            UIDataMgr.Instance.GetPanel<GamePanel>().tiptxtChat.text = "修复";
            UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(true);
            UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.rectTransform.localPosition = new Vector3(other.transform.position.x, other.transform.position.y, 0);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        towerController.gameObject.SetActive(false);
        UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(false);
    }

    public void GuntoPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Gunhead.transform.rotation = Quaternion.Slerp(Gunhead.rotation, Quaternion.LookRotation(player.transform.position - Gunhead.position), 50 * Time.deltaTime);
        Gunhead.transform.rotation = Quaternion.LookRotation(player.transform.position - Gunhead.position);
    }
}
