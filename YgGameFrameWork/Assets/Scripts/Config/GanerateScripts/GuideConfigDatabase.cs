using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class GuideConfigData
    {
        /// <summary>
		///新手引导ID
		/// </summary>
		public int ID;
		/// <summary>
		///点击引导是否需要遮罩
		/// </summary>
		public int needMask;
		/// <summary>
		///是否需要文字
		/// </summary>
		public int needTxt;
		/// <summary>
		///文字界面位置
		/// </summary>
		public float TxtPosY;
		/// <summary>
		///文字描述
		/// </summary>
		public string strIndex;
		/// <summary>
		///奖励类型
		/// </summary>
		public int RewardType;
		/// <summary>
		///奖励值
		/// </summary>
		public Int64 RewardValue;
		/// <summary>
		///是否点击任意关闭
		/// </summary>
		public int anyClick2Close;
		/// <summary>
		///是否指定位置点击引导
		/// </summary>
		public int needClickGuide;
		/// <summary>
		///点击引导控件路径
		/// </summary>
		public string[] guideObjPath;
		/// <summary>
		///点击引导位置
		/// </summary>
		public float[] guideObjPos;
		/// <summary>
		///点击函数
		/// </summary>
		public string[] function;
		/// <summary>
		///点击引导动画animation
		/// </summary>
		public string skeAnimation;
		/// <summary>
		///下一步引导ID
		/// </summary>
		public int nextGuideID;
		/// <summary>
		///是否保存进度
		/// </summary>
		public int needSaveData;
		/// <summary>
		///同序列未保存ID
		/// </summary>
		public int[] saveIDs;
    }

    public class GuideConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =8;
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
                
				if (!int.TryParse(m_datas[i][0].Trim(),out m_tempData.ID))
				{
					m_tempData.ID=0;
				}

					
				if (!int.TryParse(m_datas[i][1].Trim(),out m_tempData.needMask))
				{
					m_tempData.needMask=0;
				}

					
				if (!int.TryParse(m_datas[i][2].Trim(),out m_tempData.needTxt))
				{
					m_tempData.needTxt=0;
				}

					
				if (!float.TryParse(m_datas[i][3].Trim(),out m_tempData.TxtPosY))
				{
					m_tempData.TxtPosY=0.0f;
				}

					m_tempData.strIndex=m_datas[i][4];
					
				if (!int.TryParse(m_datas[i][5].Trim(),out m_tempData.RewardType))
				{
					m_tempData.RewardType=0;
				}

					
					
				if (!int.TryParse(m_datas[i][7].Trim(),out m_tempData.anyClick2Close))
				{
					m_tempData.anyClick2Close=0;
				}

					
				if (!int.TryParse(m_datas[i][8].Trim(),out m_tempData.needClickGuide))
				{
					m_tempData.needClickGuide=0;
				}

					m_tempData.guideObjPath=CSVConverter.ConvertToArray<string>(m_datas[i][9].Trim());
					m_tempData.guideObjPos=CSVConverter.ConvertToArray<float>(m_datas[i][10].Trim());
					m_tempData.function=CSVConverter.ConvertToArray<string>(m_datas[i][11].Trim());
					m_tempData.skeAnimation=m_datas[i][12];
					
				if (!int.TryParse(m_datas[i][13].Trim(),out m_tempData.nextGuideID))
				{
					m_tempData.nextGuideID=0;
				}

					
				if (!int.TryParse(m_datas[i][14].Trim(),out m_tempData.needSaveData))
				{
					m_tempData.needSaveData=0;
				}

					m_tempData.saveIDs=CSVConverter.ConvertToArray<int>(m_datas[i][15].Trim());
				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public GuideConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.ID == int.Parse(key));
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

