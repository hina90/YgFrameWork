using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class UpRewardConfigData
    {
        /// <summary>
		///展品ID加等级
		/// </summary>
		public int levelID;
		/// <summary>
		///展品名称
		/// </summary>
		public string exhibitName;
		/// <summary>
		///等级
		/// </summary>
		public int level;
		/// <summary>
		///升级消耗
		/// </summary>
		public string consume;
		/// <summary>
		///展馆经验
		/// </summary>
		public int toMuseumExp;
		/// <summary>
		/// 收益/s
		/// </summary>
		public string income;
		/// <summary>
		///奖励类型
		/// </summary>
		public int rewardType;
		/// <summary>
		///奖品ID
		/// </summary>
		public int rewardID;
		/// <summary>
		///奖励数量
		/// </summary>
		public int rewardNum;
    }

    public class UpRewardConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =12;
        public const string DATA_PATH ="Config/UpRewardConfig";

        private List<UpRewardConfigData> m_datas;

        public  UpRewardConfigDatabase() { }

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

		private List<UpRewardConfigData> GetAllData(string[][] m_datas)
		{
			List<UpRewardConfigData> m_tempList = new List<UpRewardConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				UpRewardConfigData m_tempData = new UpRewardConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.levelID))
				{
					m_tempData.levelID=0;
				}

					m_tempData.exhibitName=m_datas[i][1];
					
				if (!int.TryParse(m_datas[i][2].Trim(),out m_tempData.level))
				{
					m_tempData.level=0;
				}

					m_tempData.consume=m_datas[i][3];
					
				if (!int.TryParse(m_datas[i][4].Trim(),out m_tempData.toMuseumExp))
				{
					m_tempData.toMuseumExp=0;
				}

					m_tempData.income=m_datas[i][5];
					
				if (!int.TryParse(m_datas[i][6].Trim(),out m_tempData.rewardType))
				{
					m_tempData.rewardType=0;
				}

					
				if (!int.TryParse(m_datas[i][7].Trim(),out m_tempData.rewardID))
				{
					m_tempData.rewardID=0;
				}

					
				if (!int.TryParse(m_datas[i][8].Trim(),out m_tempData.rewardNum))
				{
					m_tempData.rewardNum=0;
				}

				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public UpRewardConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.levelID == int.Parse(key));
        }

		public List<UpRewardConfigData> FindAll(Predicate<UpRewardConfigData> handler = null)
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

