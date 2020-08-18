using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class AchieveConfigData
    {
        /// <summary>
		///成就ID
		/// </summary>
		public int ID;
		/// <summary>
		///成就类型
		/// </summary>
		public int Type;
		/// <summary>
		///成就值
		/// </summary>
		public Int64 NeedCount;
		/// <summary>
		///成就奖励类型
		/// </summary>
		public int RewardType;
		/// <summary>
		///成就奖励值
		/// </summary>
		public Int64 RewardValue;
		/// <summary>
		///成就描述
		/// </summary>
		public string AchieveStrIndex;
    }

    public class AchieveConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =2;
        public const string DATA_PATH ="Config/AchieveConfig";

        private List<AchieveConfigData> m_datas;

        public  AchieveConfigDatabase() { }

        public uint TypeID()
        {
            return TYPE_ID;
        }

        public string DataPath()
        {
            return DATA_PATH;
        }

        public void Load()
        {
            TextAsset textAsset = Resources.Load<TextAsset>(DataPath());
            m_datas = GetAllData(CSVConverter.SerializeCSVData(textAsset));
        }

		private List<AchieveConfigData> GetAllData(string[][] m_datas)
		{
			List<AchieveConfigData> m_tempList = new List<AchieveConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				AchieveConfigData m_tempData = new AchieveConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.ID))
				{
					m_tempData.ID=0;
				}

					
				if (!int.TryParse(m_datas[i][1].Trim(),out m_tempData.Type))
				{
					m_tempData.Type=0;
				}

					
					
				if (!int.TryParse(m_datas[i][3].Trim(),out m_tempData.RewardType))
				{
					m_tempData.RewardType=0;
				}

					
					m_tempData.AchieveStrIndex=m_datas[i][5];
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public AchieveConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.ID == int.Parse(key));
        }

		public List<AchieveConfigData> FindAll(Predicate<AchieveConfigData> handler = null)
		{
			if (handler == null)
            {
                return m_datas;
            }
            else
            {
                return m_datas.FindAll(handler);
            }
		}

        public int GetCount()
        {
			return m_datas.Count;
        }
    }
}

