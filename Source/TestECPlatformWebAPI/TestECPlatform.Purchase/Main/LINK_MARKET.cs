using KLINK.Collaborations.Contracts.Kounyu.Models;
using KNN.Collaborations.Contracts.Kounyu.Models;
using KNN.Collaborations.Contracts.Logics.Literals;
using KNN.Collaborations.Contracts.Logics.Logics;
using KNN.Collaborations.Contracts.Logics.Models;
using KNN.Collaborations.Contracts.Logics.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualBasic;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIBasis.Utilities;

namespace KNN.Collaborations.Contracts.Kounyu.Logics
{
    /// <summary>
    /// 市場連携購入のためのクラスです
    /// </summary>
    internal class LINK_MARKET
    {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="">サイト名</param>
        public LINK_MARKET()
        {
        }

        /// <summary>
        /// フローレサイトに連携してフローレサイトで購入します
        /// </summary>
        /// <param name="rKounyuParameter"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public void LinkToOthers(KounyuParameter iKounyuParameter, ref KNNBuyingResult resCaller)
        {
            //var getJyojyo = new GetJyojyo();
            var clsKounyu = new CLS_KOUNYU("");
            var resCalled = new BuyingResult();
            var rKounyuParameter = new KounyuParameter(iKounyuParameter);
            var dt = new DataTable();

            // フローレ購入用にパラメータを変換
            ConvertKounyuParameterForFlore(ref rKounyuParameter, siteName);

            try
            {
                Logger.WriteLog("send.FLORE_PP.BuyingLinkToFL", $"USER_ID:{rKounyuParameter.USER_ID} OROSI_CD_SHOZOKU:{rKounyuParameter.OROSI_CD_SHOZOKU}  BSAN_CD_LIST:{rKounyuParameter.BSAN_CD_LIST[0]} SEIK_CD_LIST:{rKounyuParameter.SEIK_CD_LIST[0]} JYOJYO_P_KEY_LIST:{rKounyuParameter.JYOJYO_P_KEY_LIST[0]} JYOJYO_SUB_P_KEY_LIST:{rKounyuParameter.JYOJYO_SUB_P_KEY_LIST[0]}, rKounyuParameter.KIBO_TANKA:{rKounyuParameter.KIBO_TANKA}", siteName);

                // 支店間取引時以外は単価・入数の一致をチェック、変更時は終了
                //if (siteName != "Flore" && iKounyuParameter.KIBO_TANKA != 0)
                //{
                //	if (false == getJyojyo.IsAbleToBuy(rKounyuParameter.SHOHIN_OROSI_CD[0], rKounyuParameter.USER_ID, rKounyuParameter.JYOJYO_P_KEY_LIST[0], rKounyuParameter.JYOJYO_SUB_P_KEY_LIST[0], rKounyuParameter.KIBO_TANKA, rKounyuParameter.IRISU_LIST[0]))
                //	{
                //		resCaller.ErrorMessage = "商品情報が変更されました。（単価、入数)";
                //		resCaller.ErrorCode = -9010;
                //		Logger.WriteLog("FLORE_PP.BuyingLinkToFL.IsAbleToBuy", $"ErrorCode:{resCalled.ErrorCode} ErrorMessage:{resCalled.ErrorMessage} return_ShiireKey:{resCalled.return_ShiireKey} return_ShiireMeisaiKey:{resCalled.return_ShiireMeisaiKey} return_JuchuuKey:{resCalled.return_JuchuuKey} return_JuchuuMeisaiKey:{resCalled.return_JuchuuMeisaiKey} return_HikiateMeisaiKey:{resCalled.return_HikiateMeisaiKey} sync_JFESeiyakuIriSuu:{resCalled.sync_JFESeiyakuIriSuu} sync_JFESeiyakuKuchiSuu:{resCalled.sync_JFESeiyakuKuchiSuu} SEIK_P_KEY:{resCalled.SEIK_P_KEY} JIOROSI_CD:{resCalled.JIOROSI_CD} zansu:{resCalled.zansu}", siteName);
                //		return;
                //	}
                //}

                cn.Open();

                // フラワーロードがフローレサイトで購入処理実行 
                resCalled = clsKounyu.KounyuLinkFL(rKounyuParameter);

                Logger.WriteLog(cn, "retun.FLORE_PP.BuyingLinkToFL", $"ErrorCode:{resCalled.ErrorCode} ErrorMessage:{resCalled.ErrorMessage} return_ShiireKey:{resCalled.return_ShiireKey} return_ShiireMeisaiKey:{resCalled.return_ShiireMeisaiKey} return_JuchuuKey:{resCalled.return_JuchuuKey} return_JuchuuMeisaiKey:{resCalled.return_JuchuuMeisaiKey} return_HikiateMeisaiKey:{resCalled.return_HikiateMeisaiKey} sync_JFESeiyakuIriSuu:{resCalled.sync_JFESeiyakuIriSuu} sync_JFESeiyakuKuchiSuu:{resCalled.sync_JFESeiyakuKuchiSuu} SEIK_P_KEY:{resCalled.SEIK_P_KEY} JIOROSI_CD:{resCalled.JIOROSI_CD} zansu:{resCalled.zansu}");
                cn.Close();

                // 板橋時に使用した[買参人コード][買参人コード枝番]をアンダーバー区切りで代入している
                resCaller.BIKO_KN = resCalled.BIKO_KN;

                //  戻り値を代入
                resCaller.ErrorCode = resCalled.ErrorCode;
                resCaller.ErrorMessage = resCalled.ErrorMessage;
                resCaller.SEIK_P_KEY = resCalled.SEIK_P_KEY;
                resCaller.zansu = resCalled.zansu;
                resCaller.sync_JFESeiyakuKuchiSuu = resCalled.sync_JFESeiyakuKuchiSuu;
                resCaller.sync_JFESeiyakuIriSuu = resCalled.sync_JFESeiyakuIriSuu;

            }
            catch (OracleException exOra)
            {
                Logger.WriteLog("ex.FLORE_PP.BuyingLinkToFL", exOra.ToString()!, siteName);
                resCaller.ErrorMessage = "データ例外(" + exOra.ToString();
                resCaller.ErrorCode = -7000;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ex.FLORE_PP.BuyingLinkToFL", ex.ToString()!, siteName);
                resCaller.ErrorCode = -9999;
                resCaller.ErrorMessage = ex.ToString();
            }
            finally
            {
                cn.Dispose();
            }
        }

