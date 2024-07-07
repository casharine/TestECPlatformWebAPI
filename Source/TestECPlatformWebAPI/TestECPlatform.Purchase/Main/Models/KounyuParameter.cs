
namespace TestECPlatform.Purchase.Models
{
    /// <summary>
    ///  購入処理用パラメータ
    ///  </summary>
    ///  <remarks></remarks>
    public class PurchaseParameter
{
        /// <summary>ユーザID</summary>
        public string USER_ID { get; set; }
        /// <summary>買参人の所属コード</summary>
        public string SHOZOKU_CD { get; set; }
        /// <summary>購入した商品のコード</summary>
        public string SHOHIN_CD { get; set; }
        /// <summary>商品を購入したWebサイトのコード</summary>
        public string SITE_CD { get; set; }
        /// <summary>連携元のコード ※def:""</summary>
        public string CLIENT_SITE_CD { get; set; }
        /// <summary>自サイト購入またはサイト連携購入</summary>
        public bool IsMySite { get; set; }

        /// <summary>
        /// コンストラクタ(自サイト商品購入用＝コントローラから直接呼び出された場合に使用)
        /// </summary>
        public PurchaseParameter()
        {
            this.USER_ID = "";
            this.SHOZOKU_CD = "";
            this.SHOHIN_CD = "";
            this.SITE_CD = "";
            this.CLIENT_SITE_CD = "";
            this.IsMySite = false;
        }
    }
}
