using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class DecoConfigData
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
		public string NameIndex;
		/// <summary>
		///描述索引
		/// </summary>
		public string DescIndex;
		/// <summary>
		///图标资源
		/// </summary>
		public string IconName;
		/// <summary>
		///模型资源
		/// </summary>
		public string ModelName;
		/// <summary>
		///"升级材料1-金币2-钻石"
		/// </summary>
		public int CostType;
		/// <summary>
		///升级价格
		/// </summary>
		public Double UpCostCount;
		/// <summary>
		///效果类型
		/// </summary>
		public int EffectType;
		/// <summary>
		///效果参数
		/// </summary>
		public float EffectValue;
    }

    public class DecoConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =4;
        public const string DATA_PATH ="Config/DecoConfig";

        private List<DecoConfigData> m_datas;

        public  DecoConfigDatabase() { }

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

		private List<DecoConfigData> GetAllData(string[][] m_datas)
		{
			List<DecoConfigData> m_tempList = new List<DecoConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				DecoConfigData m_tempData = new DecoConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.CultivteID))
				{
					m_tempData.CultivteID=0;
				}

					
				if (!int.TryParse(m_datas[i][1].Trim(),out m_tempData.CultivteLvl))
				{
					m_tempData.CultivteLvl=0;
				}

					m_tempData.NameIndex=m_datas[i][2];
					m_tempData.DescIndex=m_datas[i][3];
					m_tempData.IconName=m_datas[i][4];
					m_tempData.ModelName=m_datas[i][5];
					
				if (!int.TryParse(m_datas[i][6].Trim(),out m_tempData.CostType))
				{
					m_tempData.CostType=0;
				}

					
					
				if (!int.TryParse(m_datas[i][8].Trim(),out m_tempData.EffectType))
				{
					m_tempData.EffectType=0;
				}

					
				if (!float.TryParse(m_datas[i][9].Trim(),out m_tempData.EffectValue))
				{
					m_tempData.EffectValue=0.0f;
				}

				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public DecoConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.CultivteID == int.Parse(key));
        }

		public List<DecoConfigData> FindAll(Predicate<DecoConfigData> handler = null)
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

