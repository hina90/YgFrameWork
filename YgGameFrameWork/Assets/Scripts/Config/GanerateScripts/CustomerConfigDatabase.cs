using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class CustomerConfigData
    {
        /// <summary>
		///顾客Id
		/// </summary>
		public int Id;
		/// <summary>
		///顾客类型
		/// </summary>
		public int type;
		/// <summary>
		///顾客名称
		/// </summary>
		public string name;
		/// <summary>
		///简介
		/// </summary>
		public string introduction;
		/// <summary>
		///Icon
		/// </summary>
		public string icon;
		/// <summary>
		///顾客类型.1
		/// </summary>
		public int[] customerType;
		/// <summary>
		///刷新权重
		/// </summary>
		public int refreshWeight;
		/// <summary>
		///1星展馆表情
		/// </summary>
		public int[] star1emotion;
		/// <summary>
		///2星展馆表情
		/// </summary>
		public int[] star2emotion;
		/// <summary>
		///3星展馆表情
		/// </summary>
		public int[] star3emotion;
		/// <summary>
		///4星展馆表情
		/// </summary>
		public int[] star4emotion;
    }

    public class CustomerConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =2;
        public const string DATA_PATH ="Config/CustomerConfig";

        private List<CustomerConfigData> m_datas;

        public  CustomerConfigDatabase() { }

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

		private List<CustomerConfigData> GetAllData(string[][] m_datas)
		{
			List<CustomerConfigData> m_tempList = new List<CustomerConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				CustomerConfigData m_tempData = new CustomerConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.Id))
				{
					m_tempData.Id=0;
				}

					
				if (!int.TryParse(m_datas[i][1].Trim(),out m_tempData.type))
				{
					m_tempData.type=0;
				}

					m_tempData.name=m_datas[i][2];
					m_tempData.introduction=m_datas[i][3];
					m_tempData.icon=m_datas[i][4];
					m_tempData.customerType=CSVConverter.ConvertToArray<int>(m_datas[i][5].Trim());
					
				if (!int.TryParse(m_datas[i][6].Trim(),out m_tempData.refreshWeight))
				{
					m_tempData.refreshWeight=0;
				}

					m_tempData.star1emotion=CSVConverter.ConvertToArray<int>(m_datas[i][7].Trim());
					m_tempData.star2emotion=CSVConverter.ConvertToArray<int>(m_datas[i][8].Trim());
					m_tempData.star3emotion=CSVConverter.ConvertToArray<int>(m_datas[i][9].Trim());
					m_tempData.star4emotion=CSVConverter.ConvertToArray<int>(m_datas[i][10].Trim());
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public CustomerConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.Id == int.Parse(key));
        }

		public List<CustomerConfigData> FindAll(Predicate<CustomerConfigData> handler = null)
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

