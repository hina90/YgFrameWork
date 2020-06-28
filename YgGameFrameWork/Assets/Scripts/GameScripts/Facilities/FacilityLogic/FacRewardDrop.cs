/// <summary>
/// 许愿池奖励物品掉落
/// </summary>
public class FacRewardDrop : Facilities
{
    private GardenModule gardenModule;

    public FacRewardDrop(int facilityId) : base(facilityId)
    {
        gardenModule = GameModuleManager.Instance.GetModule<GardenModule>();
    }

    public override void PutIntoUse(FacilitiesItemData data, float[] args)
    {
        base.PutIntoUse(data, args);
        gardenModule.WishRewardNum += (int)args[0];
    }
}
