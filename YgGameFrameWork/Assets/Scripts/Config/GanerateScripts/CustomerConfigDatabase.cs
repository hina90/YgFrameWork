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
		///顾客名称
		/// </summary>
		public string name;
		/// <summary>
		///简介
		/// </summary>
		public string introduction;
		/// <summary>
		///来访需要的设施
		/// </summary>
		public int[] visitNeedFacilities;
		/// <summary>
		///来访需要的食物
		/// </summary>
		public int[] visitNeedFoods;
		/// <summary>
		///来访需要的花
		/// </summary>
		public int[] visitNeedFlower;
		/// <summary>
		///对应订单
		/// </summary>
		public int[] order;
		/// <summary>
		///需要的心级
		/// </summary>
		public int levelCondition;
		/// <summary>
		///顾客技能
		/// </summary>
		public int[] type;
		/// <summary>
		///Icon
		/// </summary>
		public string icon;
		/// <summary>
		///顾客类型
		/// </summary>
		public int[] customerType;
		/// <summary>
		///顾客动画组
		/// </summary>
		public string guest;
		/// <summary>
		///顾客菜品组
		/// </summary>
		public int[] correspondingDish;
		/// <summary>
		///刷新权重
		/// </summary>
		public int refreshWeight;
		/// <summary>
		///宣传条件
		/// </summary>
		public int promotionalConditions;
		/// <summary>
		///新顾客解锁框文本
		/// </summary>
		public string unlockIntroduction;
    }

    public class CustomerConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =3;
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

					m_tempData.name=m_datas[i][1];
					m_tempData.introduction=m_datas[i][2];
					m_tempData.visitNeedFacilities=CSVConverter.ConvertToArray<int>(m_datas[i][3].Trim());
					m_tempData.visitNeedFoods=CSVConverter.ConvertToArray<int>(m_datas[i][4].Trim());
					m_tempData.visitNeedFlower=CSVConverter.ConvertToArray<int>(m_datas[i][5].Trim());
					m_tempData.order=CSVConverter.ConvertToArray<int>(m_datas[i][6].Trim());
					
				if (!int.TryParse(m_datas[i][7].Trim(),out m_tempData.levelCondition))
				{
					m_tempData.levelCondition=0;
				}

					m_tempData.type=CSVConverter.ConvertToArray<int>(m_datas[i][8].Trim());
					m_tempData.icon=m_datas[i][9];
					m_tempData.customerType=CSVConverter.ConvertToArray<int>(m_datas[i][10].Trim());
					m_tempData.guest=m_datas[i][11];
					m_tempData.correspondingDish=CSVConverter.ConvertToArray<int>(m_datas[i][12].Trim());
					
				if (!int.TryParse(m_datas[i][13].Trim(),out m_tempData.refreshWeight))
				{
					m_tempData.refreshWeight=0;
				}

					
				if (!int.TryParse(m_datas[i][14].Trim(),out m_tempData.promotionalConditions))
				{
					m_tempData.promotionalConditions=0;
				}

					m_tempData.unlockIntroduction=m_datas[i][15];
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

