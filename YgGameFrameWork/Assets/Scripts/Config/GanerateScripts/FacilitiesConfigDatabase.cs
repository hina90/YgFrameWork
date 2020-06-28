using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class FacilitiesConfigData
    {
        /// <summary>
		///设施类型Id
		/// </summary>
		public int id;
		/// <summary>
		///设施类型名称
		/// </summary>
		public string name;
		/// <summary>
		///设施类型
		/// </summary>
		public int type;
		/// <summary>
		///物品_1
		/// </summary>
		public int item_1;
		/// <summary>
		///物品_2
		/// </summary>
		public int item_2;
		/// <summary>
		///物品_3
		/// </summary>
		public int item_3;
		/// <summary>
		///物品_4
		/// </summary>
		public int item_4;
		/// <summary>
		///建造位置坐标
		/// </summary>
		public string buildPos;
		/// <summary>
		///名称类型图标
		/// </summary>
		public string nameIcon;
		/// <summary>
		///设施预制体名称
		/// </summary>
		public string prefabName;
		/// <summary>
		///是否可点击
		/// </summary>
		public int isInteractive;
		/// <summary>
		///设施个数
		/// </summary>
		public int itemNumber;
    }

    public class FacilitiesConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =5;
        public const string DATA_PATH ="Config/FacilitiesConfig";

        private List<FacilitiesConfigData> m_datas;

        public  FacilitiesConfigDatabase() { }

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

		private List<FacilitiesConfigData> GetAllData(string[][] m_datas)
		{
			List<FacilitiesConfigData> m_tempList = new List<FacilitiesConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				FacilitiesConfigData m_tempData = new FacilitiesConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.id))
				{
					m_tempData.id=0;
				}

					m_tempData.name=m_datas[i][1];
					
				if (!int.TryParse(m_datas[i][2].Trim(),out m_tempData.type))
				{
					m_tempData.type=0;
				}

					
				if (!int.TryParse(m_datas[i][3].Trim(),out m_tempData.item_1))
				{
					m_tempData.item_1=0;
				}

					
				if (!int.TryParse(m_datas[i][4].Trim(),out m_tempData.item_2))
				{
					m_tempData.item_2=0;
				}

					
				if (!int.TryParse(m_datas[i][5].Trim(),out m_tempData.item_3))
				{
					m_tempData.item_3=0;
				}

					
				if (!int.TryParse(m_datas[i][6].Trim(),out m_tempData.item_4))
				{
					m_tempData.item_4=0;
				}

					m_tempData.buildPos=m_datas[i][7];
					m_tempData.nameIcon=m_datas[i][8];
					m_tempData.prefabName=m_datas[i][9];
					
				if (!int.TryParse(m_datas[i][10].Trim(),out m_tempData.isInteractive))
				{
					m_tempData.isInteractive=0;
				}

					
				if (!int.TryParse(m_datas[i][11].Trim(),out m_tempData.itemNumber))
				{
					m_tempData.itemNumber=0;
				}

				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public FacilitiesConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.id == int.Parse(key));
        }

		public List<FacilitiesConfigData> FindAll(Predicate<FacilitiesConfigData> handler = null)
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

