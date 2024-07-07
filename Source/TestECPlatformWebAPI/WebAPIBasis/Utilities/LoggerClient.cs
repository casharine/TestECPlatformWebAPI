using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics;
using System.Text;
using WebAPIBasis.Accesors;

namespace WebAPIBasis.Utilities
{
    /// <summary>
    /// クライアント用ロガー
    /// </summary>
    public class LoggerClient
    {
        private string _SITE_CD { get; } = null!;
        private IAccesors _Accesors { get; set; }


        public LoggerClient(string iSITE_CDSite)
        {
            _SITE_CD = iSITE_CDSite;
        }

        /// <summary>
        /// クライアント用ロガー
        /// コネクション生成制御は呼出元に依存
        /// </summary>
        /// <param name="cn"></param>
        /// <param name="programId"></param>
        /// <param name="message"></param>
        public void WriteLog(OracleConnection cn, string programId, string message)
        {
            var enc = Encoding.GetEncoding("Shift_JIS");

            if (enc.GetByteCount(message) > 3900)
            {
                var b = enc.GetBytes(message);
                var mess = enc.GetString(b, 0, 3900);
                var mess2 = enc.GetString(b, 0, 3901);
                if (mess == mess2)
                    message = mess.Remove(mess.Length - 1);
                else
                    message = mess;
            }

            var proc = Process.GetCurrentProcess();
            var thrd = Thread.CurrentThread;

            var cmd = new OracleCommand("LOG", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Clear();
            cmd.Parameters.Add("pUSERID", OracleDbType.Varchar2, ParameterDirection.Input).Value = "TestECPlatformWebAPI";
            cmd.Parameters.Add("pPROGRAMID", OracleDbType.Varchar2, ParameterDirection.Input).Value = programId;
            cmd.Parameters.Add("PMSGTEXT", OracleDbType.Varchar2, ParameterDirection.Input).Value = $"proc={proc.Id}, thread={thrd.ManagedThreadId}, {message}";
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// クライアント用ロガー
        /// コネクション生成と制御はLoggerClientクラス依存
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="message"></param>
        public void WriteLog(string programId, string message)
        {
            var strCn = new ConfigUtils().GetConnectionString(_SITE_CD);

            using (var cn = new DBUtils().CreateOracleConnection(strCn))
            {
                cn.Open();
                var tran = cn.BeginTransaction();

                WriteLog(cn, programId, message);

                tran.Commit();
                tran.Dispose();
                cn.Close();
            }
        }

        /// <summary>
        /// Staticから呼出用のクライアント用ロガー
        /// 空の場合は現在のサイトのコードのDBにログを残す
        /// コネクション生成と制御はLoggerClientクラス依存
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="message"></param>
        public static void WriteLogStatic(string programId, string message, string iSITE_CD)
        {
            var logger = new LoggerClient(iSITE_CD);
            var strCn = new ConfigUtils().GetConnectionString(iSITE_CD);

            using (var cn = new DBUtils().CreateOracleConnection(strCn))
            {
                cn.Open();
                var tran = cn.BeginTransaction();

                logger.WriteLog(cn, programId, message);

                tran.Commit();
                tran.Dispose();
                cn.Close();
            }
        }
    }
}
