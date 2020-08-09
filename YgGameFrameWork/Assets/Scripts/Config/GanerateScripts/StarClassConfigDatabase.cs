using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class StarClassConfigData
    {
        /// <summary>
		///星级ID
		/// </summary>
		public int idLevel;
		/// <summary>
		///地板对应资源
		/// </summary>
		public string floorRes;
		/// <summary>
		///墙壁对应资源
		/// </summary>
		public string wallRes;
		/// <summary>
		///对应加成
		/// </summary>
		public float addition;
		/// <summary>
		///升级要达到得在线收入值
		/// </summary>
		public string exp;
		/// <summary>
		///升级奖励
		/// </summary>
		public string reward;
		/// <summary>
		///展馆表情
		/// </summary>
		public int[] star1emotion;
    }

    public class StarClassConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =10;
        public const string DATA_PATH ="Config/StarClassConfig";

        private List<StarClassConfigData> m_datas;

        public  StarClassConfigDatabase() { }

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

		private List<StarClassConfigData> GetAllData(string[][] m_datas)
		{
			List<StarClassConfigData> m_tempList = new List<StarClassConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				StarClassConfigData m_tempData = new StarClassConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.idLevel))
				{
					m_tempData.idLevel=0;
				}

					m_tempData.floorRes=m_datas[i][1];
					m_tempData.wallRes=m_datas[i][2];
					
				if (!float.TryParse(m_datas[i][3].Trim(),out m_tempData.addition))
				{
					m_tempData.addition=0.0f;
				}

					m_tempData.exp=m_datas[i][4];
					m_tempData.reward=m_datas[i][5];
					m_tempData.star1emotion=CSVConverter.ConvertToArray<int>(m_datas[i][6].Trim());
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public StarClassConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.idLevel == int.Parse(key));
        }

		public List<StarClassConfigData> FindAll(Predicate<StarClassConfigData> handler = null)
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

