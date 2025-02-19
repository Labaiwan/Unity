using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    public Camera mainCamera;
    private Vector3 offset;
    private bool isDragging = false;

    //��¼��ǰ�����е���� Y ֵ
    private static float currentMaxY = 1.165964f;
    //��¼��ǰ������קǰ�� Y ֵ
    private float originalY;

    //��¼��ǰ��ק����
    private static GameObject currentDraggedObject = null;

    void Update()
    {
        //����갴��ʱ��ʼ��ק
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //�������������ǵ�ǰ����
                if (hit.transform == transform) 
                {
                    //������������ڱ���ק �ָ���һ������� Y ֵ
                    if (currentDraggedObject != null && currentDraggedObject != gameObject)
                    {
                        //�ָ���һ����ק����� Y ֵΪ���ĳ�ʼֵ
                        currentDraggedObject.transform.position = new Vector3(
                            currentDraggedObject.transform.position.x,
                            currentDraggedObject.GetComponent<DragObject>().originalY,
                            currentDraggedObject.transform.position.z
                        );
                    }

                    //��ʼ��קʱ ��¼�����ԭʼ Y ֵ
                    isDragging = true;
                    //���������������λ�õ�ƫ��
                    offset = hit.point - transform.position;

                    //��¼��ǰ�����ԭʼ Y ֵ
                    originalY = transform.position.y;

                    //����ק����� Y ��������Ϊ��ǰ����� Y ֵ
                    Vector3 newPosition = transform.position;
                    //����Ϊ��ǰ��� Y ֵ
                    newPosition.y = currentMaxY;
                    transform.position = newPosition;
                    //��¼��ǰ��ק����Ϊ������ק����
                    currentDraggedObject = gameObject;
                }
            }
        }

        //������ɿ�ʱֹͣ��ק
        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                // ������ currentMaxY����Ϊ��ק����ʱ���屣���ڵ�ǰ����� Y ֵ
                // ����ǰ����� Y ���걣��Ϊ�������ֵ
                transform.position = new Vector3(transform.position.x, currentMaxY, transform.position.z);
            }

            isDragging = false;
        }

        //���������ק����������λ��
        if (isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //��ȡ��ǰ���������ռ��е�λ��
                Vector3 newPosition = hit.point - offset;

                //ֻ�޸� X �� Z ���� ���� Y ����Ϊ��ǰ���ֵ
                newPosition.y = currentMaxY; 
                // ���������λ��
                transform.position = newPosition;
            }
        }
    }
}
