using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class OrderConfigData
    {
        /// <summary>
		///订单id
		/// </summary>
		public int orderID;
		/// <summary>
		///订单名字
		/// </summary>
		public string orderName;
		/// <summary>
		///订单描述
		/// </summary>
		public string orderDes;
		/// <summary>
		///鱼干奖励
		/// </summary>
		public int rewardFish;
		/// <summary>
		///评价奖励
		/// </summary>
		public int rewardStar;
		/// <summary>
		///订单时间
		/// </summary>
		public int orderCompletionTime;
    }

    public class OrderConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =11;
        public const string DATA_PATH ="Config/OrderConfig";

        private List<OrderConfigData> m_datas;

        public  OrderConfigDatabase() { }

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

		private List<OrderConfigData> GetAllData(string[][] m_datas)
		{
			List<OrderConfigData> m_tempList = new List<OrderConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				OrderConfigData m_tempData = new OrderConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.orderID))
				{
					m_tempData.orderID=0;
				}

					m_tempData.orderName=m_datas[i][1];
					m_tempData.orderDes=m_datas[i][2];
					
				if (!int.TryParse(m_datas[i][3].Trim(),out m_tempData.rewardFish))
				{
					m_tempData.rewardFish=0;
				}

					
				if (!int.TryParse(m_datas[i][4].Trim(),out m_tempData.rewardStar))
				{
					m_tempData.rewardStar=0;
				}

					
				if (!int.TryParse(m_datas[i][5].Trim(),out m_tempData.orderCompletionTime))
				{
					m_tempData.orderCompletionTime=0;
				}

				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public OrderConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.orderID == int.Parse(key));
        }

		public List<OrderConfigData> FindAll(Predicate<OrderConfigData> handler = null)
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

