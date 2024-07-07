using WebAPIBasis.Utilities;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace WebAPIBasis.Accesors
{
    /// <summary>
    /// 【 概要 】SiteAccesorsGeneratorFactoryにより自動更新されるスキーマ別アクセサオブジェクト群
    /// 【 対象 】DB接続情報、AppXXX.jsonアクセサ、DB接続プール、ロガーオブジェクト 【！！要クラス内readme参照！！】
    /// </summary>
    /// 
    /// <readme>
    /// 0.前提としてStaticで使用できる
    /// 1.カプセル化しており、getterはグローバル、setterはAccesorクラス専用（正確にはintarnal,プロジェクト内）
    /// 2.CLS_KOUNYUインスタンス化時に必ずSiteAccesorsGeneratorFactoryで更新する事
    /// 3.再帰的にCLS_KOUNYU呼出(=接続先、OROSI_CD_SITEを変更)したの場合、retun前後に呼出元OROSI_CD_SITEで元に戻す事
    /// </readme>
    public static class SiteAccesorsGlobal
    {
        /// <summary> 現在処理中のWebサイトの卸コード </summary>
        public static string gOROSI_CD_SITE { get { return _gOROSI_CD_SITE; } internal set { _gOROSI_CD_SITE = value; } }
        private static string _gOROSI_CD_SITE = "";

        /// <summary> gOROSI_CD_SITEのWebサイトのコネクションストリングス </summary>
        public static string gConnectionStrings { get { return _gConnectionStrings; } internal set { _gConnectionStrings = value; } }
        private static string _gConnectionStrings = null!;

        /// <summary> 本WebAPIのAppsettings.Jsonのルートオブジェクト  </summary>
        public static IConfigurationRoot gConfigurationRoot { get { return _gConfigurationRoot; } internal set { _gConfigurationRoot = value; } }
        private static IConfigurationRoot _gConfigurationRoot = null!;

        /// <summary> gOROSI_CD_SITEのDB接続情報から生成された接続プール </summary>
        public static OracleConnection gOracleConnection { get { return _gOracleConnection; } internal set { _gOracleConnection = value; } }
        private static OracleConnection _gOracleConnection = null!;

        /// <summary> APIDB用ロガー </summary>
        public static LoggerAPI gLoggerAPI { get { return _gLoggerAPI; } internal set { _gLoggerAPI = value; } }
        private static LoggerAPI _gLoggerAPI = null!;

        /// <summary> APIクライアント側のDB用ロガー  </summary>
        public static LoggerClient gLoggerClient { get { return _gLoggerClient; } internal set { _gLoggerClient = value; } }
        private static LoggerClient _gLoggerClient = null!;
    }
}