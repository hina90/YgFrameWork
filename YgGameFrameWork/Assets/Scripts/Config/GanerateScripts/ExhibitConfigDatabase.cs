using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class ExhibitConfigData
    {
        /// <summary>
		///养成等级
		/// </summary>
		public int expoLv;
		/// <summary>
		///单价（价格等级）
		/// </summary>
		public Double costPara;
		/// <summary>
		///"速度参数（速度等级）"
		/// </summary>
		public float speedPara;
		/// <summary>
		///"移速参数（速度等级）"
		/// </summary>
		public float movespeedPara;
		/// <summary>
		///"经验值（设备等级）"
		/// </summary>
		public int baseEXP;
		/// <summary>
		///"房间编号（设备等级）"
		/// </summary>
		public int RoomNum;
		/// <summary>
		///"参观点参数（设备等级）"
		/// </summary>
		public int npcPara;
		/// <summary>
		///"展馆资源（设备等级）"
		/// </summary>
		public string expoRes;
		/// <summary>
		///价格升级（价格等级）
		/// </summary>
		public Double costMoney;
		/// <summary>
		///价格进度
		/// </summary>
		public float costProgress;
		/// <summary>
		///速度升级（速度等级）
		/// </summary>
		public Double speedMoney;
		/// <summary>
		///设备升级（设备等级）
		/// </summary>
		public Double roomMoney;
		/// <summary>
		///设备进度
		/// </summary>
		public float roomProgress;
    }

    public class ExhibitConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =6;
        public const string DATA_PATH ="Config/ExhibitConfig";

        private List<ExhibitConfigData> m_datas;

        public  ExhibitConfigDatabase() { }

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

		private List<ExhibitConfigData> GetAllData(string[][] m_datas)
		{
			List<ExhibitConfigData> m_tempList = new List<ExhibitConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				ExhibitConfigData m_tempData = new ExhibitConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.expoLv))
				{
					m_tempData.expoLv=0;
				}

					
					
				if (!float.TryParse(m_datas[i][2].Trim(),out m_tempData.speedPara))
				{
					m_tempData.speedPara=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][3].Trim(),out m_tempData.movespeedPara))
				{
					m_tempData.movespeedPara=0.0f;
				}

					
				if (!int.TryParse(m_datas[i][4].Trim(),out m_tempData.baseEXP))
				{
					m_tempData.baseEXP=0;
				}

					
				if (!int.TryParse(m_datas[i][5].Trim(),out m_tempData.RoomNum))
				{
					m_tempData.RoomNum=0;
				}

					
				if (!int.TryParse(m_datas[i][6].Trim(),out m_tempData.npcPara))
				{
					m_tempData.npcPara=0;
				}

					m_tempData.expoRes=m_datas[i][7];
					
					
				if (!float.TryParse(m_datas[i][9].Trim(),out m_tempData.costProgress))
				{
					m_tempData.costProgress=0.0f;
				}

					
					
					
				if (!float.TryParse(m_datas[i][12].Trim(),out m_tempData.roomProgress))
				{
					m_tempData.roomProgress=0.0f;
				}

				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public ExhibitConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.expoLv == int.Parse(key));
        }

		public List<ExhibitConfigData> FindAll(Predicate<ExhibitConfigData> handler = null)
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

