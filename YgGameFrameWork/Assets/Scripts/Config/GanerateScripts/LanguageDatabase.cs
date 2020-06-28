using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class LanguageData
    {
        /// <summary>
		///内容id
		/// </summary>
		public int ID;
		/// <summary>
		///中文简体
		/// </summary>
		public string cnj;
		/// <summary>
		///中文繁体
		/// </summary>
		public string cnf;
		/// <summary>
		///英文
		/// </summary>
		public string en;
    }

    public class LanguageDatabase : IDatabase
    {
        public const uint TYPE_ID =9;
        public const string DATA_PATH ="Config/Language";

        private List<LanguageData> m_datas;

        public  LanguageDatabase() { }

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

		private List<LanguageData> GetAllData(string[][] m_datas)
		{
			List<LanguageData> m_tempList = new List<LanguageData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				LanguageData m_tempData = new LanguageData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.ID))
				{
					m_tempData.ID=0;
				}

					m_tempData.cnj=m_datas[i][1];
					m_tempData.cnf=m_datas[i][2];
					m_tempData.en=m_datas[i][3];
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public LanguageData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.ID == int.Parse(key));
        }

		public List<LanguageData> FindAll(Predicate<LanguageData> handler = null)
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

