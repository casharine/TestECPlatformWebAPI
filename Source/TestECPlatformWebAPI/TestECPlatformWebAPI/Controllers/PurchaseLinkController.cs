using TestECPlatform.Purchase.Models;
using TestECPlatform.Purchase.Main.Factory;
using TestECPlatformWebAPI.Models;
using TestECPlatformWebAPI.Models.TestECPlatformWebAPI.Models;
using TestECPlatformWebAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Oracle.ManagedDataAccess.Client;
using WebAPIBasis.Accesors;
using WebAPIBasis.Literals;
using WebAPIBasis.Utilities;

namespace TestECPlatformWebAPI.Controllers
{
    /// <summary>
    /// 連携API購入コントローラクラス
    /// </summary>
    [ApiController]
    [Route("")]
    public class PurchaseLinkController : ControllerBase
    {
        #region Init(Field, Constructor)
        internal readonly ILogger<PurchaseLinkController> mLogger;
        private PurchaseParameter param = new PurchaseParameter();
        private StringValues token = "";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ログオブジェクト</param>
        public PurchaseLinkController(ILogger<PurchaseLinkController> logger)
        {
            this.mLogger = logger;
        }
        #endregion Init(Field, Constructor)

        #region
        /// <summary>
        /// 連携購入用API
        /// </summary>
        /// <param name="body">連携購入用API</param>
        /// <response code="200">処理成功、業務エラーもここに含まれる</response>
        /// <response code="400">必須パラメーターが足りないなどuserのリクエストに誤りがある。</response>
        /// <remarks>
        /// <para>更新履歴</para>
        /// <para>Y.ITO</para>
        /// </remarks>
        [HttpPost("purchase_link")]
        [ProducesResponseType(typeof(IEnumerable<PostPurchaseLinkResponseBase>), 200)]
        public IActionResult PostPurchaseLinkData(PostPurchaseLinkRequestBase body)
        {
            // APIの初期設定
            var apiUtility = new APIUtility();
            var accesors = apiUtility.StartUpAPI(this.mLogger, body, body.USER_ID, this.ToString()!);
            var res = new PostPurchaseLinkResponseBase();
            long TotalCountLockFailed = 0;

            try
            {
                // JsonからPurchaseParameterへプロパティ変換
                param = CommonUtils.CreateObject<PurchaseParameter>(body);

                // 自社購入処理を生成し実行
                IPurchase Purchase = new SelfPurchaseFactory().createPurchase(param);
                PurchaseResult resPurchaseLink = Purchase.purchase();
                TotalCountLockFailed += resPurchaseLink.CountLockFailed;

                // 間デットロック対策：ロック取得失敗時は計三度まで購入処理をトライ
                while (resPurchaseLink.ErrorCode != 0 && 0 < TotalCountLockFailed && TotalCountLockFailed > 3)
                {
                    resPurchaseLink = Purchase.purchase();
                    TotalCountLockFailed += resPurchaseLink.CountLockFailed;
                }

                // 購入処理実行結果を結果Jsonプロパティに変換
                res = CommonUtils.CreateObject<PostPurchaseLinkResponseBase>(resPurchaseLink);

                // 結果をJsonで返しログを残す
                return apiUtility.CreateResponse(this, this.mLogger, (int)HttpRes.Success, body.USER_ID, this.ToString()!, res);
            }
            catch (APIException exAPI)
            {
                res.ErrorCode = Convert.ToInt32(exAPI.Message);
                res.ErrorMessage = CommonUtils.DisplayEnumByInt(new PurchaseRes(), res.ErrorCode);
                return apiUtility.CreateResponse(this, this.mLogger, (int)HttpRes.Success, body.USER_ID, this.ToString()!, res);
            }
            catch (OracleException exOra)
            {
                var logMessage = "USER_ID：" + body.USER_ID + ", token：" + token + res.ErrorMessage;
                return apiUtility.CreateBadResult(this, this.mLogger, (int)HttpRes.ExceptionError, -7000, $"データ例外({exOra.ToString()})", body.USER_ID, this.ToString()!, logMessage, exOra); ;
            }
            catch (Exception ex)
            {
                var logMessage = "USER_ID：" + body.USER_ID + ", token：" + token + res.ErrorMessage;
                return apiUtility.CreateBadResult(this, this.mLogger, (int)HttpRes.ExceptionError, -9999, $"予期せぬエラーが発生しました({ex.ToString()}", body.USER_ID, this.ToString()!, logMessage, ex); ;
            }
        }
        #endregion 購入処理
    }
}
