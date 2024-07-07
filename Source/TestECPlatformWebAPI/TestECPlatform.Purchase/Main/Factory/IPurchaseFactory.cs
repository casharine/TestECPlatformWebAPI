using TestECPlatform.Purchase.Models;

namespace TestECPlatform.Purchase.Main.Factory
{
    public interface IPurchaseFactory
    {
        public IPurchase CreatePurchase(PurchaseParameter iPurchaseParameter)
        {
            IPurchase Purchase = createPurchase(iPurchaseParameter);
            return Purchase;
        }

        protected  IPurchase createPurchase(PurchaseParameter purchaseParameter);
    }
}
