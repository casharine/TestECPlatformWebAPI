using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WebAPIBasis.Literals
{
# region Httpレスポンス
    /// <summary>
    /// HttpStatusCodeに対するメッセージ定義
    /// </summary>
    public enum HttpRes
    {
        [Display(Name = "正常")]
        Success = 200,

        [Display(Name = "システムエラー")]
        ExceptionError = 550,

        [Display(Name = "認証エラー(トークンエラー)")]
        AuthError = 401,
    }
    #endregion

    #region Purchaseクラスレスポンス
    /// <summary>
    /// Purchaseクラスのエラーコードに対するメッセージ定義
    /// </summary>
    public enum PurchaseRes
    {
        [Display(Name = "正常に処理を終了しました")]
        Success = 0,

        [Display(Name = "この商品はWEB販売が終了した為、引当できませんでした。")]
        NoPublicItemError = -1000,

        [Display(Name = "売止されている為、引当できません。")]
        StopPurchasingError = -2000,

        [Display(Name = "この商品は残数が少ない為、引当する事ができませんでした。")]
        LackOfItemError = -3000,

        [Display(Name = "販売先コードが見つからない為、引当できませんでした。")]
        UnregisterdCustomer = -4000,

        [Display(Name = "販売先コードが見つからない為、引当できませんでした。")]
        UnregisterdSubCustomer = -4100,

        // 単価未設定
        [Display(Name = "APIエラーです。")]
        UnitPriceNotSetError = -5000,

        // ロックオブジェクト取得タイムアウト
        [Display(Name = "注文が集中したためタイムアウトしました。再度ご注文ください。")]
        TimeoutForGetLockObj = -6000,

        [Display(Name = "申し訳ありません、引当できませんでした。")]
        APIfailError = -8000,

        // 外部連携エラー 
        [Display(Name = "外部連携エラーです。")]
        LINKfailedError = -9000,

        // 外部連携（単価、入数）チェックエラー
        [Display(Name = "単価、入数が変更されたため、引当できませんでした。")]
        ChangedTankaSuryo = -9010,

        [Display(Name = "予期せぬエラーが発生しました")]
        UnexpectedError = -9999
    }
    #endregion 購入クラス

    #region APIレスポンス
    /// <summary>
    /// 処理ステータスに対するメッセージ定義
    /// </summary>
    public enum APIRes
    {
        [Display(Name = "正常に処理を終了しました")]
        Success = 0,

        [Display(Name = "ユーザー認証に失敗しました")]
        AuthErr = -100,

        [Display(Name = "ただいま販売時間外です")]
        OutOfTime = -200,

        [Display(Name = "連携停止中です")]
        LinkStop = -300,

        [Display(Name = "該当の商品は公開されておりません")]
        KoukaiOrosiErr = -400,

        [Display(Name = "指定した業者はみつかりませんでした")]
        NotFoundOrosi = -500,

        [Display(Name = "トークンの生成に失敗しました")]
        TokenError = -600,

        [Display(Name = "データ例外")]
        DataException = -700,

        [Display(Name = "こちらのユーザーIDは現在使う事はできません")]
        UserException = -900,

        [Display(Name = "予期せぬエラーが発生いたしました")]
        ExceptionError = -9999,
    }
    #endregion APIレスポンス

    #region Enum取得関数


    ///// <summary>
    ///// 詳細レスポンスAttribute取得 ※ジェネリクスを使用しない場合
    ///// </summary>
    //public static class APIReseExtensions
    //{
    //    public static string ToDisplayName(this APIRes source)
    //    {
    //        string internalFormat = source.ToString();
    //        var displayAttribute = source.GetType().GetField(internalFormat)?.GetCustomAttribute<DisplayAttribute>();
    //        return displayAttribute?.Name ?? internalFormat;
    //    }
    //}
    #endregion
}
