using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class AccountConfigData
    {
        /// <summary>
		///等级
		/// </summary>
		public int Lvl;
		/// <summary>
		///等级经验下限
		/// </summary>
		public Double ExpMin;
		/// <summary>
		///等级经验上限
		/// </summary>
		public Double ExpMax;
		/// <summary>
		///升级奖励金币数量
		/// </summary>
		public Double RewardCount;
    }

    public class AccountConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =1;
        public const string DATA_PATH ="Config/AccountConfig";

        private List<AccountConfigData> m_datas;

        public  AccountConfigDatabase() { }

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

		private List<AccountConfigData> GetAllData(string[][] m_datas)
		{
			List<AccountConfigData> m_tempList = new List<AccountConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				AccountConfigData m_tempData = new AccountConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.Lvl))
				{
					m_tempData.Lvl=0;
				}

					
					
					
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public AccountConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.Lvl == int.Parse(key));
        }

		public List<AccountConfigData> FindAll(Predicate<AccountConfigData> handler = null)
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

