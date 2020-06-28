using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class GardenConfigData
    {
        /// <summary>
		///花种Id
		/// </summary>
		public int id;
		/// <summary>
		///花种名称
		/// </summary>
		public string name;
		/// <summary>
		///种植周期
		/// </summary>
		public int[] GrowthTime;
		/// <summary>
		///变异概率
		/// </summary>
		public float mutationRate;
		/// <summary>
		///变异CD时间
		/// </summary>
		public int mutationCDTime;
		/// <summary>
		///重新种植CD时间
		/// </summary>
		public int sowCDTime;
		/// <summary>
		///突变种子Id
		/// </summary>
		public int mutationID;
		/// <summary>
		///播种价格
		/// </summary>
		public int price;
		/// <summary>
		///种子图标
		/// </summary>
		public string seedIcon;
		/// <summary>
		///花苗图标
		/// </summary>
		public string flowerSeedingIcon;
		/// <summary>
		///图标
		/// </summary>
		public string icon;
    }

    public class GardenConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =6;
        public const string DATA_PATH ="Config/GardenConfig";

        private List<GardenConfigData> m_datas;

        public  GardenConfigDatabase() { }

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

		private List<GardenConfigData> GetAllData(string[][] m_datas)
		{
			List<GardenConfigData> m_tempList = new List<GardenConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				GardenConfigData m_tempData = new GardenConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.id))
				{
					m_tempData.id=0;
				}

					m_tempData.name=m_datas[i][1];
					m_tempData.GrowthTime=CSVConverter.ConvertToArray<int>(m_datas[i][2].Trim());
					
				if (!float.TryParse(m_datas[i][3].Trim(),out m_tempData.mutationRate))
				{
					m_tempData.mutationRate=0.0f;
				}

					
				if (!int.TryParse(m_datas[i][4].Trim(),out m_tempData.mutationCDTime))
				{
					m_tempData.mutationCDTime=0;
				}

					
				if (!int.TryParse(m_datas[i][5].Trim(),out m_tempData.sowCDTime))
				{
					m_tempData.sowCDTime=0;
				}

					
				if (!int.TryParse(m_datas[i][6].Trim(),out m_tempData.mutationID))
				{
					m_tempData.mutationID=0;
				}

					
				if (!int.TryParse(m_datas[i][7].Trim(),out m_tempData.price))
				{
					m_tempData.price=0;
				}

					m_tempData.seedIcon=m_datas[i][8];
					m_tempData.flowerSeedingIcon=m_datas[i][9];
					m_tempData.icon=m_datas[i][10];
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public GardenConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.id == int.Parse(key));
        }

		public List<GardenConfigData> FindAll(Predicate<GardenConfigData> handler = null)
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

