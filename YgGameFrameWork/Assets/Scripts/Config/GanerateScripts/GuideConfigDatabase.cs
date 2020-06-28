using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class GuideConfigData
    {
        /// <summary>
		///引导Id
		/// </summary>
		public int Id;
		/// <summary>
		///引导类型
		/// </summary>
		public int type;
		/// <summary>
		///引导触发场景
		/// </summary>
		public string ui;
		/// <summary>
		///任务区域
		/// </summary>
		public int area;
		/// <summary>
		///任务资源
		/// </summary>
		public string guideResources;
		/// <summary>
		///引导面板
		/// </summary>
		public string guideWindow;
		/// <summary>
		///引导按钮
		/// </summary>
		public string btnName;
		/// <summary>
		///按钮遮罩类型
		/// </summary>
		public int btnMaskType;
		/// <summary>
		///引导手指位置
		/// </summary>
		public float[] fingerPos;
		/// <summary>
		///引导手指旋转角度
		/// </summary>
		public float fingerAngle;
		/// <summary>
		///是否显示引导手指
		/// </summary>
		public int showFinger;
		/// <summary>
		///自动完成等待时间
		/// </summary>
		public float waitTime;
		/// <summary>
		///达到指定条件完成
		/// </summary>
		public int needCondition;
		/// <summary>
		///下一步引导
		/// </summary>
		public int nextStep;
		/// <summary>
		///剧情内容
		/// </summary>
		public string[] dialogue;
		/// <summary>
		///剧情动画索引
		/// </summary>
		public string anim;
    }

    public class GuideConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =7;
        public const string DATA_PATH ="Config/GuideConfig";

        private List<GuideConfigData> m_datas;

        public  GuideConfigDatabase() { }

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

		private List<GuideConfigData> GetAllData(string[][] m_datas)
		{
			List<GuideConfigData> m_tempList = new List<GuideConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				GuideConfigData m_tempData = new GuideConfigData();
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.Id))
				{
					m_tempData.Id=0;
				}

					
				if (!int.TryParse(m_datas[i][1].Trim(),out m_tempData.type))
				{
					m_tempData.type=0;
				}

					m_tempData.ui=m_datas[i][2];
					
				if (!int.TryParse(m_datas[i][3].Trim(),out m_tempData.area))
				{
					m_tempData.area=0;
				}

					m_tempData.guideResources=m_datas[i][4];
					m_tempData.guideWindow=m_datas[i][5];
					m_tempData.btnName=m_datas[i][6];
					
				if (!int.TryParse(m_datas[i][7].Trim(),out m_tempData.btnMaskType))
				{
					m_tempData.btnMaskType=0;
				}

					m_tempData.fingerPos=CSVConverter.ConvertToArray<float>(m_datas[i][8].Trim());
					
				if (!float.TryParse(m_datas[i][9].Trim(),out m_tempData.fingerAngle))
				{
					m_tempData.fingerAngle=0.0f;
				}

					
				if (!int.TryParse(m_datas[i][10].Trim(),out m_tempData.showFinger))
				{
					m_tempData.showFinger=0;
				}

					
				if (!float.TryParse(m_datas[i][11].Trim(),out m_tempData.waitTime))
				{
					m_tempData.waitTime=0.0f;
				}

					
				if (!int.TryParse(m_datas[i][12].Trim(),out m_tempData.needCondition))
				{
					m_tempData.needCondition=0;
				}

					
				if (!int.TryParse(m_datas[i][13].Trim(),out m_tempData.nextStep))
				{
					m_tempData.nextStep=0;
				}

					m_tempData.dialogue=CSVConverter.ConvertToArray<string>(m_datas[i][14].Trim());
					m_tempData.anim=m_datas[i][15];
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public GuideConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.Id == int.Parse(key));
        }

		public List<GuideConfigData> FindAll(Predicate<GuideConfigData> handler = null)
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

