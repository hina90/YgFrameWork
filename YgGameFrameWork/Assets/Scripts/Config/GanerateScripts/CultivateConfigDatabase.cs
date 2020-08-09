using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class CultivateConfigData
    {
        /// <summary>
		///养成ID
		/// </summary>
		public int CultivteID;
		/// <summary>
		///养成等级
		/// </summary>
		public int CultivteLvl;
		/// <summary>
		///名称索引
		/// </summary>
		public string Name;
		/// <summary>
		///描述索引
		/// </summary>
		public string Desc;
		/// <summary>
		///升级消耗
		/// </summary>
		public int UpCostCount;
		/// <summary>
		///效果类型
		/// </summary>
		public int EffectType;
		/// <summary>
		///效果参数
		/// </summary>
		public float EffectValue;
		/// <summary>
		///装饰品品质
		/// </summary>
		public int quality;
		/// <summary>
		///图标资源
		/// </summary>
		public string IconName;
		/// <summary>
		///挖掘图标资源
		/// </summary>
		public string DigIconName;
		/// <summary>
		///模型资源
		/// </summary>
		public string ModelName;
		/// <summary>
		///剪影资源
		/// </summary>
		public string lockIconName;
		/// <summary>
		///诱惑描述
		/// </summary>
		public string NextDesc;
		/// <summary>
		///展板资源缩放
		/// </summary>
		public float[] BoardArray;
		/// <summary>
		///展板资源偏移
		/// </summary>
		public float[] BoardArray2;
    }

    public class CultivateConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =1;
        public const string DATA_PATH ="Config/CultivateConfig";

        private List<CultivateConfigData> m_datas;

        public  CultivateConfigDatabase() { }

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

		private List<CultivateConfigData> GetAllData(string[][] m_datas)
		{
			List<CultivateConfigData> m_tempList = new List<CultivateConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				CultivateConfigData m_tempData = new CultivateConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.CultivteID))
				{
					m_tempData.CultivteID=0;
				}

					
				if (!int.TryParse(m_datas[i][1].Trim(),out m_tempData.CultivteLvl))
				{
					m_tempData.CultivteLvl=0;
				}

					m_tempData.Name=m_datas[i][2];
					m_tempData.Desc=m_datas[i][3];
					
				if (!int.TryParse(m_datas[i][4].Trim(),out m_tempData.UpCostCount))
				{
					m_tempData.UpCostCount=0;
				}

					
				if (!int.TryParse(m_datas[i][5].Trim(),out m_tempData.EffectType))
				{
					m_tempData.EffectType=0;
				}

					
				if (!float.TryParse(m_datas[i][6].Trim(),out m_tempData.EffectValue))
				{
					m_tempData.EffectValue=0.0f;
				}

					
				if (!int.TryParse(m_datas[i][7].Trim(),out m_tempData.quality))
				{
					m_tempData.quality=0;
				}

					m_tempData.IconName=m_datas[i][8];
					m_tempData.DigIconName=m_datas[i][9];
					m_tempData.ModelName=m_datas[i][10];
					m_tempData.lockIconName=m_datas[i][11];
					m_tempData.NextDesc=m_datas[i][12];
					m_tempData.BoardArray=CSVConverter.ConvertToArray<float>(m_datas[i][13].Trim());
					m_tempData.BoardArray2=CSVConverter.ConvertToArray<float>(m_datas[i][14].Trim());
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public CultivateConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.CultivteID == int.Parse(key));
        }

		public List<CultivateConfigData> FindAll(Predicate<CultivateConfigData> handler = null)
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

