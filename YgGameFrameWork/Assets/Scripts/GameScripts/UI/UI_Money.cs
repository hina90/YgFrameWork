using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Money : UIBase
{
    private PlayerModule playerModule;
    private Text goldNum;
    private Text starNum;
    private int currentFish;
    private int addFish;
    private int currentStar;
    private int addStar;
    private Sequence mScoreSequence;//声明
    public override void Init()
    {
        Layer = LayerMenue.MONEY;
        playerModule = GameModuleManager.Instance.GetModule<PlayerModule>();
        mScoreSequence = DOTween.Sequence();//函数内初始化
        mScoreSequence.SetAutoKill(false);//函数内设置属性
        currentFish = playerModule.Fish;
        currentStar = playerModule.Star;
    }
    protected override void Enter()
    {
        goldNum = Find<Text>(gameObject, "GoldNum");
        starNum = Find<Text>(gameObject, "StarNum");
        goldNum.text = playerModule.Fish.ToString();
        starNum.text = playerModule.Star.ToString();
    }
    public override Dictionary<GameEvent, Callback<object[]>> CtorEvent()
    {
        Dictionary<GameEvent, Callback<object[]>> eventDic = new Dictionary<GameEvent, Callback<object[]>>
        {
            //更新小鱼干
            [GameEvent.UPDATE_FISH] = delegate (object[] param)
            {
                addFish = (int)param[0];
                if (addFish > 0)
                    TaskManager.Instance.CheckTask(TaskType.GET_FISH, addFish);
                playerModule.Fish = addFish + playerModule.Fish;
                mScoreSequence.Append(DOTween.To((value) =>
                {
                    var temp = System.Math.Floor(value);//向下取整
                    if (temp < 0) { temp = 0; }
                    goldNum.text = temp + "";//向Text组件赋值
                }, currentFish, playerModule.Fish, 0.9f));
                currentFish = playerModule.Fish;//将更新后的值记录下来, 用于下一次滚动动画
                TDDebug.DebugLog("人物现在的鱼干数量是：" + playerModule.Fish);
            },
            //更新星星
            [GameEvent.UPDATE_STAR] = delegate (object[] param)
            {
                addStar = (int)param[0];
                if (addStar > 0)
                    TaskManager.Instance.CheckTask(TaskType.GET_STAR, addStar);
                playerModule.Star = addStar + playerModule.Star;
                mScoreSequence.Append(DOTween.To((value) =>
                {
                    var temp = System.Math.Floor(value);
                    if (temp < 0) { temp = 0; }
                    starNum.text = temp + "";
                }, currentStar, playerModule.Star, 0.9f));
                currentStar = playerModule.Star;
                if (playerModule.Star >= 30 && !playerModule.MoneyIsOne)
                {
                    TipManager.Instance.ShowMsg("每日任务更新了！");
                    TaskManager.Instance.dayTaskModule.SetInit();
                    TaskManager.Instance.Init();
                    playerModule.MoneyIsOne = true;
                }
            }
        };
        return eventDic;
    }
    public override void Release()
    {
        //金钱永不消失！！！！！！！！！！！！！！！！！！！！！！！
    }
}
