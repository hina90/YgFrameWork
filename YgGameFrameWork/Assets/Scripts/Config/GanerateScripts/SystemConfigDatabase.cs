using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class SystemConfigData
    {
        /// <summary>
		///系统Id
		/// </summary>
		public int Id;
		/// <summary>
		///名称
		/// </summary>
		public string name;
		/// <summary>
		///价格（小鱼干）
		/// </summary>
		public int price;
		/// <summary>
		///评价需求（星级）
		/// </summary>
		public int star;
    }

    public class SystemConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =14;
        public const string DATA_PATH ="Config/SystemConfig";

        private List<SystemConfigData> m_datas;

        public  SystemConfigDatabase() { }

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

		private List<SystemConfigData> GetAllData(string[][] m_datas)
		{
			List<SystemConfigData> m_tempList = new List<SystemConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				SystemConfigData m_tempData = new SystemConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.Id))
				{
					m_tempData.Id=0;
				}

					m_tempData.name=m_datas[i][1];
					
				if (!int.TryParse(m_datas[i][2].Trim(),out m_tempData.price))
				{
					m_tempData.price=0;
				}

					
				if (!int.TryParse(m_datas[i][3].Trim(),out m_tempData.star))
				{
					m_tempData.star=0;
				}

				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public SystemConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.Id == int.Parse(key));
        }

		public List<SystemConfigData> FindAll(Predicate<SystemConfigData> handler = null)
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

