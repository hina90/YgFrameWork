namespace Tool.Database
{
    public interface IDatabase
    {
        uint TypeID();
        string DataPath();
        void Load();
    }
}

