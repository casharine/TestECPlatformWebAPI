using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using TestECPlatformWebAPI.Utilities;
using WebAPIBasis.Utilities;
using LineWebAPI.Models;
using WebAPIBasis.Literals;

namespace TestECPlatformWebAPI.Controllers
{
    /// <summary>
    /// 疎通確認用
    /// </summary>
    [ApiController]
    [Route("test_connection")]
    public class TestConnectionController : ControllerBase
    {
        internal readonly ILogger<TestConnectionController> mLogger;
        private IConfiguration cnf { get; set; }
        private const string _USER_ID = "999";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ログオブジェクト</param>
        public TestConnectionController(ILogger<TestConnectionController> logger)
        {
            this.mLogger = logger;
            cnf = new ConfigUtils().BuildConfigurationRoot();
        }

        /// <summary>
        /// 疎通確認用Get（引数不要）
        /// </summary>
        /// <response code="200">処理成功、業務エラーもここに含まれる</response>
        /// <response code="400">必須パラメーターが足りないなどのリクエストに誤りがある。</response>
        /// <remarks>
        /// <para>更新履歴</para>
        /// <para>Y.Ito 新規作成</para>
        /// </remarks>
        [HttpPost("get")]
        [ProducesResponseType(typeof(IEnumerable<GetTestResponseBase>), 200)]
        public IActionResult GetTestConnectionController()
        {
            var hoge1 = (APIRes)Enum.ToObject(typeof(APIRes), -200);
            var hoge2 = CommonUtils.DisplayEnum(hoge1);
            var hoge3 = CommonUtils.DisplayEnum((APIRes)Enum.ToObject(typeof(APIRes), -200));
            var hoge4 = CommonUtils.DisplayEnumByInt(new APIRes(), -200);
            var fuga = CommonUtils.DisplayEnum(APIRes.AuthErr);

            var apiUtility = new APIUtility();
            var dbLog = new LoggerAPI();
            var wBody = new object();
            wBody = "疎通を確認しました";

            try
            {
                    dbLog.WriteLogAPI(_USER_ID, "req.GetTestConnectionController", "200", "疎通確認中。。。 ※コントローラ呼出完了");

                    // レスポンスに設定
                    var result = new GetTestResponseBase()
                    {
                        message = wBody.ToString()!
                    };

                    return apiUtility.CreateResponse(this, this.mLogger, (int)HttpRes.Success, _USER_ID, this.ToString()!, wBody);
            }
            catch (OracleException exOra)
            {
                this.mLogger.LogError(exOra, exOra.ToString());
                var resMessage = CommonUtils.DisplayEnum(APIRes.ExceptionError) + "(" + exOra.Message + ")";
                return apiUtility.CreateBadResult(this, this.mLogger, (int)HttpRes.Success, (int)APIRes.DataException, CommonUtils.DisplayEnum(APIRes.ExceptionError), _USER_ID, this.ToString()!, "", exOra);
            }
            catch (Exception ex)
            {
                this.mLogger.LogError(ex, ex.ToString());
                var resMessage = CommonUtils.DisplayEnum(APIRes.ExceptionError) + "(" + ex.Message + ")";
                return apiUtility.CreateBadResult(this, this.mLogger, (int)HttpRes.Success, (int)APIRes.ExceptionError, CommonUtils.DisplayEnum(APIRes.ExceptionError), _USER_ID, this.ToString()!, "", ex);
            }
        }
    }
}