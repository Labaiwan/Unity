using UnityEngine;
using UnityEngine.UI;

public class TargetToScreen : MonoBehaviour
{
    public Transform TargetTransform;
    public Vector3 TargetPositionOffset = new Vector3(0, 1, 0);
    //屏幕边界的偏移量
    public float screenBoundOffset = 0.1f;
    public RectTransform TargetImage, TargetImageArrow;
    public Text TargetText;
    //屏幕位置和屏幕边界
    public Vector3 screenPosition, screenBound;
    //箭头指向的方向
    public Vector2 Arrowdirection;
    private void LateUpdate()
    {
        TargetToScreenPosition();
    }

    //将目标的世界位置转换为屏幕位置 然后更新目标图像和箭头图像的位置
    public void TargetToScreenPosition()
    {
        if (Camera.main == null)
        {
            return;
        }
        screenPosition = Camera.main.WorldToScreenPoint(TargetTransform.position + TargetPositionOffset);

        (screenBound.x, screenBound.y) = (Screen.width, Screen.height);
        //如果目标在摄像机后面 翻转屏幕位置
        if (screenPosition.z < 0)
        {
            screenPosition = -screenPosition;
            //目标在摄像机后面，屏幕位置被翻转，但y位置仍在屏幕上
            if (screenPosition.y > screenBound.y / 2)
            {
                screenPosition.y = screenBound.y;
            }
            else 
            {
                screenPosition.y = 0;
            }
        }
        //将屏幕位置限制在屏幕边界内
        TargetImage.transform.position = new Vector2(
                    Mathf.Clamp(screenPosition.x, screenBound.y * screenBoundOffset, screenBound.x - screenBound.y * screenBoundOffset),
                    Mathf.Clamp(screenPosition.y, screenBound.y * screenBoundOffset, screenBound.y - screenBound.y * screenBoundOffset)
                                );

        //通过从箭头的屏幕位置减去目标的屏幕位置来获取箭头的方向
        Arrowdirection = TargetImageArrow.transform.position - screenPosition;
        //如果目标太接近屏幕中心
        if (Mathf.Abs(Arrowdirection.x + Arrowdirection.y) < 0.1)
        {
            TargetImageArrow.gameObject.SetActive(false);
        }
        else
        {
            TargetImageArrow.gameObject.SetActive(true);
            //设置箭头的up向量为箭头方向
            TargetImageArrow.transform.up = Arrowdirection;
        }

        //更新距离文本
        TargetText.text = Vector3.Distance(TargetTransform.position, Camera.main.transform.position).ToString("F1") + "m";
    }
}