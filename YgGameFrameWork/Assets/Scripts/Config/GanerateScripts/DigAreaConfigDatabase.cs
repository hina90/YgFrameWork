using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class DigAreaConfigData
    {
        /// <summary>
		///挖掘区域ID
		/// </summary>
		public int DigAreaID;
		/// <summary>
		///挖掘点坐标
		/// </summary>
		public int[] DigAreaPoints;
		/// <summary>
		///挖掘展品次数
		/// </summary>
		public int DigECount;
		/// <summary>
		///挖掘展品ID
		/// </summary>
		public int[] ListExhibit;
		/// <summary>
		///挖掘展品ID权重
		/// </summary>
		public int[] ListEWeight;
		/// <summary>
		///挖掘装饰品次数
		/// </summary>
		public int DigDCount;
		/// <summary>
		///挖掘装饰品ID
		/// </summary>
		public int[] ListDecoration;
		/// <summary>
		///挖掘装饰品权重
		/// </summary>
		public int[] ListDWeight;
		/// <summary>
		///挖掘钻石次数
		/// </summary>
		public int DigMDCount;
		/// <summary>
		///挖掘钻石数量
		/// </summary>
		public int[] ListDiamond;
		/// <summary>
		///挖掘钻石权重
		/// </summary>
		public int[] ListMDWeight;
		/// <summary>
		///挖掘金币倍率
		/// </summary>
		public float[] ListGold;
		/// <summary>
		///挖掘金币权重
		/// </summary>
		public int[] ListMGWeight;
    }

    public class DigAreaConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =3;
        public const string DATA_PATH ="Config/DigAreaConfig";

        private List<DigAreaConfigData> m_datas;

        public  DigAreaConfigDatabase() { }

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

		private List<DigAreaConfigData> GetAllData(string[][] m_datas)
		{
			List<DigAreaConfigData> m_tempList = new List<DigAreaConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				DigAreaConfigData m_tempData = new DigAreaConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.DigAreaID))
				{
					m_tempData.DigAreaID=0;
				}

					m_tempData.DigAreaPoints=CSVConverter.ConvertToArray<int>(m_datas[i][1].Trim());
					
				if (!int.TryParse(m_datas[i][2].Trim(),out m_tempData.DigECount))
				{
					m_tempData.DigECount=0;
				}

					m_tempData.ListExhibit=CSVConverter.ConvertToArray<int>(m_datas[i][3].Trim());
					m_tempData.ListEWeight=CSVConverter.ConvertToArray<int>(m_datas[i][4].Trim());
					
				if (!int.TryParse(m_datas[i][5].Trim(),out m_tempData.DigDCount))
				{
					m_tempData.DigDCount=0;
				}

					m_tempData.ListDecoration=CSVConverter.ConvertToArray<int>(m_datas[i][6].Trim());
					m_tempData.ListDWeight=CSVConverter.ConvertToArray<int>(m_datas[i][7].Trim());
					
				if (!int.TryParse(m_datas[i][8].Trim(),out m_tempData.DigMDCount))
				{
					m_tempData.DigMDCount=0;
				}

					m_tempData.ListDiamond=CSVConverter.ConvertToArray<int>(m_datas[i][9].Trim());
					m_tempData.ListMDWeight=CSVConverter.ConvertToArray<int>(m_datas[i][10].Trim());
					m_tempData.ListGold=CSVConverter.ConvertToArray<float>(m_datas[i][11].Trim());
					m_tempData.ListMGWeight=CSVConverter.ConvertToArray<int>(m_datas[i][12].Trim());
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public DigAreaConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.DigAreaID == int.Parse(key));
        }

		public List<DigAreaConfigData> FindAll(Predicate<DigAreaConfigData> handler = null)
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

