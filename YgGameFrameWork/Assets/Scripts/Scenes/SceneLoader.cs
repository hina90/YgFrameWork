using System.Collections;
using UnityEngine;
using System.Text;
using UnityEngine.SceneManagement;


/// <summary>
/// 场景类
/// </summary>
public class SceneLoader : MonoBehaviour
{
    private string sceneName = "";    //场景资源名字

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <param name="mode"></param>
    public void LoadScene(string scName, LoadSceneMode mode = LoadSceneMode.Additive)
    {
        sceneName = scName;
        gameObject.SetActive(true);
        StartCoroutine(LoadNormalScene(mode));
    }
    IEnumerator LoadNormalScene(LoadSceneMode mode, float startPercent = 0)
    {
        int startProgress = (int)(startPercent * 100);
        int displayProgress = startProgress;
        int toProgress = startProgress;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, mode);
        yield return op;


        op.allowSceneActivation = false;   //关闭场景自动切换
        while (op.progress < 0.9f)
        {
            toProgress = startProgress + (int)(op.progress * (1.0f - startPercent) * 100);
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                CSceneManager.Instance.OnLoadProgress(displayProgress / 100.0f);
                yield return null;
            }
            yield return null;
        }
        //场景加载进度
        toProgress = 100;
        while (displayProgress < toProgress)
        {
            ++displayProgress;
            CSceneManager.Instance.OnLoadProgress(displayProgress / 100.0f);
            yield return null;
        }
        op.allowSceneActivation = true;  //开启场景自动切换
        //加载完成
        if (op.isDone)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(scene);
            CSceneManager.Instance.OnLevelBaseLoaded();
        }
    }

}
