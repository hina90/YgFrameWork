using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class ConditionConfigData
    {
        /// <summary>
		///条件ID
		/// </summary>
		public int id;
		/// <summary>
		///条件描述
		/// </summary>
		public string conditionDes;
    }

    public class ConditionConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =2;
        public const string DATA_PATH ="Config/ConditionConfig";

        private List<ConditionConfigData> m_datas;

        public  ConditionConfigDatabase() { }

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

		private List<ConditionConfigData> GetAllData(string[][] m_datas)
		{
			List<ConditionConfigData> m_tempList = new List<ConditionConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				ConditionConfigData m_tempData = new ConditionConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.id))
				{
					m_tempData.id=0;
				}

					m_tempData.conditionDes=m_datas[i][1];
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public ConditionConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.id == int.Parse(key));
        }

		public List<ConditionConfigData> FindAll(Predicate<ConditionConfigData> handler = null)
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

