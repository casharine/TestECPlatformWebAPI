namespace TestECPlatform.Purchase.Models
{
    /// <summary>
    ///  花き仲購入処理結果クラス
    ///  </summary>
    ///  <remarks></remarks>
    public class PurchaseResult : ICloneable
    {
        public int ErrorCode { get; set; } = 0;
        public string ErrorMessage { get; set; } = string.Empty;
        public long CountLockFailed { get; set; } = 0;


        // クローンメソッドの実装
        public object Clone()
        {
            return new PurchaseResult
            {
                ErrorCode = this.ErrorCode,
                ErrorMessage = this.ErrorMessage,
                CountLockFailed = this.CountLockFailed
            };
        }
    }

}
