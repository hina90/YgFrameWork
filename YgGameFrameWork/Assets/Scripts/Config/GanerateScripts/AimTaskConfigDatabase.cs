using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class AimTaskConfigData
    {
        /// <summary>
		///任务id
		/// </summary>
		public int taskId;
		/// <summary>
		///任务描述
		/// </summary>
		public string des;
		/// <summary>
		///任务目标数量
		/// </summary>
		public int targetValue;
		/// <summary>
		///任务奖励类型
		/// </summary>
		public int rewardType;
		/// <summary>
		///奖励数值
		/// </summary>
		public int rewardValue;
		/// <summary>
		///跳转界面
		/// </summary>
		public int uiType;
		/// <summary>
		///是否指引提示
		/// </summary>
		public int showFinger;
		/// <summary>
		///指引类型
		/// </summary>
		public int guideType;
		/// <summary>
		///指引父级
		/// </summary>
		public string parentName;
		/// <summary>
		///指引位置
		/// </summary>
		public float[] pos;
		/// <summary>
		///角度
		/// </summary>
		public float angle;
    }

    public class AimTaskConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =1;
        public const string DATA_PATH ="Config/AimTaskConfig";

        private List<AimTaskConfigData> m_datas;

        public  AimTaskConfigDatabase() { }

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

		private List<AimTaskConfigData> GetAllData(string[][] m_datas)
		{
			List<AimTaskConfigData> m_tempList = new List<AimTaskConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				AimTaskConfigData m_tempData = new AimTaskConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.taskId))
				{
					m_tempData.taskId=0;
				}

					m_tempData.des=m_datas[i][1];
					
				if (!int.TryParse(m_datas[i][2].Trim(),out m_tempData.targetValue))
				{
					m_tempData.targetValue=0;
				}

					
				if (!int.TryParse(m_datas[i][3].Trim(),out m_tempData.rewardType))
				{
					m_tempData.rewardType=0;
				}

					
				if (!int.TryParse(m_datas[i][4].Trim(),out m_tempData.rewardValue))
				{
					m_tempData.rewardValue=0;
				}

					
				if (!int.TryParse(m_datas[i][5].Trim(),out m_tempData.uiType))
				{
					m_tempData.uiType=0;
				}

					
				if (!int.TryParse(m_datas[i][6].Trim(),out m_tempData.showFinger))
				{
					m_tempData.showFinger=0;
				}

					
				if (!int.TryParse(m_datas[i][7].Trim(),out m_tempData.guideType))
				{
					m_tempData.guideType=0;
				}

					m_tempData.parentName=m_datas[i][8];
					m_tempData.pos=CSVConverter.ConvertToArray<float>(m_datas[i][9].Trim());
					
				if (!float.TryParse(m_datas[i][10].Trim(),out m_tempData.angle))
				{
					m_tempData.angle=0.0f;
				}

				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public AimTaskConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.taskId == int.Parse(key));
        }

		public List<AimTaskConfigData> FindAll(Predicate<AimTaskConfigData> handler = null)
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

