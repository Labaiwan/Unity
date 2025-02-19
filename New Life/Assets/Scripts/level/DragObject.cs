using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    public Camera mainCamera;
    private Vector3 offset;
    private bool isDragging = false;

    //记录当前场景中的最高 Y 值
    private static float currentMaxY = 1.165964f;
    //记录当前物体拖拽前的 Y 值
    private float originalY;

    //记录当前拖拽物体
    private static GameObject currentDraggedObject = null;

    void Update()
    {
        //当鼠标按下时开始拖拽
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //如果点击的物体是当前物体
                if (hit.transform == transform) 
                {
                    //如果有物体正在被拖拽 恢复上一个物体的 Y 值
                    if (currentDraggedObject != null && currentDraggedObject != gameObject)
                    {
                        //恢复上一个拖拽物体的 Y 值为它的初始值
                        currentDraggedObject.transform.position = new Vector3(
                            currentDraggedObject.transform.position.x,
                            currentDraggedObject.GetComponent<DragObject>().originalY,
                            currentDraggedObject.transform.position.z
                        );
                    }

                    //开始拖拽时 记录物体的原始 Y 值
                    isDragging = true;
                    //计算物体与鼠标点击位置的偏移
                    offset = hit.point - transform.position;

                    //记录当前物体的原始 Y 值
                    originalY = transform.position.y;

                    //将拖拽物体的 Y 坐标设置为当前的最高 Y 值
                    Vector3 newPosition = transform.position;
                    //设置为当前最高 Y 值
                    newPosition.y = currentMaxY;
                    transform.position = newPosition;
                    //记录当前拖拽物体为正在拖拽物体
                    currentDraggedObject = gameObject;
                }
            }
        }

        //当鼠标松开时停止拖拽
        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                // 不更新 currentMaxY，因为拖拽结束时物体保持在当前的最高 Y 值
                // 将当前物体的 Y 坐标保持为它的最高值
                transform.position = new Vector3(transform.position.x, currentMaxY, transform.position.z);
            }

            isDragging = false;
        }

        //如果正在拖拽，更新物体位置
        if (isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //获取当前鼠标在世界空间中的位置
                Vector3 newPosition = hit.point - offset;

                //只修改 X 和 Z 坐标 保持 Y 坐标为当前最大值
                newPosition.y = currentMaxY; 
                // 更新物体的位置
                transform.position = newPosition;
            }
        }
    }
}
