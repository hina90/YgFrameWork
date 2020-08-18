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
		///顾客等级
		/// </summary>
		public int level;
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
		///刷新权重
		/// </summary>
		public int refreshWeight;
		/// <summary>
		///自动解锁
		/// </summary>
		public int autoUnlock;
		/// <summary>
		///"效果类型1-金币加成2-经验加成"
		/// </summary>
		public int effectType;
		/// <summary>
		///加成效果
		/// </summary>
		public float effectValue;
		/// <summary>
		///下级ID
		/// </summary>
		public int NextLvlID;
		/// <summary>
		///幸运合成几率
		/// </summary>
		public float luckyRate;
		/// <summary>
		///幸运合成增加等级
		/// </summary>
		public int LuckyNextLvlID;
		/// <summary>
		///回收价格
		/// </summary>
		public int RecoveryPrice;
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

					
				if (!int.TryParse(m_datas[i][1].Trim(),out m_tempData.type))
				{
					m_tempData.type=0;
				}

					
				if (!int.TryParse(m_datas[i][2].Trim(),out m_tempData.level))
				{
					m_tempData.level=0;
				}

					m_tempData.name=m_datas[i][3];
					m_tempData.introduction=m_datas[i][4];
					m_tempData.icon=m_datas[i][5];
					
				if (!int.TryParse(m_datas[i][6].Trim(),out m_tempData.refreshWeight))
				{
					m_tempData.refreshWeight=0;
				}

					
				if (!int.TryParse(m_datas[i][7].Trim(),out m_tempData.autoUnlock))
				{
					m_tempData.autoUnlock=0;
				}

					
				if (!int.TryParse(m_datas[i][8].Trim(),out m_tempData.effectType))
				{
					m_tempData.effectType=0;
				}

					
				if (!float.TryParse(m_datas[i][9].Trim(),out m_tempData.effectValue))
				{
					m_tempData.effectValue=0.0f;
				}

					
				if (!int.TryParse(m_datas[i][10].Trim(),out m_tempData.NextLvlID))
				{
					m_tempData.NextLvlID=0;
				}

					
				if (!float.TryParse(m_datas[i][11].Trim(),out m_tempData.luckyRate))
				{
					m_tempData.luckyRate=0.0f;
				}

					
				if (!int.TryParse(m_datas[i][12].Trim(),out m_tempData.LuckyNextLvlID))
				{
					m_tempData.LuckyNextLvlID=0;
				}

					
				if (!int.TryParse(m_datas[i][13].Trim(),out m_tempData.RecoveryPrice))
				{
					m_tempData.RecoveryPrice=0;
				}

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

