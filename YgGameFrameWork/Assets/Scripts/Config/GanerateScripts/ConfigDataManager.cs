using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool.Database
{
	public class ConfigDataManager : Singleton<ConfigDataManager>
	{
		public Dictionary<uint, IDatabase> m_Databases;

		public ConfigDataManager()
		{
			m_Databases = new Dictionary<uint, IDatabase>();
			RegisterDataType(new AccountConfigDatabase());
			RegisterDataType(new AchieveConfigDatabase());
			RegisterDataType(new CustomerConfigDatabase());
			RegisterDataType(new DecoConfigDatabase());
			RegisterDataType(new EggConfigDatabase());
			RegisterDataType(new ExhibitConfigDatabase());
			RegisterDataType(new GlobalConfigDatabase());
			RegisterDataType(new GuideConfigDatabase());
			RegisterDataType(new LuckyTurntableConfigDatabase());
			RegisterDataType(new NumShowConfigDatabase());
			RegisterDataType(new SingleInConfigDatabase());
			RegisterDataType(new SoundConfigDatabase());

			Load();
		}

		public void Load()
		{
			foreach (KeyValuePair<uint, IDatabase> data in m_Databases)
			{
				data.Value.Load();
			}
		}

		public T GetDatabase<T>() where T : IDatabase, new()
		{
			T result = new T();
			if (m_Databases.ContainsKey(result.TypeID()))
			{
				return (T)m_Databases[result.TypeID()];
			}
			return default(T);
		}

		private void RegisterDataType(IDatabase database)
		{
			m_Databases[database.TypeID()] = database;
		}
	}
}
