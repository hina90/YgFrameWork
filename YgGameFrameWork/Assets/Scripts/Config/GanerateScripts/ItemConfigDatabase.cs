using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class ItemConfigData
    {
        /// <summary>
		///物品Id
		/// </summary>
		public int Id;
		/// <summary>
		///物品名称
		/// </summary>
		public string name;
		/// <summary>
		///设施等级
		/// </summary>
		public int level;
		/// <summary>
		///解锁参数
		/// </summary>
		public int unlockHeart;
		/// <summary>
		///描述
		/// </summary>
		public string des;
		/// <summary>
		///物品功能类型
		/// </summary>
		public int[] funType;
		/// <summary>
		///物品功能参数1
		/// </summary>
		public float[] funParam_1;
		/// <summary>
		///物品功能参数2
		/// </summary>
		public float[] funParam_2;
		/// <summary>
		///物品功能参数3
		/// </summary>
		public float[] funParam_3;
		/// <summary>
		///购买价格
		/// </summary>
		public int price;
		/// <summary>
		///物品Icon
		/// </summary>
		public string icon;
		/// <summary>
		///UI图标
		/// </summary>
		public string uiIcon;
		/// <summary>
		///所属设施
		/// </summary>
		public int facId;
		/// <summary>
		///是否展示特效
		/// </summary>
		public int effect;
		/// <summary>
		///特效索引
		/// </summary>
		public int effectIndex;
		/// <summary>
		///特效位置
		/// </summary>
		public float[] effectPos;
    }

    public class ItemConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =8;
        public const string DATA_PATH ="Config/ItemConfig";

        private List<ItemConfigData> m_datas;

        public  ItemConfigDatabase() { }

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

		private List<ItemConfigData> GetAllData(string[][] m_datas)
		{
			List<ItemConfigData> m_tempList = new List<ItemConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				ItemConfigData m_tempData = new ItemConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.Id))
				{
					m_tempData.Id=0;
				}

					m_tempData.name=m_datas[i][1];
					
				if (!int.TryParse(m_datas[i][2].Trim(),out m_tempData.level))
				{
					m_tempData.level=0;
				}

					
				if (!int.TryParse(m_datas[i][3].Trim(),out m_tempData.unlockHeart))
				{
					m_tempData.unlockHeart=0;
				}

					m_tempData.des=m_datas[i][4];
					m_tempData.funType=CSVConverter.ConvertToArray<int>(m_datas[i][5].Trim());
					m_tempData.funParam_1=CSVConverter.ConvertToArray<float>(m_datas[i][6].Trim());
					m_tempData.funParam_2=CSVConverter.ConvertToArray<float>(m_datas[i][7].Trim());
					m_tempData.funParam_3=CSVConverter.ConvertToArray<float>(m_datas[i][8].Trim());
					
				if (!int.TryParse(m_datas[i][9].Trim(),out m_tempData.price))
				{
					m_tempData.price=0;
				}

					m_tempData.icon=m_datas[i][10];
					m_tempData.uiIcon=m_datas[i][11];
					
				if (!int.TryParse(m_datas[i][12].Trim(),out m_tempData.facId))
				{
					m_tempData.facId=0;
				}

					
				if (!int.TryParse(m_datas[i][13].Trim(),out m_tempData.effect))
				{
					m_tempData.effect=0;
				}

					
				if (!int.TryParse(m_datas[i][14].Trim(),out m_tempData.effectIndex))
				{
					m_tempData.effectIndex=0;
				}

					m_tempData.effectPos=CSVConverter.ConvertToArray<float>(m_datas[i][15].Trim());
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public ItemConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.Id == int.Parse(key));
        }

		public List<ItemConfigData> FindAll(Predicate<ItemConfigData> handler = null)
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

