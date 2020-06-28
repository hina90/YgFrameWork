using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class StoreConfigData
    {
        /// <summary>
		///货物Id
		/// </summary>
		public int Id;
		/// <summary>
		///货物名称
		/// </summary>
		public string name;
		/// <summary>
		///描述
		/// </summary>
		public string des;
		/// <summary>
		///上货心级条件
		/// </summary>
		public int needStar;
		/// <summary>
		///上货消耗小鱼干
		/// </summary>
		public int costFish;
		/// <summary>
		///售出价格
		/// </summary>
		public int soldPrice;
		/// <summary>
		///上货时间
		/// </summary>
		public int loadTime;
		/// <summary>
		///初始售卖次数
		/// </summary>
		public int soldTimes;
		/// <summary>
		///图标
		/// </summary>
		public string icon;
    }

    public class StoreConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =13;
        public const string DATA_PATH ="Config/StoreConfig";

        private List<StoreConfigData> m_datas;

        public  StoreConfigDatabase() { }

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

		private List<StoreConfigData> GetAllData(string[][] m_datas)
		{
			List<StoreConfigData> m_tempList = new List<StoreConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				StoreConfigData m_tempData = new StoreConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.Id))
				{
					m_tempData.Id=0;
				}

					m_tempData.name=m_datas[i][1];
					m_tempData.des=m_datas[i][2];
					
				if (!int.TryParse(m_datas[i][3].Trim(),out m_tempData.needStar))
				{
					m_tempData.needStar=0;
				}

					
				if (!int.TryParse(m_datas[i][4].Trim(),out m_tempData.costFish))
				{
					m_tempData.costFish=0;
				}

					
				if (!int.TryParse(m_datas[i][5].Trim(),out m_tempData.soldPrice))
				{
					m_tempData.soldPrice=0;
				}

					
				if (!int.TryParse(m_datas[i][6].Trim(),out m_tempData.loadTime))
				{
					m_tempData.loadTime=0;
				}

					
				if (!int.TryParse(m_datas[i][7].Trim(),out m_tempData.soldTimes))
				{
					m_tempData.soldTimes=0;
				}

					m_tempData.icon=m_datas[i][8];
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public StoreConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.Id == int.Parse(key));
        }

		public List<StoreConfigData> FindAll(Predicate<StoreConfigData> handler = null)
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

