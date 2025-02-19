using System.Collections;
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class LoadPanel : BasePanel 
{
    public Slider slider; 
    public Text loadtxt;
    //��ǰ����
    private int currentProgress;
    //Ŀ�����
    private int targetProgress;  

    public override void Init() 
    {
        currentProgress = 0;
        targetProgress = 0; 
        StartCoroutine(LoadingScene());
    }

    private IEnumerator LoadingScene()
    {
        UIDataMgr.Instance.HidePanel<BeginPanel>();
        //��ȡ��ǰ�ĳ�������+1 ������ת���¸�����
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //����������������
        asyncOperation.allowSceneActivation = false;
        //��allowSceneActivation = falseʱ���Ῠ��0.89999��
        while (asyncOperation.progress < 0.9f) 
        {
            targetProgress = (int)(asyncOperation.progress * 100);
            yield return LoadProgress(); 
        }
        //��ǰ����Ϊ90 ������Ҫ����Ŀ����ȵ�100
        targetProgress = 100;
        yield return LoadProgress();
        yield return new WaitUntil(() => Input.anyKeyDown);

        //���ϳ�����ת
        asyncOperation.allowSceneActivation = true;
        UIDataMgr.Instance.showPanel<GamePanel>();
        UIDataMgr.Instance.HidePanel<LoadPanel>();
    }

    //���¼��ؽ���
    private IEnumerator LoadProgress()
    {
        while (currentProgress < targetProgress)
        {
            ++currentProgress;
            slider.value = (float)currentProgress / 100;
            loadtxt.text = currentProgress + "%";
            if (currentProgress >= 100)
            {
                loadtxt.text = "�����������ʼ��Ϸ";
            }
            yield return new WaitForEndOfFrame();
        }
    }
}