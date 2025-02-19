using UnityEngine;
using UnityEngine.UI;

public class TargetToScreen : MonoBehaviour
{
    public Transform TargetTransform;
    public Vector3 TargetPositionOffset = new Vector3(0, 1, 0);
    //��Ļ�߽��ƫ����
    public float screenBoundOffset = 0.1f;
    public RectTransform TargetImage, TargetImageArrow;
    public Text TargetText;
    //��Ļλ�ú���Ļ�߽�
    public Vector3 screenPosition, screenBound;
    //��ͷָ��ķ���
    public Vector2 Arrowdirection;
    private void LateUpdate()
    {
        TargetToScreenPosition();
    }

    //��Ŀ�������λ��ת��Ϊ��Ļλ�� Ȼ�����Ŀ��ͼ��ͼ�ͷͼ���λ��
    public void TargetToScreenPosition()
    {
        if (Camera.main == null)
        {
            return;
        }
        screenPosition = Camera.main.WorldToScreenPoint(TargetTransform.position + TargetPositionOffset);

        (screenBound.x, screenBound.y) = (Screen.width, Screen.height);
        //���Ŀ������������� ��ת��Ļλ��
        if (screenPosition.z < 0)
        {
            screenPosition = -screenPosition;
            //Ŀ������������棬��Ļλ�ñ���ת����yλ��������Ļ��
            if (screenPosition.y > screenBound.y / 2)
            {
                screenPosition.y = screenBound.y;
            }
            else 
            {
                screenPosition.y = 0;
            }
        }
        //����Ļλ����������Ļ�߽���
        TargetImage.transform.position = new Vector2(
                    Mathf.Clamp(screenPosition.x, screenBound.y * screenBoundOffset, screenBound.x - screenBound.y * screenBoundOffset),
                    Mathf.Clamp(screenPosition.y, screenBound.y * screenBoundOffset, screenBound.y - screenBound.y * screenBoundOffset)
                                );

        //ͨ���Ӽ�ͷ����Ļλ�ü�ȥĿ�����Ļλ������ȡ��ͷ�ķ���
        Arrowdirection = TargetImageArrow.transform.position - screenPosition;
        //���Ŀ��̫�ӽ���Ļ����
        if (Mathf.Abs(Arrowdirection.x + Arrowdirection.y) < 0.1)
        {
            TargetImageArrow.gameObject.SetActive(false);
        }
        else
        {
            TargetImageArrow.gameObject.SetActive(true);
            //���ü�ͷ��up����Ϊ��ͷ����
            TargetImageArrow.transform.up = Arrowdirection;
        }

        //���¾����ı�
        TargetText.text = Vector3.Distance(TargetTransform.position, Camera.main.transform.position).ToString("F1") + "m";
    }
}