        /// <summary>
        /// Json及びDBからフローレサイト購入のためのパラメータを取得し変換します
        /// </summary>
        /// <param name="kounyuparameter"></param>
        /// <returns></returns>
        public void ConvertKounyuParameterForFlore(ref KounyuParameter rKounyuParameter, string siteName)
        {
            try
            {
                cn.Open();
                Logger.WriteLog(cn, "beforeConvert.BuyingLinkToFL", $"USER_ID:{rKounyuParameter.USER_ID} OROSI_CD_SHOZOKU:{rKounyuParameter.OROSI_CD_SHOZOKU}  BSAN_CD_LIST:{rKounyuParameter.BSAN_CD_LIST[0]} SEIK_CD_LIST:{rKounyuParameter.SEIK_CD_LIST[0]} JYOJYO_P_KEY_LIST:{rKounyuParameter.JYOJYO_P_KEY_LIST[0]} JYOJYO_SUB_P_KEY_LIST:{rKounyuParameter.JYOJYO_SUB_P_KEY_LIST[0]}");

                // 共通変換処理
                rKounyuParameter.GYOSHA_KBN = "1";

                // 取引先別変換処理
                if (siteName == "Flore" && rKounyuParameter.ICHIBA_KBN == "0" && rKounyuParameter.SHOHIN_OROSI_CD[0].ToString() != rKounyuParameter.OROSI_CD_SHOZOKU)
                {
                    // FL支店間取引時
                    rKounyuParameter.RENKEIMOTO_OROSI_CD_SUB = rKounyuParameter.RENKEIMOTO_OROSI_CD;
                    rKounyuParameter.RENKEIMOTO_OROSI_CD = rKounyuParameter.OROSI_CD_SHOZOKU;

                    string strBanchSQL = "SELECT SITE_ID, BSAN_CD, EDA_CD FROM M_KOUKAI_ORSI WHERE OROSI_CD = '" + rKounyuParameter.OROSI_CD_SHOZOKU + "' and KOUKAI_OROSI_CD ='" + rKounyuParameter.SHOHIN_OROSI_CD[0] + "'";
                    var dtBranch = new DBfunctions().executeSelectQuery(strBanchSQL, cn);
                    rKounyuParameter.USER_ID = dtBranch.Rows[0]["SITE_ID"].ToString()!;
                    rKounyuParameter.OROSI_CD_SHOZOKU = rKounyuParameter.SHOHIN_OROSI_CD[0];
                    rKounyuParameter.BSAN_CD_LIST[0] = dtBranch.Rows[0]["BSAN_CD"].ToString()!;
                    rKounyuParameter.SEIK_CD_LIST[0] = dtBranch.Rows[0]["EDA_CD"].ToString()!;
                }
                else
                {
                    // 他市場から連携時
                    rKounyuParameter.RENKEIMOTO_OROSI_CD_SUB = "0";
                    rKounyuParameter.RENKEIMOTO_OROSI_CD = rKounyuParameter.OROSI_CD_SHOZOKU;

                    rKounyuParameter.USER_ID = cnf[$"AppSettings:{siteName}:LinkToFlore:FLORE_USER_ID"];
                    rKounyuParameter.OROSI_CD_SHOZOKU = cnf[$"AppSettings:{siteName}:LinkToFlore:FLORE_OROSI_CD_SHOZOKU"];
                    rKounyuParameter.BSAN_CD_LIST[0] = cnf[$"AppSettings:{siteName}:LinkToFlore:FLORE_BSAN_CD"];
                    rKounyuParameter.SEIK_CD_LIST[0] = cnf[$"AppSettings:{siteName}:LinkToFlore:FLORE_SEIK_CD"];

                    string strSQL = $"SELECT ORG_JYOJYO_P_KEY, LAST_TIMESTAMP FROM T_JYOJYO WHERE P_KEY = {rKounyuParameter.JYOJYO_P_KEY_LIST[0]}";
                    var dt = new DBfunctions().executeSelectQuery(strSQL, cn);
                    rKounyuParameter.JYOJYO_P_KEY_LIST[0] = Convert.ToDecimal(dt.Rows[0]["ORG_JYOJYO_P_KEY"].ToString()!);
                    rKounyuParameter.JYOJYO_SUB_P_KEY_LIST[0] = Convert.ToDecimal(dt.Rows[0]["LAST_TIMESTAMP"].ToString()!);
                    rKounyuParameter.GYOSHA_KBN = "1";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(cn, "ex.FLORE_PP.ConvertKounyuParameterForFlore", ex.ToString()!);
            }
            finally
            {
                cn.Close();
            }
        }
    }
}
