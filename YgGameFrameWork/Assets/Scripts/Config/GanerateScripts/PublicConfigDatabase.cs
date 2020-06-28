using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class PublicConfigData
    {
        /// <summary>
		///ID
		/// </summary>
		public int Id;
		/// <summary>
		///条件参数值
		/// </summary>
		public int[] args;
    }

    public class PublicConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =12;
        public const string DATA_PATH ="Config/PublicConfig";

        private List<PublicConfigData> m_datas;

        public  PublicConfigDatabase() { }

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

		private List<PublicConfigData> GetAllData(string[][] m_datas)
		{
			List<PublicConfigData> m_tempList = new List<PublicConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				PublicConfigData m_tempData = new PublicConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.Id))
				{
					m_tempData.Id=0;
				}

					m_tempData.args=CSVConverter.ConvertToArray<int>(m_datas[i][1].Trim());
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public PublicConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.Id == int.Parse(key));
        }

		public List<PublicConfigData> FindAll(Predicate<PublicConfigData> handler = null)
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

