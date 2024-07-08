using TestECPlatform.Purchase.Models;

namespace TestECPlatform.Purchase.Main.Factory
{
    public interface IPurchaseFactory
    {
        /// <summary>
        /// 購入処理のファクトリクラス
        /// </summary>
        /// <param name="iPurchaseParameter"></param>
        /// <returns></returns>
        public IPurchase CreatePurchase(PurchaseParameter iPurchaseParameter)
        {
            IPurchase Purchase = createPurchase(iPurchaseParameter);
            return Purchase;
        }

        protected  IPurchase createPurchase(PurchaseParameter purchaseParameter);
    }
}
