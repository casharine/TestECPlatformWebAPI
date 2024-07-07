using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace WebAPIBasis.Utilities
{
    /// <summary>
    /// コンストラクタの引数でログを残すDBを切り替えます
    /// デフォルト値：Jsonのコネクションストリングを取得します
    /// コード指定：DBからのコネクションストリングを取得します    
    /// </summary>
    public class LoggerAPI
    {
        private string _site_cd { get; set; }

        public LoggerAPI(string iSITE_CD = "")
        {
            _site_cd = iSITE_CD;
        }

        public void WriteLogAPI(OracleConnection cn, string userId, string programId, string httpStatus, string message)
        {
            var tran = cn.BeginTransaction();
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

            var cmd = new OracleCommand("LOG", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            var proc = Process.GetCurrentProcess();
            var thrd = Thread.CurrentThread;

            cmd.Parameters.Clear();
            cmd.Parameters.Add("pUSERID", OracleDbType.Varchar2, ParameterDirection.Input).Value = userId;
            cmd.Parameters.Add("pPROGRAMID", OracleDbType.Varchar2, ParameterDirection.Input).Value = programId;
            cmd.Parameters.Add("pHTTPSTATUS", OracleDbType.Varchar2, ParameterDirection.Input).Value = httpStatus;
            cmd.Parameters.Add("pMSGTEXT", OracleDbType.Varchar2, ParameterDirection.Input).Value = string.Format("[proc={0}, thread={1}]{2}", proc.Id, thrd.ManagedThreadId, message);
            cmd.ExecuteNonQuery();

            tran.Commit();
            tran.Dispose();
        }

        /// <summary>
        /// 基本的にLoggerAPIはこちらを呼び出してください
        /// ※コネクション制御はLoggerAPI側で行うため
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="programId"></param>
        /// <param name="httpStatus"></param>
        /// <param name="message"></param>
        public void WriteLogAPI(string userId, string programId, string httpStatus, string message)
        {
            var strCn = new ConfigUtils().GetConnectionString(_site_cd);

            using (var cn = new DBUtils().CreateOracleConnection(strCn))
            {
                cn.Open();
                WriteLogAPI(cn, userId, programId, httpStatus, message);
                cn.Dispose();
            }
        }
    }
}
