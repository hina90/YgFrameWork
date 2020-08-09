using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class DigConfigData
    {
        /// <summary>
		///挖掘场ID
		/// </summary>
		public int DigID;
		/// <summary>
		///挖掘次数
		/// </summary>
		public int DigFrequency;
		/// <summary>
		///挖掘消耗类型
		/// </summary>
		public int DigCostType;
		/// <summary>
		///挖掘消耗数量
		/// </summary>
		public int DigCostCount;
		/// <summary>
		///挖掘区域ID
		/// </summary>
		public int[] DigAreaIDs;
		/// <summary>
		///挖掘满级转化货币类型
		/// </summary>
		public int ExchangeType;
		/// <summary>
		///挖掘满级转化货币数量
		/// </summary>
		public int ExchangeCount;
    }

    public class DigConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =4;
        public const string DATA_PATH ="Config/DigConfig";

        private List<DigConfigData> m_datas;

        public  DigConfigDatabase() { }

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

		private List<DigConfigData> GetAllData(string[][] m_datas)
		{
			List<DigConfigData> m_tempList = new List<DigConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				DigConfigData m_tempData = new DigConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.DigID))
				{
					m_tempData.DigID=0;
				}

					
				if (!int.TryParse(m_datas[i][1].Trim(),out m_tempData.DigFrequency))
				{
					m_tempData.DigFrequency=0;
				}

					
				if (!int.TryParse(m_datas[i][2].Trim(),out m_tempData.DigCostType))
				{
					m_tempData.DigCostType=0;
				}

					
				if (!int.TryParse(m_datas[i][3].Trim(),out m_tempData.DigCostCount))
				{
					m_tempData.DigCostCount=0;
				}

					m_tempData.DigAreaIDs=CSVConverter.ConvertToArray<int>(m_datas[i][4].Trim());
					
				if (!int.TryParse(m_datas[i][5].Trim(),out m_tempData.ExchangeType))
				{
					m_tempData.ExchangeType=0;
				}

					
				if (!int.TryParse(m_datas[i][6].Trim(),out m_tempData.ExchangeCount))
				{
					m_tempData.ExchangeCount=0;
				}

				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public DigConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.DigID == int.Parse(key));
        }

		public List<DigConfigData> FindAll(Predicate<DigConfigData> handler = null)
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

