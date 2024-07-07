
using System.Data;
using WebAPIBasis.Accesors;
using WebAPIBasis.Literals;
using WebAPIBasis.Utilities;
using TestECPlatform.Purchase.Models;
using TestECPlatform.Purchase.Utilities;
using TestECPlatform.Purchase.Main;
using TestECPlatform.Purchase.Main.Factory;
using Oracle.ManagedDataAccess.Client;
using System.Transactions;
using System.Data.Common;

/// <summary>
/// 購入処理用クラス
/// </summary
/// <remarks>ビジネスロジックは割愛</remarks>
public class PurchaseFUGA : IPurchase
{
    #region Init(Field, Property, Constructor)
    /// <summary>各種アクセサー</summary>
    private IAccesors _Accesors { get; set; }
    /// <summary>購入クラス</summary>
    private PurchaseParameter _KonyuParameter { get; set; }
    /// <summary>ロックオブジェクト取得真偽値</summary>
    private bool lockTaken = false;
    /// <summary>購入結果</summary>
    private PurchaseResult _res = new PurchaseResult() { ErrorCode = 0, ErrorMessage = "" };

    /// <summary>
    /// Controllerから呼出用コンストラクタ 
    /// </summary>
    public PurchaseFUGA(PurchaseParameter iPurchaseParameter, IAccesors iAccesors)
    {
        _Accesors = iAccesors;
        _KonyuParameter = iPurchaseParameter;
    }
    #endregion 初期処理(フィールド,プロパティ,コンストラクタ)

    #  region 購入処理
    /// <summary>
    /// 購入処理
    /// </summary>
    /// <returns>
    /// </returns>
    /// <remarks></remarks>
    public PurchaseResult purchase()
    {
        try
        {
            // ロック制御
            Monitor.TryEnter(_Accesors.getLockObject(), 1000, ref lockTaken);
            if (lockTaken)
            {
                using (OracleConnection connection = _Accesors.getConnection())
                {
                    connection.Open();
                    using (OracleTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            //自社購入処理 ※ビジネスロジック割愛
                            _res = new PurchaseResult();
                            _Accesors.getLoggerClient().WriteLog("Purchase.purchase", $"エラー内容:{_res.ErrorCode} : {_res.ErrorMessage}");

                            // 他ECサイトへ連携
                            if (_KonyuParameter.SHOHIN_CD != _KonyuParameter.SITE_CD)
                            {
                                new PurchaseConnector().purchaseLink(_KonyuParameter, ref _res);

                                if (_res.ErrorCode != 0)
                                {
                                    _Accesors.getLoggerClient().WriteLog($"Purchase.purchase", $"エラー内容：サイトコード={_Accesors.getCode()}, エラーコード：{_res.ErrorCode}, メッセージ：{_res.ErrorMessage}");
                                    return _res;
                                }
                            }

                            // デモとして商品コードが999の場合は残数不足による失敗とする
                            if (_KonyuParameter.SHOHIN_CD.ToString() == "999")
                            {
                                _res.ErrorCode = Convert.ToInt32(PurchaseRes.LackOfItemError);
                                _res.ErrorMessage = CommonUtils.DisplayEnum(PurchaseRes.LackOfItemError);
                            }
                            else
                            {
                                _res.ErrorCode = Convert.ToInt32(PurchaseRes.Success);
                                _res.ErrorMessage = CommonUtils.DisplayEnum(PurchaseRes.Success);
                            }

                            transaction.Commit();
                        }
                        catch (APIException ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                        catch (Exception ex)
                        {
                            _Accesors.getLoggerClient().WriteLog(this.GetType().Name, ex.Message);

                            _res.CountLockFailed = 0;
                            _res.ErrorCode = (int)PurchaseRes.UnexpectedError;
                            _res.ErrorMessage = CommonUtils.DisplayEnumByInt(new PurchaseRes(), _res.ErrorCode);

                            return _res;
                        }
                    }
                }
            }
        }
        finally
        {
            // LockObjの開放
            if (!lockTaken)
            {
                // 本クラスのロックオブジェクト取得失敗
                _res.ErrorCode = (int)PurchaseRes.TimeoutForGetLockObj;
                _res.ErrorMessage = CommonUtils.DisplayEnumByInt(new PurchaseRes(), _res.ErrorCode);
                _res.CountLockFailed++;
            }
            else if (lockTaken && _res.ErrorMessage.Contains(CommonUtils.DisplayEnum(PurchaseRes.TimeoutForGetLockObj)))
            {
                // 連携先のロックオブジェクト取得に失敗
                Monitor.Exit(_Accesors.getLockObject());
            }
            else
            {
                // 正常終了
                Monitor.Exit(_Accesors.getLockObject());
                _res.CountLockFailed = 0;
            }
        }
        return _res;
    }
    #endregion 購入処理
}
