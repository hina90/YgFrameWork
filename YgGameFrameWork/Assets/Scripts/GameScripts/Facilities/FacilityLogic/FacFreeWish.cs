/// <summary>
/// 许愿池免费许愿次数
/// </summary>
public class FacFreeWish : Facilities
{
    private GardenModule gardenModule;

    public FacFreeWish(int facilityId) : base(facilityId)
    {
        gardenModule = GameModuleManager.Instance.GetModule<GardenModule>();
    }

    public override void PutIntoUse(FacilitiesItemData data, float[] args)
    {
        base.PutIntoUse(data, args);
        gardenModule.WishTimes += (int)args[0];
        gardenModule.RemainWishTimes += (int)args[0];
    }
}
