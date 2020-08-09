using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class ExhibitsConfigData
    {
        /// <summary>
		///展品ID
		/// </summary>
		public int exhibitID;
		/// <summary>
		///展品尺寸
		/// </summary>
		public int size;
		/// <summary>
		///升星经验
		/// </summary>
		public int[] starExpGroup;
		/// <summary>
		///名称索引
		/// </summary>
		public string nameIndex;
		/// <summary>
		///展品品质
		/// </summary>
		public int quality;
		/// <summary>
		///品质描述
		/// </summary>
		public string typeIndex;
		/// <summary>
		///短描述索引
		/// </summary>
		public string descIndex;
		/// <summary>
		///长描述索引
		/// </summary>
		public string longdescIndex;
		/// <summary>
		///展品对应的资源名
		/// </summary>
		public string resName;
		/// <summary>
		///展品对应的图片
		/// </summary>
		public string IconName;
		/// <summary>
		///展品绑定的展台
		/// </summary>
		public int standID;
    }

    public class ExhibitsConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =5;
        public const string DATA_PATH ="Config/ExhibitsConfig";

        private List<ExhibitsConfigData> m_datas;

        public  ExhibitsConfigDatabase() { }

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

		private List<ExhibitsConfigData> GetAllData(string[][] m_datas)
		{
			List<ExhibitsConfigData> m_tempList = new List<ExhibitsConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				ExhibitsConfigData m_tempData = new ExhibitsConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.exhibitID))
				{
					m_tempData.exhibitID=0;
				}

					
				if (!int.TryParse(m_datas[i][1].Trim(),out m_tempData.size))
				{
					m_tempData.size=0;
				}

					m_tempData.starExpGroup=CSVConverter.ConvertToArray<int>(m_datas[i][2].Trim());
					m_tempData.nameIndex=m_datas[i][3];
					
				if (!int.TryParse(m_datas[i][4].Trim(),out m_tempData.quality))
				{
					m_tempData.quality=0;
				}

					m_tempData.typeIndex=m_datas[i][5];
					m_tempData.descIndex=m_datas[i][6];
					m_tempData.longdescIndex=m_datas[i][7];
					m_tempData.resName=m_datas[i][8];
					m_tempData.IconName=m_datas[i][9];
					
				if (!int.TryParse(m_datas[i][10].Trim(),out m_tempData.standID))
				{
					m_tempData.standID=0;
				}

				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public ExhibitsConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.exhibitID == int.Parse(key));
        }

		public List<ExhibitsConfigData> FindAll(Predicate<ExhibitsConfigData> handler = null)
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

