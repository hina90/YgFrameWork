using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tool.Database
{
    public class GlobalConfigData
    {
        /// <summary>
		///初始金币
		/// </summary>
		public Double InitPlayerGold;
		/// <summary>
		///初始钻石
		/// </summary>
		public Double InitPlayerDiamond;
		/// <summary>
		///NPC参观时间
		/// </summary>
		public float visitTime;
		/// <summary>
		///点击速度倍率
		/// </summary>
		public float ClickSpeed;
		/// <summary>
		///看广告得钻石数
		/// </summary>
		public int[] AdsDCount;
		/// <summary>
		///看广告得钻石权重
		/// </summary>
		public int[] AdsDWeight;
		/// <summary>
		///基础离线收益时间（分）
		/// </summary>
		public int BaseOffRewardTime;
		/// <summary>
		///离线收益倍率
		/// </summary>
		public float OffRewardRate;
		/// <summary>
		///离线收益广告倍率
		/// </summary>
		public int OffRewardAdsRate;
		/// <summary>
		///基础经验
		/// </summary>
		public int baseEXP;
		/// <summary>
		///签到看广告倍数
		/// </summary>
		public int SingleInAdsRate;
		/// <summary>
		///双倍收益刷新间隔
		/// </summary>
		public float DoubleIncomeCD;
		/// <summary>
		///双倍收益持续时间
		/// </summary>
		public float DoubleIncomeLast;
		/// <summary>
		///双倍收益出现概率
		/// </summary>
		public float DoubleIncomeRate;
		/// <summary>
		///双倍收益效果时长（秒）
		/// </summary>
		public float DoubleIncomeEffLast;
		/// <summary>
		///升级广告倍率
		/// </summary>
		public int LvlUpgradeRate;
		/// <summary>
		///免费转盘CD(秒)
		/// </summary>
		public float FreeTurnCD;
		/// <summary>
		///转盘CD(秒)
		/// </summary>
		public float LuckyTurnCD;
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
		public Double minUfoMoney;
		/// <summary>
		///顾客招聘参数1
		/// </summary>
		public float guestHireParam1;
		/// <summary>
		///顾客招聘参数2
		/// </summary>
		public float guestHireParam2;
		/// <summary>
		///顾客招聘参数3
		/// </summary>
		public int guestHireParam3;
		/// <summary>
		///顾客招聘参数4
		/// </summary>
		public int guestHireParam4;
		/// <summary>
		///顾客背包最小个数
		/// </summary>
		public int minGuestCountInBag;
    }

    public class GlobalConfigDatabase : IDatabase
    {
        public const uint TYPE_ID =7;
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
                
					
					
				if (!float.TryParse(m_datas[i][2].Trim(),out m_tempData.visitTime))
				{
					m_tempData.visitTime=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][3].Trim(),out m_tempData.ClickSpeed))
				{
					m_tempData.ClickSpeed=0.0f;
				}

					m_tempData.AdsDCount=CSVConverter.ConvertToArray<int>(m_datas[i][4].Trim());
					m_tempData.AdsDWeight=CSVConverter.ConvertToArray<int>(m_datas[i][5].Trim());
					
				if (!int.TryParse(m_datas[i][6].Trim(),out m_tempData.BaseOffRewardTime))
				{
					m_tempData.BaseOffRewardTime=0;
				}

					
				if (!float.TryParse(m_datas[i][7].Trim(),out m_tempData.OffRewardRate))
				{
					m_tempData.OffRewardRate=0.0f;
				}

					
				if (!int.TryParse(m_datas[i][8].Trim(),out m_tempData.OffRewardAdsRate))
				{
					m_tempData.OffRewardAdsRate=0;
				}

					
				if (!int.TryParse(m_datas[i][9].Trim(),out m_tempData.baseEXP))
				{
					m_tempData.baseEXP=0;
				}

					
				if (!int.TryParse(m_datas[i][10].Trim(),out m_tempData.SingleInAdsRate))
				{
					m_tempData.SingleInAdsRate=0;
				}

					
				if (!float.TryParse(m_datas[i][11].Trim(),out m_tempData.DoubleIncomeCD))
				{
					m_tempData.DoubleIncomeCD=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][12].Trim(),out m_tempData.DoubleIncomeLast))
				{
					m_tempData.DoubleIncomeLast=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][13].Trim(),out m_tempData.DoubleIncomeRate))
				{
					m_tempData.DoubleIncomeRate=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][14].Trim(),out m_tempData.DoubleIncomeEffLast))
				{
					m_tempData.DoubleIncomeEffLast=0.0f;
				}

					
				if (!int.TryParse(m_datas[i][15].Trim(),out m_tempData.LvlUpgradeRate))
				{
					m_tempData.LvlUpgradeRate=0;
				}

					
				if (!float.TryParse(m_datas[i][16].Trim(),out m_tempData.FreeTurnCD))
				{
					m_tempData.FreeTurnCD=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][17].Trim(),out m_tempData.LuckyTurnCD))
				{
					m_tempData.LuckyTurnCD=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][18].Trim(),out m_tempData.ufoSpeed))
				{
					m_tempData.ufoSpeed=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][19].Trim(),out m_tempData.ufoLastTime))
				{
					m_tempData.ufoLastTime=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][20].Trim(),out m_tempData.ufoRefreshTime))
				{
					m_tempData.ufoRefreshTime=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][21].Trim(),out m_tempData.ufoRate))
				{
					m_tempData.ufoRate=0.0f;
				}

					
				if (!int.TryParse(m_datas[i][22].Trim(),out m_tempData.minUfoMoneyTime))
				{
					m_tempData.minUfoMoneyTime=0;
				}

					
				if (!int.TryParse(m_datas[i][23].Trim(),out m_tempData.maxUfoMoneyTime))
				{
					m_tempData.maxUfoMoneyTime=0;
				}

					
					
				if (!float.TryParse(m_datas[i][25].Trim(),out m_tempData.guestHireParam1))
				{
					m_tempData.guestHireParam1=0.0f;
				}

					
				if (!float.TryParse(m_datas[i][26].Trim(),out m_tempData.guestHireParam2))
				{
					m_tempData.guestHireParam2=0.0f;
				}

					
				if (!int.TryParse(m_datas[i][27].Trim(),out m_tempData.guestHireParam3))
				{
					m_tempData.guestHireParam3=0;
				}

					
				if (!int.TryParse(m_datas[i][28].Trim(),out m_tempData.guestHireParam4))
				{
					m_tempData.guestHireParam4=0;
				}

					
				if (!int.TryParse(m_datas[i][29].Trim(),out m_tempData.minGuestCountInBag))
				{
					m_tempData.minGuestCountInBag=0;
				}

				m_tempList.Add(m_tempData);
            }
            return m_tempList;
		}

        public GlobalConfigData GetDataByKey(string key)
        {
			return m_datas.Find(temp => temp.InitPlayerGold == Double.Parse(key));
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

