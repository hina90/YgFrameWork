using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class SuperNpcConfigData
    {
        /// <summary>
		///特殊NPCid
		/// </summary>
		public int SuperNpcID;
		/// <summary>
		///对应特殊NPC外观资源
		/// </summary>
		public string SuperNpcRes;
		/// <summary>
		///NPC首次出现条件1
		/// </summary>
		public int FirstCondition1;
		/// <summary>
		///NPC首次出现条件2
		/// </summary>
		public int FirstCondition2;
		/// <summary>
		///NPC刷新条件1
		/// </summary>
		public int RefreshCondition1;
		/// <summary>
		///NPC刷新条件2
		/// </summary>
		public int RefreshCondition2;
		/// <summary>
		///NPC刷新概率
		/// </summary>
		public int RefreshProb;
		/// <summary>
		///刷新次数上限
		/// </summary>
		public int RefreshLimit;
		/// <summary>
		///消失时间
		/// </summary>
		public int DisappearTime;
		/// <summary>
		///基础奖励
		/// </summary>
		public int[] Reward;
		/// <summary>
		///广告奖励倍数1
		/// </summary>
		public int[] Rate1;
		/// <summary>
		///权重1
		/// </summary>
		public int[] probability1;
		/// <summary>
		///广告奖励倍数2
		/// </summary>
		public int[] Rate2;
		/// <summary>
		///权重2
		/// </summary>
		public int[] probability2;
		/// <summary>
		///广告奖励倍数3
		/// </summary>
		public int[] Rate3;
		/// <summary>
		///权重3
		/// </summary>
		public int[] probability3;
    }

    public class SuperNpcConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =11;
        public const string DATA_PATH ="Config/SuperNpcConfig";

        private List<SuperNpcConfigData> m_datas;

        public  SuperNpcConfigDatabase() { }

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

		private List<SuperNpcConfigData> GetAllData(string[][] m_datas)
		{
			List<SuperNpcConfigData> m_tempList = new List<SuperNpcConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				SuperNpcConfigData m_tempData = new SuperNpcConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.SuperNpcID))
				{
					m_tempData.SuperNpcID=0;
				}

					m_tempData.SuperNpcRes=m_datas[i][1];
					
				if (!int.TryParse(m_datas[i][2].Trim(),out m_tempData.FirstCondition1))
				{
					m_tempData.FirstCondition1=0;
				}

					
				if (!int.TryParse(m_datas[i][3].Trim(),out m_tempData.FirstCondition2))
				{
					m_tempData.FirstCondition2=0;
				}

					
				if (!int.TryParse(m_datas[i][4].Trim(),out m_tempData.RefreshCondition1))
				{
					m_tempData.RefreshCondition1=0;
				}

					
				if (!int.TryParse(m_datas[i][5].Trim(),out m_tempData.RefreshCondition2))
				{
					m_tempData.RefreshCondition2=0;
				}

					
				if (!int.TryParse(m_datas[i][6].Trim(),out m_tempData.RefreshProb))
				{
					m_tempData.RefreshProb=0;
				}

					
				if (!int.TryParse(m_datas[i][7].Trim(),out m_tempData.RefreshLimit))
				{
					m_tempData.RefreshLimit=0;
				}

					
				if (!int.TryParse(m_datas[i][8].Trim(),out m_tempData.DisappearTime))
				{
					m_tempData.DisappearTime=0;
				}

					m_tempData.Reward=CSVConverter.ConvertToArray<int>(m_datas[i][9].Trim());
					m_tempData.Rate1=CSVConverter.ConvertToArray<int>(m_datas[i][10].Trim());
					m_tempData.probability1=CSVConverter.ConvertToArray<int>(m_datas[i][11].Trim());
					m_tempData.Rate2=CSVConverter.ConvertToArray<int>(m_datas[i][12].Trim());
					m_tempData.probability2=CSVConverter.ConvertToArray<int>(m_datas[i][13].Trim());
					m_tempData.Rate3=CSVConverter.ConvertToArray<int>(m_datas[i][14].Trim());
					m_tempData.probability3=CSVConverter.ConvertToArray<int>(m_datas[i][15].Trim());
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public SuperNpcConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.SuperNpcID == int.Parse(key));
        }

		public List<SuperNpcConfigData> FindAll(Predicate<SuperNpcConfigData> handler = null)
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

