using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinMapCamera : MonoBehaviour
{
    private Camera minimapCamera;
    private Transform player;
    public float height = 10f;

    void Start()
    {
        minimapCamera = GetComponent<Camera>();
        player = transform.parent;
    }
    void LateUpdate()
    {
        if (minimapCamera != null && player != null)
        {
            Vector3 newPos = Vector3.zero;
            //更新小地图摄像机的位置，确保它总是位于玩家正上方
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 4:
                    newPos = new Vector3(player.position.x - 0.5f, player.position.y + 5.13f, player.position.z + 4.5f);
                    minimapCamera.transform.rotation = Quaternion.Euler(90f, 0, 0f);
                    break;
                case 2:                   
                    newPos = new Vector3(player.position.x - 4.5f, player.position.y + 5.13f, player.position.z + 0.4f);
                    minimapCamera.transform.rotation = Quaternion.Euler(90, 180f, -90f);
                    break;
                case 3:                   
                    newPos = new Vector3(player.position.x + 5f, minimapCamera.transform.position.y, player.position.z - 0.95f);
                    minimapCamera.transform.rotation = Quaternion.Euler(90, 360, -90f);
                    break;
                case 1:                
                    newPos = new Vector3(player.position.x - 1.07f, minimapCamera.transform.position.y, player.position.z - 5.45f);
                    minimapCamera.transform.rotation = Quaternion.Euler(90, 90, -90f);
                    break;

            }
            minimapCamera.transform.position = newPos;
            ////固定小地图摄像机的高度
            //newPos.y = height;
            //固定小地图摄像机的朝向

        }
    }
}
