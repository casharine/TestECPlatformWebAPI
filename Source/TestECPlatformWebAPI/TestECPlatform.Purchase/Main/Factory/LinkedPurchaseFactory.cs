using TestECPlatform.Purchase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestECPlatform.Purchase.Utilities;
using WebAPIBasis.Accesors;
using WebAPIBasis.ApiBuilder;
using WebAPIBasis.Utilities;

namespace TestECPlatform.Purchase.Main.Factory
{
    class LinkedPurchaseFactory : IPurchaseFactory {

        /// <summary>
        /// 各の自社購入、連携購入を生成するファクトリ
        /// </summary>
        /// <param name="iPurchaseParameter"></param>
        /// <returns></returns>
        public IPurchase createPurchase(PurchaseParameter iPurchaseParameter)
        {
            // 連携元コンフィグ、DB、ロガーアクセサー群を取得
            var accesorsClient = ApiBuilder.Create().Code(iPurchaseParameter.SITE_CD).Build();
            accesorsClient.getLoggerClient().WriteLog( CommonUtils.GetMyName(), "変換前：" + PurchaseUtilities.getPurchaseParamLogMessage(iPurchaseParameter));

            // 連携購入用購入パラメータ生成
            var wPurchaseParameter = new LinkedPurchaseParameter().getParameter(iPurchaseParameter, accesorsClient);

            // 連携先コンフィグ、DB、ロガーアクセサー群を取得
            var accesorsHost = ApiBuilder.Create().Code(iPurchaseParameter.SHOHIN_CD).Build();
            accesorsHost.getLoggerClient().WriteLog(CommonUtils.GetMyName(), "変換後：" + PurchaseUtilities.getPurchaseParamLogMessage(iPurchaseParameter));

            // 購入クラス生成
            IPurchase Purchase = PurchaseUtilities.createSelectedPurchase(wPurchaseParameter, accesorsHost);

            return Purchase;
        }
    }
}
