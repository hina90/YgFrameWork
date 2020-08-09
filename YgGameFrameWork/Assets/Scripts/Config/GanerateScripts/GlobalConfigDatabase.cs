using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class GlobalConfigData
    {
        /// <summary>
		///ufo速度
		/// </summary>
		public float ufoSpeed;
		/// <summary>
		///ufo持续时间
		/// </summary>
		public float ufoLastTime;
		/// <summary>
		///ufo刷新时间 秒
		/// </summary>
		public float ufoRefreshTime;
		/// <summary>
		///钱不够升级展台时刷新ufo时间 秒
		/// </summary>
		public float ufoRefreshExtTime;
		/// <summary>
		///ufo出现几率
		/// </summary>
		public float ufoRate;
		/// <summary>
		///ufo金币随机参数下限（3min）
		/// </summary>
		public int minUfoMoneyTime;
		/// <summary>
		///ufo金币随机参数上限（5min）
		/// </summary>
		public int maxUfoMoneyTime;
		/// <summary>
		///ufo金币下限
		/// </summary>
		public int minUfoMoney;
		/// <summary>
		///幸运电话刷新间隔
		/// </summary>
		public float LuckPhoneCD;
		/// <summary>
		///幸运电话持续时间
		/// </summary>
		public float LuckPhoneLast;
		/// <summary>
		///幸运电话出现概率
		/// </summary>
		public float LuckPhoneRate;
		/// <summary>
		///幸运电话效果时长
		/// </summary>
		public float LuckPhoneEffLast;
		/// <summary>
		///幸运电话效果倍率
		/// </summary>
		public float LuckPhoneEffRate;
		/// <summary>
		///2阶段品质收入系数
		/// </summary>
		public float exbtIncomeP2;
		/// <summary>
		///3阶段品质收入系数
		/// </summary>
		public float exbtIncomeP3;
		/// <summary>
		///出现表情概率
		/// </summary>
		public float qipaoRate;
		/// <summary>
		///表情1点击奖励
		/// </summary>
		public int[] qipao01;
		/// <summary>
		///表情2点击奖励
		/// </summary>
		public int[] qipao02;
		/// <summary>
		///表情3点击奖励
		/// </summary>
		public int[] qipao03;
		/// <summary>
		///表情4点击奖励
		/// </summary>
		public int[] qipao04;
		/// <summary>
		///表情5点击奖励
		/// </summary>
		public int[] qipao05;
		/// <summary>
		///看广告得钻石数
		/// </summary>
		public int[] AdsDCount;
		/// <summary>
		///看广告得钻石权重
		/// </summary>
		public int[] AdsDWeight;
		/// <summary>
		///离线收益倍率
		/// </summary>
		public float OffRewardRate;
		/// <summary>
		///挖掘物转换钻石数量（展品）
		/// </summary>
		public int DigMaxReward;
		/// <summary>
		///挖掘物转换钻石数量（装饰物）
		/// </summary>
		public int DigMaxReward2;
		/// <summary>
		///基础离线收益时间（分）
		/// </summary>
		public int BaseOffRewardTime;
		/// <summary>
		///离线收益广告倍率
		/// </summary>
		public int OffRewardAdsRate;
    }

    public class GlobalConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =6;
        public const string DATA_PATH ="Config/GlobalConfig";

        private List<GlobalConfigData> m_datas;

        public  GlobalConfigDatabase() { }

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

		private List<GlobalConfigData> GetAllData(string[][] m_datas)
		{
			List<GlobalConfigData> m_tempList = new List<GlobalConfigData>();
			for (int i = 0; i < m_datas.Length; i++)
            {
				GlobalConfigData m_tempData = new GlobalConfigData();
                
				if (!float.TryParse(m_datas[i][0].Trim(),out m_tempData.ufoSpeed))
				{
					m_tempData.ufoSpeed=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][1].Trim(),out m_tempData.ufoLastTime))
				{
					m_tempData.ufoLastTime=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][2].Trim(),out m_tempData.ufoRefreshTime))
				{
					m_tempData.ufoRefreshTime=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][3].Trim(),out m_tempData.ufoRefreshExtTime))
				{
					m_tempData.ufoRefreshExtTime=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][4].Trim(),out m_tempData.ufoRate))
				{
					m_tempData.ufoRate=0.0f;
				}

					
				if (!int.TryParse(m_datas[i][5].Trim(),out m_tempData.minUfoMoneyTime))
				{
					m_tempData.minUfoMoneyTime=0;
				}

					
				if (!int.TryParse(m_datas[i][6].Trim(),out m_tempData.maxUfoMoneyTime))
				{
					m_tempData.maxUfoMoneyTime=0;
				}

					
				if (!int.TryParse(m_datas[i][7].Trim(),out m_tempData.minUfoMoney))
				{
					m_tempData.minUfoMoney=0;
				}

					
				if (!float.TryParse(m_datas[i][8].Trim(),out m_tempData.LuckPhoneCD))
				{
					m_tempData.LuckPhoneCD=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][9].Trim(),out m_tempData.LuckPhoneLast))
				{
					m_tempData.LuckPhoneLast=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][10].Trim(),out m_tempData.LuckPhoneRate))
				{
					m_tempData.LuckPhoneRate=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][11].Trim(),out m_tempData.LuckPhoneEffLast))
				{
					m_tempData.LuckPhoneEffLast=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][12].Trim(),out m_tempData.LuckPhoneEffRate))
				{
					m_tempData.LuckPhoneEffRate=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][13].Trim(),out m_tempData.exbtIncomeP2))
				{
					m_tempData.exbtIncomeP2=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][14].Trim(),out m_tempData.exbtIncomeP3))
				{
					m_tempData.exbtIncomeP3=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][15].Trim(),out m_tempData.qipaoRate))
				{
					m_tempData.qipaoRate=0.0f;
				}

					m_tempData.qipao01=CSVConverter.ConvertToArray<int>(m_datas[i][16].Trim());
					m_tempData.qipao02=CSVConverter.ConvertToArray<int>(m_datas[i][17].Trim());
					m_tempData.qipao03=CSVConverter.ConvertToArray<int>(m_datas[i][18].Trim());
					m_tempData.qipao04=CSVConverter.ConvertToArray<int>(m_datas[i][19].Trim());
					m_tempData.qipao05=CSVConverter.ConvertToArray<int>(m_datas[i][20].Trim());
					m_tempData.AdsDCount=CSVConverter.ConvertToArray<int>(m_datas[i][21].Trim());
					m_tempData.AdsDWeight=CSVConverter.ConvertToArray<int>(m_datas[i][22].Trim());
					
				if (!float.TryParse(m_datas[i][23].Trim(),out m_tempData.OffRewardRate))
				{
					m_tempData.OffRewardRate=0.0f;
				}

					
				if (!int.TryParse(m_datas[i][24].Trim(),out m_tempData.DigMaxReward))
				{
					m_tempData.DigMaxReward=0;
				}

					
				if (!int.TryParse(m_datas[i][25].Trim(),out m_tempData.DigMaxReward2))
				{
					m_tempData.DigMaxReward2=0;
				}

					
				if (!int.TryParse(m_datas[i][26].Trim(),out m_tempData.BaseOffRewardTime))
				{
					m_tempData.BaseOffRewardTime=0;
				}

					
				if (!int.TryParse(m_datas[i][27].Trim(),out m_tempData.OffRewardAdsRate))
				{
					m_tempData.OffRewardAdsRate=0;
				}

				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public GlobalConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.ufoSpeed == float.Parse(key));
        }

		public List<GlobalConfigData> FindAll(Predicate<GlobalConfigData> handler = null)
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

