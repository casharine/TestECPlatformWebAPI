using TestECPlatform.Purchase.Main.Factory;
using TestECPlatform.Purchase.Models;
using Microsoft.VisualBasic;
using Oracle.ManagedDataAccess.Client;

using System.Data;
using WebAPIBasis.Accesors;

namespace TestECPlatform.Purchase.Utilities
{
    /// <summary>
    /// Purchaseクラス専用のUtility群です
    /// </summary>
    internal static class PurchaseUtilities
    {
        /// <summary>
        /// 購入パラメータのサイトコードに対応する購入クラスインスタンス生成します ※シンプルファクトリーパターン
        /// </summary>
        /// <param name="wPurchaseParameter"></param>
        /// <param name="accesors"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IPurchase createSelectedPurchase(PurchaseParameter wPurchaseParameter, IAccesors accesors)
        {
            IPurchase result = null!;

            try
            {
                // 購入クラス選定
                var target = wPurchaseParameter.SITE_CD switch
                {
                    "1001" => result = new PurchaseHOGE(wPurchaseParameter, accesors),
                    "1002" => result = new PurchaseFUGA(wPurchaseParameter, accesors),
                    "1003" => result = new PurchasePIYO(wPurchaseParameter, accesors),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            catch (Exception ex)
            {
                accesors.getLoggerClient().WriteLog("createSelectedPurchase", $"コード{accesors.getCode()}の購入クラス生成に失敗しました");
                throw;
            }
            return result;
        }

        /// <summary>
        /// 自サイト購入true、連携先購入falseを返します。購入パラメータのサイトと所属から自サイト購入か判定します。
        /// </summary>
        /// <param name="iPurchaseParameter"></param>
        /// <returns></returns>
        public static bool getIsMySite(PurchaseParameter iPurchaseParameter)
        {
            var b = iPurchaseParameter.SHOZOKU_CD == iPurchaseParameter.SITE_CD ? true : false;
            return b;
        }

        /// <summary>
        /// 購入パラメータのログメッセージ定型文です
        /// </summary>
        /// <param name="iPurchaseParameter"></param>
        /// <returns></returns>
        public static string getPurchaseParamLogMessage(PurchaseParameter iPurchaseParameter)
        {
            return $"\n USER_ID:{iPurchaseParameter.USER_ID} \n SHOZOKU_CD:{iPurchaseParameter.SHOZOKU_CD} \n SHOHIN_CD:{iPurchaseParameter.SITE_CD} \n SITE_CD:{iPurchaseParameter.SHOHIN_CD} \n ";
        }
    }
}