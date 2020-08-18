using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class LuckyTurntableConfigData
    {
        /// <summary>
		///转盘ID
		/// </summary>
		public int ID;
		/// <summary>
		///转盘权重
		/// </summary>
		public int weight;
		/// <summary>
		///转盘物品类型
		/// </summary>
		public int itemType;
		/// <summary>
		///转盘物品值
		/// </summary>
		public float itemValue;
		/// <summary>
		///转盘物品图标
		/// </summary>
		public string itemIcon;
    }

    public class LuckyTurntableConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =9;
        public const string DATA_PATH ="Config/LuckyTurntableConfig";

        private List<LuckyTurntableConfigData> m_datas;

        public  LuckyTurntableConfigDatabase() { }

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

		private List<LuckyTurntableConfigData> GetAllData(string[][] m_datas)
		{
			List<LuckyTurntableConfigData> m_tempList = new List<LuckyTurntableConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				LuckyTurntableConfigData m_tempData = new LuckyTurntableConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.ID))
				{
					m_tempData.ID=0;
				}

					
				if (!int.TryParse(m_datas[i][1].Trim(),out m_tempData.weight))
				{
					m_tempData.weight=0;
				}

					
				if (!int.TryParse(m_datas[i][2].Trim(),out m_tempData.itemType))
				{
					m_tempData.itemType=0;
				}

					
				if (!float.TryParse(m_datas[i][3].Trim(),out m_tempData.itemValue))
				{
					m_tempData.itemValue=0.0f;
				}

					m_tempData.itemIcon=m_datas[i][4];
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public LuckyTurntableConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.ID == int.Parse(key));
        }

		public List<LuckyTurntableConfigData> FindAll(Predicate<LuckyTurntableConfigData> handler = null)
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

