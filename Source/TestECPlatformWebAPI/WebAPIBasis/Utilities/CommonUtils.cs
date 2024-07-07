using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Security.Cryptography;
using WebAPIBasis.Literals;

namespace WebAPIBasis.Utilities
{
    /// <summary>
    /// 汎用的な関数群の定義クラス
    /// </summary>
    /// <author>Y.Ito</author>
    public static class CommonUtils
    {
        #region ENUM処理
        /// <summary>
        /// EnumのDisplayAttriuteをEnumのキーから取得します
        /// ※ジェネリクス対応 以下記述例
        /// CommonUtils.DisplayEnum(APIRes.AuthErr))
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumKey"></param>
        /// <returns></returns>
        public static string DisplayEnum<T>(T enumKey) where T : IComparable
        {
            string internalFormat = enumKey.ToString()!;
            var dispAttribute = enumKey.GetType().GetField(internalFormat!)?.GetCustomAttribute<DisplayAttribute>()!;

            return dispAttribute?.Name ?? internalFormat!;
        }

        /// <summary>
        /// EnumのDisplayAttriuteをEnumの値から取得します
        /// outsideMsgに文字が入力されている場合コードコードとメッセージをネストします
        /// ※ジェネリクス対応 以下記述例
        /// CommonUtils.DisplayEnumByInt(new APIRes(), -200)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string DisplayEnumByInt<T>(T enumType, int val, string outsideMsg = "") where T : IComparable
        {
            var key = (T)Enum.ToObject(typeof(T), val);
            var msg = DisplayEnum(key);

            msg = outsideMsg != "" ? $"{outsideMsg} [コード：{val}] [メッセージ：{msg}]" : msg;

            return msg;
        }
        #endregion ENUM

