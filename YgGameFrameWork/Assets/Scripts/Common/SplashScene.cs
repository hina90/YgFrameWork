using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class SplashScene : MonoBehaviour
{
    //Logo
    public Sprite SplashLogo;
    //要加载的关卡
    public string SceneName = "GameScene";
    //淡入的时间
    public float fadeIn = 3;
    //淡出的时间
    public float fadeOut = 3;
    //logo精灵组件
    private Image m_LogoImg;
    //一个异步值，用来异步加载
    private AsyncOperation async = null;
    void Start()
    {

        DontDestroyOnLoad(this.transform.parent);
        m_LogoImg = GetComponent<Image>();
        m_LogoImg.overrideSprite = SplashLogo;
        m_LogoImg.color = new Color(1, 1, 1, 0);            
        //检查目标关卡是否为空
        if (SceneName == "")
        {
            Debug.Log("There is not have the level to load please check again");
            return;
        }
        SDKManager.Instance.LogEvent(EventId.GameLogo, "logo", "gamelogo");
        m_LogoImg.DOFade(1, fadeIn).OnComplete(() =>
        {
            LoadScene(SceneName, () =>
            {
                m_LogoImg.DOFade(0, fadeOut).OnComplete(() =>
                {
                    Destroy(this.transform.parent.gameObject);
                });
            });
        });
    }
    public void LoadScene(string levelName, Callback callBack)
    {
        StartCoroutine(IELoadScene(levelName, callBack));
    }
    private IEnumerator IELoadScene(string levelName, Callback callback)
    {
        async = SceneManager.LoadSceneAsync(levelName);
        yield return async;
        callback?.Invoke();
    }
}
