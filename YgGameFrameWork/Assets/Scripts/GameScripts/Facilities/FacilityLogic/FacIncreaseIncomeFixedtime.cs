/// <summary>
/// 每间隔固定时间额外获取收入
/// </summary>
public class FacIncreaseIncomeFixedtime : Facilities
{
    public FacIncreaseIncomeFixedtime(int facilityId) : base(facilityId)
    {
    }

    public override void PutIntoUse(FacilitiesItemData data, float[] args)
    {
        base.PutIntoUse(data, args);
        FacModule.AddFacilityIncomeValue(data.facilitiesConfigData.id, args[1]);
    }
}
