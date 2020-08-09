using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class SoundConfigData
    {
        /// <summary>
		///音效ID
		/// </summary>
		public string name;
		/// <summary>
		///资源路径
		/// </summary>
		public string path;
		/// <summary>
		///音效音量
		/// </summary>
		public float volume;
		/// <summary>
		///备注
		/// </summary>
		public string remark;
    }

    public class SoundConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =8;
        public const string DATA_PATH ="Config/SoundConfig";

        private List<SoundConfigData> m_datas;

        public  SoundConfigDatabase() { }

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

		private List<SoundConfigData> GetAllData(string[][] m_datas)
		{
			List<SoundConfigData> m_tempList = new List<SoundConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				SoundConfigData m_tempData = new SoundConfigData();
                m_tempData.name=m_datas[i][0];
					m_tempData.path=m_datas[i][1];
					
				if (!float.TryParse(m_datas[i][2].Trim(),out m_tempData.volume))
				{
					m_tempData.volume=0.0f;
				}

					m_tempData.remark=m_datas[i][3];
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public SoundConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.name == key);
        }

		public List<SoundConfigData> FindAll(Predicate<SoundConfigData> handler = null)
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