        # region 処理プログラム情報取得処理
        /// <summary>
        /// このメソッドを呼び出したクラス、メソッド名をドット区切りで取得します(50字以上切捨）
        /// </summary>
        /// <returns>メソッド名</returns>
        public static string GetMyName(int i = 0)
        {
            var stackFrame = new StackFrame(1 + i, true);
            var className = "";
            var methodName = "";
            var resName = $"{className}.{methodName}";

            try
            {
                className = stackFrame.GetFileName();
                className = className!.Substring(className.LastIndexOf(@"\") + 1, className.LastIndexOf(".cs") - className.LastIndexOf(@"\") - 1);
            } catch { } 

            try
            {
                methodName = stackFrame.GetMethod()!.Name;
                resName = $"{className}.{methodName}";
                resName = resName.Length > 50 ? resName.Substring(0, 50) : resName;
            }
            catch { }

            // 空文字の場合
            resName = resName.Length <= 0 ? "ECPlatform.GetMyName" : resName;

            return resName;
        }
# endregion

        #region DBNull処理
        /// <summary>
        /// 値がDBNull.Value, 空文字、 NULLの場合Trueを返します
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isNullOrEmptyOrDBNull(string str)
        {
            return DBNull.Value.Equals(str) ? true : string.IsNullOrEmpty(str) ? true : false;
        }

        /// <summary>
        /// DBNull.Valueを空文字に変換して返却する関数
        /// </summary>
        /// <param name="value">対象の値</param>
        /// <remarks>
        /// <para>更新履歴</para>
        /// </remarks>
        public static string GetDBValueAsString(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;

            return value.ToString()!;
        }

        /// <summary>
        /// DBNull.Valueを0に変換して返却する関数
        /// </summary>
        /// <param name="value">対象の値</param>
        /// <remarks>
        /// <para>更新履歴</para>
        /// </remarks>
        public static long GetDBValueAsLong(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0;

            if (long.TryParse(value.ToString(), out long result) == false)
                return 0;

            return result;
        }

        /// <summary>
        /// objectがnull or DBNull型 または 値が空文字、 NULLの場合は空文字を返し、値がある場合文字型に変換して返す関数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <author></author>
        public static string objectToStringOrEmpty(object value)
        {
            return value == null ? string.Empty : value is DBNull ? string.Empty : String.IsNullOrEmpty(value.ToString()!) ? string.Empty : value.ToString()!;
        }
        #endregion

        #region プロパティ制御
        /// <summary>
        /// 対象オブジェクトのプロパティにプロパティの型通りに変換した値にセットする関数
        /// </summary>
        /// <param name="pi">プロパティ情報</param>
        /// <param name="target">値をセットするオブジェクト</param>
        /// <param name="value">セットされる値</param>
        /// <param name="index">インデックスプロパティのインデックス値</param>
        /// <remarks></remarks>
        public static void SetProperty(PropertyInfo pi, object target, object value, object[]? index = null)
        {
            try
            {
                if (value == null && !pi.PropertyType.IsValueType)
                {
                    pi.SetValue(target, value, index);
                }
                else
                {
                    // 型ごとに条件をまとめる
                    switch (Type.GetTypeCode(pi.PropertyType))
                    {
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                            pi.SetValue(target, Convert.ChangeType(value, pi.PropertyType), index);
                            break;
                        case TypeCode.Decimal:
                            pi.SetValue(target, Convert.ToDecimal(value), index);
                            break;
                        case TypeCode.DateTime:
                            pi.SetValue(target, Convert.ToDateTime(value), index);
                            break;
                        case TypeCode.Boolean:
                            pi.SetValue(target, Convert.ToBoolean(value), index);
                            break;
                        case TypeCode.String:
                            pi.SetValue(target, Convert.ToString(value), index);
                            break;
                        default:
                            if (pi.PropertyType.IsEnum)
                            {
                                pi.SetValue(target, Enum.Parse(pi.PropertyType, (string)value), index);
                            }
                            else if (pi.PropertyType == typeof(Color) && value is string colorName)
                            {
                                pi.SetValue(target, Color.FromName(colorName), index);
                            }
                            else if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>) && value is Dictionary<string, object> dictionary)
                            {
                                var convertedDictionary = dictionary.ToDictionary(kv => kv.Key, kv => Convert.ChangeType(kv.Value, typeof(int)));
                                pi.SetValue(target, convertedDictionary, index);
                            }
                            else
                            {
                                pi.SetValue(target, value, index);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to set property {pi.Name} with type {pi.PropertyType} and value {value}", ex);
            }
        }
        #endregion

        #region Entity生成
        /// <summary>
        /// 指定した型オブジェクトを生成して、DataRowの値をオブジェクトに設定する
        /// </summary>
        /// <typeparam name="T">対象オブジェクトの型</typeparam>
        /// <param name="iRow">データ取得元オブジェクト</param>
        /// <param name="iNotTargetColumNames">設定対象外の列名・プロパティ名</param>
        /// <para>Y.ITO</para>
        /// <returns></returns>
        public static T CreateObject<T>(DataRow iRow, params string[] iNotTargetColumNames) where T : new()
        {
            T rslt = new T();
            SetObject(iRow, rslt, iNotTargetColumNames);
            return rslt;
        }

        /// <summary>
        /// Entityの値を設定したEntityを返却する
        /// </summary>
        /// <typeparam name="T">対象オブジェクトの型</typeparam>
        /// <param name="iEntity">データ取得元オブジェクト</param>
        /// <param name="iNotTargetColumNames">設定対象外の列名・プロパティ名</param>
        /// <para>Y.ITO</para>
        /// <returns></returns>
        public static T CreateObject<T>(object iEntity, params string[] iNotTargetColumNames) where T : new()
        {
            T rslt = new T();
            SetObject(iEntity, rslt, iNotTargetColumNames);
            return rslt;
        }

        /// <summary>
        /// <para>渡されたコレクションやリストの値をEntityに設定する</para>
        /// <para>対応しているコレクションやリスト</para>
        /// <para>・DataRow</para>
        /// <para>・Entyties.EntityBase</para>
        /// </summary>
        /// <typeparam name="T">対象オブジェクトの型</typeparam>
        /// <param name="iData">データ取得元オブジェクト</param>
        /// <param name="iTarget">データ取得先オブジェクト</param>
        /// <param name="iNotTargetColumNames">設定対象外の列名・プロパティ名</param>
        /// <para>Y.ITO</para>
        /// <remarks></remarks>
        public static void SetObject<T>(object iData, T iTarget, params string[] iNotTargetColumNames) where T : new()
        {
            SetObject(iData, iTarget, GetValueForEntity, iNotTargetColumNames);
        }

        /// <summary>
        /// <para>渡されたコレクションやリストの値をEntityに設定する</para>
        /// <para>対応しているコレクションやリスト</para>
        /// <para>・DataRow</para>
        /// </summary>
        /// <typeparam name="T">対象オブジェクトの型</typeparam>
        /// <param name="iData">データ取得元オブジェクト</param>
        /// <param name="iTarget">データ取得先オブジェクト</param>
        /// <param name="GetValueMethod">値取得関数</param>
        /// <param name="iNotTargetColumNames">設定対象外の列名・プロパティ名</param>
        /// <para>Y.ITO</para>
        /// <remarks></remarks>
        public static void SetObject<T>(object iData, T iTarget, Func<object, string, object> GetValueMethod, params string[] iNotTargetColumNames) where T : new()
        {
            var piList = typeof(T).GetProperties();

            foreach (var pi in piList)
            {
                // 対象外のリストを取得
                var piName = pi.Name;
                var wNotTargetColumName = iNotTargetColumNames.Where(n => n.ToString() == piName);
                if (wNotTargetColumName.Count() > 0)
                    continue;

                object value = GetValueMethod.Invoke(iData, pi.Name);
                // DB Nullは値を設定しない。
                if (Convert.IsDBNull(value))
                    continue;
                try
                {
                    SetProperty(pi, iTarget!, value);
                }
                catch (Exception)
                {
                    throw new ApplicationException(string.Format("SharedFunctions.SetObject - プロパティ{0}の設定に失敗しました：Type{1}, Value{2}", pi.Name, pi.PropertyType, value));
                }
            }

            return;
        }

        /// <summary>
        /// SetObject関数で使用する、値設定関数（標準）
        /// </summary>
        /// <param name="dataSource">データの取得元</param>
        /// <param name="targetName">対象の値の名称</param>
        /// <returns>取得した値</returns>
        /// <para>Y.ITO</para>
        /// <remarks></remarks>
        private static object GetValueForEntity(object dataSource, string targetName)
        {
            if (dataSource is DataRow)
            {
                // DataRowからの取得
                var row = (DataRow)dataSource;
                if (!row.Table.Columns.Contains(targetName))
                    return DBNull.Value;

                return row[targetName];
            }
            else
            {
                // Entityからの取得
                var pi = dataSource.GetType().GetProperty(targetName);
                if (pi == null)
                    return DBNull.Value;

                return pi.GetValue(dataSource, null)!;
            }
        }
        #endregion

        #region 暗号化処理
        //  ハッシュ化・トークン生成
        /// <summary>
        ///  apiアクセストークン：アクセストークンと”：”と日付YYY/MM/DDを連結し、sha-256のhash値を返す
        /// </summary>
        /// <param name="str">トークン生成指定文字列</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetAccessToken(string str)
        {
            // byte型配列に変換する
            byte[] data = System.Text.Encoding.UTF8.GetBytes(str);

            // SHA256オブジェクトを作成
            HashAlgorithm SHA256 = System.Security.Cryptography.SHA256.Create();

            // ハッシュ値に変換する
            byte[] bs = SHA256.ComputeHash(data);

            // リソースを解放する
            SHA256.Clear();

            // byte型配列を16進数の文字列に変換
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            foreach (var b in bs)
                result.Append(b.ToString("x2"));

            // アクセス用トークン
            string AccessToken = result.ToString();
            return AccessToken;
        }
        #endregion
    }
}
