using TestECPlatform.Purchase.Main.Factory;
using TestECPlatform.Purchase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIBasis.Accesors;
using WebAPIBasis.Literals;
using WebAPIBasis.Utilities;

namespace TestECPlatform.Purchase.Main
{
    /// <summary>
    /// 連携購入クラスを生成し実行します
    /// </summary>
    public class PurchaseConnector
    {
        /// <summary>
        /// 連携購入クラスを生成し実行します
        /// </summary>
        public void purchaseLink(PurchaseParameter iPurchaseParameter, ref PurchaseResult rPurchaseResult)
        {
            IPurchase Purchase = null!;

            // 連携先購入クラス、パラメータ生成
            try
            {
                Purchase = new LinkedPurchaseFactory().createPurchase(iPurchaseParameter);
            }catch (APIException ex){
                throw ex;
            }

            // 購入処理実行
            var wPurchaseResult = Purchase.purchase();

            // 外部連携結果を更新する
            rPurchaseResult.ErrorCode = wPurchaseResult.ErrorCode;
            rPurchaseResult.CountLockFailed = wPurchaseResult.CountLockFailed;

            // エラー処理
            if (wPurchaseResult.ErrorCode != 0)
            {
                rPurchaseResult.ErrorMessage = CommonUtils.DisplayEnumByInt(new PurchaseRes(), rPurchaseResult.ErrorCode, "外部連携エラー ");
                rPurchaseResult.ErrorCode = -9000;
            }
            else
            {
                rPurchaseResult.ErrorMessage = wPurchaseResult.ErrorMessage;
            }
        }
    }
}
