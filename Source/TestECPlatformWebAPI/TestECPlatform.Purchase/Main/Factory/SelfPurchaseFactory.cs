using TestECPlatform.Purchase.Models;
using TestECPlatform.Purchase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIBasis.Accesors;

namespace TestECPlatform.Purchase.Main.Factory
{
    /// <summary>
    /// 自社購入に必要な１．購入クラス、２．購入パラメータ、３．各種アクセサを一括生成するAbstractFactoryです
    /// </summary>
    public class SelfPurchaseFactory : IPurchaseFactory
    {

        /// <summary>
        /// 自社購入に必要な１．購入クラス、２．購入パラメータ、３．各種アクセサを一括生成します
        /// </summary>
        public IPurchase createPurchase(PurchaseParameter iPurchaseParameter)
        {
            // コンフィグ、DB、ロガーアクセサー群を取得
            IAccesors AccesorsCurrent = new ClientAccesors(iPurchaseParameter.SITE_CD);

            // 購入パラメータ作成
            var wPurchaseParameter = new SelfPurchaseParameter().getParameter(iPurchaseParameter, AccesorsCurrent);

            // 購入クラス生成
            IPurchase Purchase = PurchaseUtilities.createSelectedPurchase(wPurchaseParameter, AccesorsCurrent);

            return Purchase;
        }
    }
}
