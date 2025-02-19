using System.Collections;
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class LoadPanel : BasePanel 
{
    public Slider slider; 
    public Text loadtxt;
    //当前进度
    private int currentProgress;
    //目标进度
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
        //获取当前的场景索引+1 就是跳转到下个场景
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //不允许场景立即激活
        asyncOperation.allowSceneActivation = false;
        //在allowSceneActivation = false时，会卡在0.89999上
        while (asyncOperation.progress < 0.9f) 
        {
            targetProgress = (int)(asyncOperation.progress * 100);
            yield return LoadProgress(); 
        }
        //当前进度为90 所以需要设置目标进度到100
        targetProgress = 100;
        yield return LoadProgress();
        yield return new WaitUntil(() => Input.anyKeyDown);

        //马上场景跳转
        asyncOperation.allowSceneActivation = true;
        UIDataMgr.Instance.showPanel<GamePanel>();
        UIDataMgr.Instance.HidePanel<LoadPanel>();
    }

    //更新加载进度
    private IEnumerator LoadProgress()
    {
        while (currentProgress < targetProgress)
        {
            ++currentProgress;
            slider.value = (float)currentProgress / 100;
            loadtxt.text = currentProgress + "%";
            if (currentProgress >= 100)
            {
                loadtxt.text = "请点击任意键开始游戏";
            }
            yield return new WaitForEndOfFrame();
        }
    }
}