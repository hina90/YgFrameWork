using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class EggConfigData
    {
        /// <summary>
		///编号
		/// </summary>
		public int EggNum;
		/// <summary>
		///彩蛋动效名称
		/// </summary>
		public string EggRes;
		/// <summary>
		///对应参观点
		/// </summary>
		public string[] EggPoint;
		/// <summary>
		///冷却时间
		/// </summary>
		public int EggTime;
		/// <summary>
		///触发几率
		/// </summary>
		public float EggProb;
    }

    public class EggConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =5;
        public const string DATA_PATH ="Config/EggConfig";

        private List<EggConfigData> m_datas;

        public  EggConfigDatabase() { }

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

		private List<EggConfigData> GetAllData(string[][] m_datas)
		{
			List<EggConfigData> m_tempList = new List<EggConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				EggConfigData m_tempData = new EggConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.EggNum))
				{
					m_tempData.EggNum=0;
				}

					m_tempData.EggRes=m_datas[i][1];
					m_tempData.EggPoint=CSVConverter.ConvertToArray<string>(m_datas[i][2].Trim());
					
				if (!int.TryParse(m_datas[i][3].Trim(),out m_tempData.EggTime))
				{
					m_tempData.EggTime=0;
				}

					
				if (!float.TryParse(m_datas[i][4].Trim(),out m_tempData.EggProb))
				{
					m_tempData.EggProb=0.0f;
				}

				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public EggConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.EggNum == int.Parse(key));
        }

		public List<EggConfigData> FindAll(Predicate<EggConfigData> handler = null)
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

