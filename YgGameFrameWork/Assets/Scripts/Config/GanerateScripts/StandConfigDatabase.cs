using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class StandConfigData
    {
        /// <summary>
		///展台ID
		/// </summary>
		public int id;
		/// <summary>
		///展台位置
		/// </summary>
		public string site;
		/// <summary>
		///展台尺寸
		/// </summary>
		public int size;
		/// <summary>
		///解锁价格
		/// </summary>
		public int unlockPrice;
		/// <summary>
		///解锁条件
		/// </summary>
		public int unlockCondition;
		/// <summary>
		///对应展品
		/// </summary>
		public int Exhibits;
    }

    public class StandConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =9;
        public const string DATA_PATH ="Config/StandConfig";

        private List<StandConfigData> m_datas;

        public  StandConfigDatabase() { }

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

		private List<StandConfigData> GetAllData(string[][] m_datas)
		{
			List<StandConfigData> m_tempList = new List<StandConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				StandConfigData m_tempData = new StandConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.id))
				{
					m_tempData.id=0;
				}

					m_tempData.site=m_datas[i][1];
					
				if (!int.TryParse(m_datas[i][2].Trim(),out m_tempData.size))
				{
					m_tempData.size=0;
				}

					
				if (!int.TryParse(m_datas[i][3].Trim(),out m_tempData.unlockPrice))
				{
					m_tempData.unlockPrice=0;
				}

					
				if (!int.TryParse(m_datas[i][4].Trim(),out m_tempData.unlockCondition))
				{
					m_tempData.unlockCondition=0;
				}

					
				if (!int.TryParse(m_datas[i][5].Trim(),out m_tempData.Exhibits))
				{
					m_tempData.Exhibits=0;
				}

				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public StandConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.id == int.Parse(key));
        }

		public List<StandConfigData> FindAll(Predicate<StandConfigData> handler = null)
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

