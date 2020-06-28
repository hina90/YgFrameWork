using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class MenuConfigData
    {
        /// <summary>
		///菜品Id
		/// </summary>
		public int Id;
		/// <summary>
		///菜品名称
		/// </summary>
		public int name;
		/// <summary>
		///类型
		/// </summary>
		public int type;
		/// <summary>
		///学习条件
		/// </summary>
		public int[] UnlockCondition;
		/// <summary>
		///条件值
		/// </summary>
		public int[] unluckValue;
		/// <summary>
		///描述
		/// </summary>
		public int des;
		/// <summary>
		///学习价格
		/// </summary>
		public int price;
		/// <summary>
		///制作时间
		/// </summary>
		public int makeTime;
		/// <summary>
		///出售价格
		/// </summary>
		public int soldPrice;
		/// <summary>
		///菜品Icon
		/// </summary>
		public string icon;
    }

    public class MenuConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =10;
        public const string DATA_PATH ="Config/MenuConfig";

        private List<MenuConfigData> m_datas;

        public  MenuConfigDatabase() { }

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

		private List<MenuConfigData> GetAllData(string[][] m_datas)
		{
			List<MenuConfigData> m_tempList = new List<MenuConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				MenuConfigData m_tempData = new MenuConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.Id))
				{
					m_tempData.Id=0;
				}

					
				if (!int.TryParse(m_datas[i][1].Trim(),out m_tempData.name))
				{
					m_tempData.name=0;
				}

					
				if (!int.TryParse(m_datas[i][2].Trim(),out m_tempData.type))
				{
					m_tempData.type=0;
				}

					m_tempData.UnlockCondition=CSVConverter.ConvertToArray<int>(m_datas[i][3].Trim());
					m_tempData.unluckValue=CSVConverter.ConvertToArray<int>(m_datas[i][4].Trim());
					
				if (!int.TryParse(m_datas[i][5].Trim(),out m_tempData.des))
				{
					m_tempData.des=0;
				}

					
				if (!int.TryParse(m_datas[i][6].Trim(),out m_tempData.price))
				{
					m_tempData.price=0;
				}

					
				if (!int.TryParse(m_datas[i][7].Trim(),out m_tempData.makeTime))
				{
					m_tempData.makeTime=0;
				}

					
				if (!int.TryParse(m_datas[i][8].Trim(),out m_tempData.soldPrice))
				{
					m_tempData.soldPrice=0;
				}

					m_tempData.icon=m_datas[i][9];
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public MenuConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.Id == int.Parse(key));
        }

		public List<MenuConfigData> FindAll(Predicate<MenuConfigData> handler = null)
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

