using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class SingleInConfigData
    {
        /// <summary>
		///签到天数
		/// </summary>
		public int DayCount;
		/// <summary>
		///物品类型
		/// </summary>
		public int itemType;
		/// <summary>
		///物品值
		/// </summary>
		public float itemValue;
		/// <summary>
		///物品图标
		/// </summary>
		public string itemIcon;
    }

    public class SingleInConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =11;
        public const string DATA_PATH ="Config/SingleInConfig";

        private List<SingleInConfigData> m_datas;

        public  SingleInConfigDatabase() { }

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

		private List<SingleInConfigData> GetAllData(string[][] m_datas)
		{
			List<SingleInConfigData> m_tempList = new List<SingleInConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				SingleInConfigData m_tempData = new SingleInConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.DayCount))
				{
					m_tempData.DayCount=0;
				}

					
				if (!int.TryParse(m_datas[i][1].Trim(),out m_tempData.itemType))
				{
					m_tempData.itemType=0;
				}

					
				if (!float.TryParse(m_datas[i][2].Trim(),out m_tempData.itemValue))
				{
					m_tempData.itemValue=0.0f;
				}

					m_tempData.itemIcon=m_datas[i][3];
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public SingleInConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.DayCount == int.Parse(key));
        }

		public List<SingleInConfigData> FindAll(Predicate<SingleInConfigData> handler = null)
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

