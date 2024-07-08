using TestECPlatform.Purchase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestECPlatform.Purchase.Main.Factory
{
    /// <summary>
    /// 購入処理の抽象インターフェイス
    /// </summary>
    public interface IPurchase
    {
        public PurchaseResult purchase();
    }
}
