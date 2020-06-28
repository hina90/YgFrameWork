public class TaskCondition
{
    public TaskType conditionType;//条件id
    public int nowAmount;//当前这个条件的进度
    public int targetAmount;//这个条件的目标进度
    public int customerID;//顾客ID
    public bool isFinish = false;//是否满足这个条件
    public TaskCondition(TaskType conditionType, int nowAmount, int targetAmount, int customerID = 0)
    {
        this.conditionType = conditionType;
        this.nowAmount = nowAmount;
        this.targetAmount = targetAmount;
        this.customerID = customerID;
    }
}
