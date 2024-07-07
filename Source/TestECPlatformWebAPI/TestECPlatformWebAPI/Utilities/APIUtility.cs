using WebAPIBasis.Utilities;
using TestECPlatformWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPIBasis.Literals;
using WebAPIBasis.Accesors;
using WebAPIBasis.ApiBuilder;

namespace TestECPlatformWebAPI.Utilities
{
    /// <summary>
    /// WebAPI用ユーティリティクラス
    /// </summary>
    public class APIUtility
    {
        private IAccesors _Accesors { get; set; } = null!;

        /// <summary>
        /// APIにおいて、共通の開始処理、DBにログを残す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iLogger"></param>
        /// <param name="iBody"></param>
        /// <param name="userID"></param>
        /// <param name="prgID"></param>
        /// <param name="iCode">クライアントのコードを入力するとクライアントのアクセサを生成できます</param>
        public IAccesors StartUpAPI<T>(ILogger<T> iLogger, object iBody, string userID, string prgID, string iCode = "")
        {
            iLogger.LogInformation(iBody.ToString());
            try
            {
                // コンフィグ、DB、ロガーアクセサー群を取得
                _Accesors = ApiBuilder.Create().Code(iCode).Build();
                _Accesors.getLoggerAPI().WriteLogAPI(userID, prgID, "", iBody.ToString()!);
            } catch
            {
                iLogger.LogInformation("ApiBuilder内でコンフィグ情報の生成に失敗しました");
            }
            return _Accesors;
        }

        /// <summary>
        /// 共通の関数終了前処理（ログ出力）
        /// </summary>
        /// <param name="logger">ログクラスのインスタンス</param>
        /// <param name="responseBody">レスポンスボディ</param>
        /// <remarks>
        /// <para>更新履歴</para>
        /// </remarks>
        public static void EndAPI<T>(ILogger<T> logger, object responseBody)
        {
            logger.LogInformation(responseBody.ToString());
        }

        /// <summary>
        /// APIにおいて、共通の異常終了レスポンスの作成処理,DBにログを残す
        /// ※デフォルト値 550、 他HttpStatus
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller">呼出元のコントローラインスタンス</param>
        /// <param name="logger">呼出元で生成されたロガーインスタンス</param>
        /// <param name="httpStatus">Httpレスポンス用ステータスコード</param>
        /// <param name="errorCode">Httpレスポンス用ステータスコード</param>
        /// <param name="errorMessage">Httpレスポンス用ステータスメッセージ</param>
        /// <param name="userID">T_LOG_API用</param>
        /// <param name="prgID">T_LOG_API用</param>
        /// <param name="logMessage">T_LOG_API用 Jsonは空なので任意のメッセージを入れる事ができます</param>
        /// <param name="ex">例外オブジェクト</param>
        /// <returns></returns>
        public IActionResult CreateBadResult<T>(ControllerBase controller, ILogger<T> logger, int httpStatus, int errorCode, string errorMessage, string userID, string prgID, string logMessage = "", Exception? ex = null)
        {
            // レスポンスBody（Jsonは空を想定）
            var result = new ErrorResult();
            result.errorCode = (errorCode == 0) ? Convert.ToInt32(APIRes.ExceptionError) : errorCode;
            result.errorMessage = (string.IsNullOrEmpty(errorMessage) == true) ? CommonUtils.DisplayEnum(APIRes.ExceptionError) : errorMessage;

            // LogAPI用メッセージ(Jsonの代わり)
            var wLogMessage = (string.IsNullOrEmpty(logMessage) == true) ? result.ToString() : logMessage;

            return this.CreateBadResponse(controller, logger, result, userID, prgID, httpStatus, wLogMessage, ex);
        }

        /// <summary>
        /// APIにおいて、共通の終了レスポンスの作成処理
        /// </summary>
        /// <param name="controller">本関数をコールしたインスタンス</param>
        /// <param name="logger">ログクラスのインスタンス</param>
        /// <param name="httpStatus">Httpレスポンス用ステータスコード</param>
        /// <param name="userID">T_LOG_API用</param>
        /// <param name="prgID">T_LOG_API用</param>
        /// <param name="responseBody">レスポンスボディ</param>
        /// <returns>レスポンスオブジェクト</returns>
        /// <remarks>
        /// <para>更新履歴</para>
        /// </remarks>
        public IActionResult CreateResponse<T>(ControllerBase controller, ILogger<T> logger, int httpStatus, string userID, string prgID, object responseBody)
        {

            // テキストログ
            logger.LogError("★" + HttpRes.Success.ToString() + ":" + CommonUtils.DisplayEnum(HttpRes.Success));
            APIUtility.EndAPI(logger, responseBody);
            // DBログ
            _Accesors.getLoggerAPI().WriteLogAPI(userID, prgID, httpStatus.ToString(), responseBody.ToString()!);

            return controller.StatusCode(httpStatus, responseBody);
        }

        /// <summary>
        /// APIにおいて、共通の異常時終了レスポンスの作成処理、任意メッセージをログを残す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="logger"></param>
        /// <param name="responseBody"></param>
        /// <param name="userID">T_LOG_API用</param>
        /// <param name="prgID">T_LOG_API用</param>
        /// <param name="httpStatus">T_LOG_API用</param>
        /// <param name="logMessage">T_LOG_API用</param>
        /// <param name="ex">例外インスタンス</param>
        /// <para>更新履歴</para>
        /// <returns></returns>
        public IActionResult CreateBadResponse<T>(ControllerBase controller, ILogger<T> logger, object responseBody, string userID, string prgID, int httpStatus, string logMessage, Exception? ex)
        {
            // 例外があればログをテキストとDBに残す
            if (ex != null)
            {
                logger.LogError("★" + httpStatus.ToString());
                logger.LogError(ex, ex.ToString());
                _Accesors.getLoggerAPI().WriteLogAPI(userID, prgID, httpStatus.ToString(), ex.ToString());
            }

            // ユーザー定義のエラーメッセージはT_LOG_APIのみ残す
            _Accesors.getLoggerAPI().WriteLogAPI(userID, prgID, httpStatus.ToString(), logMessage);

            return controller.StatusCode(httpStatus, responseBody);
        }
    }
}

