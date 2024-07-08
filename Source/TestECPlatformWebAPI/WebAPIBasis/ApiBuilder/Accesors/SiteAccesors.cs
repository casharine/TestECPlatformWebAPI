using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using WebAPIBasis.Utilities;
using WebAPIBasis.Literals;
using System.Runtime.CompilerServices;

namespace WebAPIBasis.Accesors
{
    /// <summary>
    /// 【 概要 】APIクライアント用のアクセサオブジェクト群を一括生成する、IAccesorsの具象クラス
    /// 【 対象 】DB接続情報、AppX.jsonアクセサ、DB接続プール、ロガーオブジェクト 【！！要クラス内readme参照！！】
    /// </summary>
    /// 
    /// <readme>
    /// 1.基本的にApiBuilderから生成してください ※例： ApiBuilder.CreateAccessors().Code(string iCode).Build();
    /// </readme>
    /// <author> Y.Ito  </author>
    public class ClientAccesors : IAccesors
    {
        #region Init(Field, Property, Constructor)
        private string _SITE { get; } = "";
        private string _connectionStrings { get; } = "";
        private IConfigurationRoot _configurationRoot { get; } = null!;
        private OracleConnection _oracleConnection { get; } = null!;
        private LoggerAPI _loggerAPI { get; } = null!;
        private LoggerClient _loggerClient { get; } = null!;
        private static object _myLockObject { get; set; } = new object();
        private static object _LockObjectFUGA { get; } = new object();
        private static object _LockObjectPIYO { get; } = new object();
        private static object _LockObjectHOGE { get; } = new object();

        public ClientAccesors(string iSITE)
        {
            var fnc = new ConfigUtils();
            _SITE = iSITE;
            _configurationRoot = fnc.BuildConfigurationRoot();
            _connectionStrings = fnc.GetConnectionString(iSITE)!;
            _oracleConnection = new DBUtils().CreateOracleConnection(_connectionStrings);
            _loggerAPI = new LoggerAPI("");
            _loggerClient = new LoggerClient(iSITE);
        }
        #endregion Init(field, property, constructor)

        #region Getterメソッド群
        public string getCode() { return _SITE; }
        public string getConnectionString() { return _connectionStrings; }
        public IConfigurationRoot getAppsettings() { return _configurationRoot; }
        public OracleConnection getConnection() { return _oracleConnection; }
        public LoggerAPI getLoggerAPI() { return _loggerAPI; }
        public LoggerClient getLoggerClient() { return _loggerClient; }
        #endregion Getterメソッド群

        #region その他のメソッド群
        /// <summary>
        /// 購入処理のサイトのロックオブジェクトを設定します
        /// ※ 連携先が増えた場合コードを追加してください
        /// </summary>
        public object getLockObject()
        {
            object wLockObject;

            try
            {
                var target = this.getCode() switch
                {
                    "1001" => wLockObject = LockObjectsSingleton.GetInstance().GetLockObject("1001"),
                    "1002" => wLockObject = LockObjectsSingleton.GetInstance().GetLockObject("1002"),
                    "1003" => wLockObject = LockObjectsSingleton.GetInstance().GetLockObject("1003"),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            catch (Exception ex)
            {
                this.getLoggerClient().WriteLog("createSelectedPurchase", $"コード{this.getCode()}の購入クラス生成に失敗しました");
                throw;
            }
            return wLockObject;
        }
        #endregion その他のメソッド群
    }
}
