namespace TestECPlatform.Purchase.Models
{
    /// <summary>
    ///  花き仲購入処理結果クラス
    ///  </summary>
    ///  <remarks></remarks>
    public class PurchaseResult
    {
        /// <summary>エラーコード</summary>
        public int ErrorCode { get; set; } = 0;
        /// <summary>メッセージ</summary>
        public string ErrorMessage { get; set; } = string.Empty;
        /// <summary>残数</summary>
        public long CountLockFailed { get; set; } = 0;

    }
}
