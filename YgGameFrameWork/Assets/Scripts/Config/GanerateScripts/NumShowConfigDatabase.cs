using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class NumShowConfigData
    {
        /// <summary>
		///数值单位ID
		/// </summary>
		public int ID;
		/// <summary>
		///数值上限值
		/// </summary>
		public Double numCount;
		/// <summary>
		///单位
		/// </summary>
		public string numUnit;
    }

    public class NumShowConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =10;
        public const string DATA_PATH ="Config/NumShowConfig";

        private List<NumShowConfigData> m_datas;

        public  NumShowConfigDatabase() { }

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

		private List<NumShowConfigData> GetAllData(string[][] m_datas)
		{
			List<NumShowConfigData> m_tempList = new List<NumShowConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				NumShowConfigData m_tempData = new NumShowConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.ID))
				{
					m_tempData.ID=0;
				}

					
					m_tempData.numUnit=m_datas[i][2];
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public NumShowConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.ID == int.Parse(key));
        }

		public List<NumShowConfigData> FindAll(Predicate<NumShowConfigData> handler = null)
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

