using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class DayTaskConfigData
    {
        /// <summary>
		///任务id
		/// </summary>
		public int taskID;
		/// <summary>
		///任务描述
		/// </summary>
		public string des;
		/// <summary>
		///任务条件ID
		/// </summary>
		public int condition;
		/// <summary>
		///条件参数
		/// </summary>
		public int[] conArgument;
		/// <summary>
		///获得奖励描述
		/// </summary>
		public int reward;
		/// <summary>
		///奖励参数
		/// </summary>
		public int rewardArgument;
		/// <summary>
		///接取条件
		/// </summary>
		public int[] accessConditions;
    }

    public class DayTaskConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =4;
        public const string DATA_PATH ="Config/DayTaskConfig";

        private List<DayTaskConfigData> m_datas;

        public  DayTaskConfigDatabase() { }

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

		private List<DayTaskConfigData> GetAllData(string[][] m_datas)
		{
			List<DayTaskConfigData> m_tempList = new List<DayTaskConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				DayTaskConfigData m_tempData = new DayTaskConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.taskID))
				{
					m_tempData.taskID=0;
				}

					m_tempData.des=m_datas[i][1];
					
				if (!int.TryParse(m_datas[i][2].Trim(),out m_tempData.condition))
				{
					m_tempData.condition=0;
				}

					m_tempData.conArgument=CSVConverter.ConvertToArray<int>(m_datas[i][3].Trim());
					
				if (!int.TryParse(m_datas[i][4].Trim(),out m_tempData.reward))
				{
					m_tempData.reward=0;
				}

					
				if (!int.TryParse(m_datas[i][5].Trim(),out m_tempData.rewardArgument))
				{
					m_tempData.rewardArgument=0;
				}

					m_tempData.accessConditions=CSVConverter.ConvertToArray<int>(m_datas[i][6].Trim());
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public DayTaskConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.taskID == int.Parse(key));
        }

		public List<DayTaskConfigData> FindAll(Predicate<DayTaskConfigData> handler = null)
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

