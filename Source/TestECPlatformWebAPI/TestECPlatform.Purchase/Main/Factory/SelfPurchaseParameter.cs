using TestECPlatform.Purchase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIBasis.Accesors;
using WebAPIBasis.Utilities;
using Oracle.ManagedDataAccess.Client;

namespace TestECPlatform.Purchase.Main.Factory
{
    /// <summary>
    /// 自社購入時の購入パラメータを生成します
    /// </summary>
    class SelfPurchaseParameter : IPurchaseParameter
    {
        /// <summary>
        /// 自社購入時の購入パラメータを生成します
        /// </summary>
        public PurchaseParameter getParameter(PurchaseParameter iPurchaseParameter, IAccesors accesors)
        {
            // 自サイト購入または連携購入か
            iPurchaseParameter.IsMySite = iPurchaseParameter.SITE_CD == iPurchaseParameter.SHOHIN_CD ? true : false;

            return iPurchaseParameter;
        }
    }
}
