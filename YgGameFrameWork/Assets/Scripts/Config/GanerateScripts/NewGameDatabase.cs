using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class NewGameData
    {
        /// <summary>
		///id
		/// </summary>
		public int ID;
		/// <summary>
		///新手引导阶段
		/// </summary>
		public int teachType;
		/// <summary>
		///dese
		/// </summary>
		public string dese;
    }

    public class NewGameDatabase : IDatabase
    {
        public const uint TYPE_ID =7;
        public const string DATA_PATH ="Config/NewGame";

        private List<NewGameData> m_datas;

        public  NewGameDatabase() { }

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

		private List<NewGameData> GetAllData(string[][] m_datas)
		{
			List<NewGameData> m_tempList = new List<NewGameData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				NewGameData m_tempData = new NewGameData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.ID))
				{
					m_tempData.ID=0;
				}

					
				if (!int.TryParse(m_datas[i][1].Trim(),out m_tempData.teachType))
				{
					m_tempData.teachType=0;
				}

					m_tempData.dese=m_datas[i][2];
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public NewGameData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.ID == int.Parse(key));
        }

		public List<NewGameData> FindAll(Predicate<NewGameData> handler = null)
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

