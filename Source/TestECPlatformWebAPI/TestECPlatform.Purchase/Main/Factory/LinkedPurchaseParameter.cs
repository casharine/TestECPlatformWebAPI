using TestECPlatform.Purchase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIBasis.Accesors;
using WebAPIBasis.Utilities;
using Oracle.ManagedDataAccess.Client;
using System.Reflection.Metadata;

namespace TestECPlatform.Purchase.Main.Factory
{
    /// <summary>
    /// 連携購入時の購入パラメータを生成します
    /// </summary>
    class LinkedPurchaseParameter : IPurchaseParameter
    {
        /// <summary>
        /// 連携購入時の購入パラメータを取得します
        /// </summary>
        public PurchaseParameter getParameter(PurchaseParameter iPurchaseParameter, IAccesors iAccessors)
        {
            var wPurchaseParameter = new PurchaseParameter();

            try
            {            
                // 連携先で購入可能なパラメータに変換処理
            }
            catch (OracleException exOra)
            {
				iAccessors.getLoggerClient().WriteLog(iAccessors.getConnection(), "ex.LinkedPurchaseParameter", $"所属{iPurchaseParameter.SHOZOKU_CD}から商品{iPurchaseParameter.SHOHIN_CD}へPurchaseParameter変換処理 {exOra.ToString()!}");
			}
			catch (Exception ex)
			{
                iAccessors.getLoggerClient().WriteLog(iAccessors.getConnection(), "ex.LinkedPurchaseParameter", $"所属{iPurchaseParameter.SHOZOKU_CD}から商品{iPurchaseParameter.SHOHIN_CD}へPurchaseParameter変換処理 {ex.ToString()!}");
			}
            finally
            {
            }
        return wPurchaseParameter;
        }
    }
}
