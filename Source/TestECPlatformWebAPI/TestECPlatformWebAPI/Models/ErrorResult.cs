using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using WebAPIBasis.Literals;

namespace TestECPlatformWebAPI.Models
{
    /// <summary>
    /// 汎用リザルトクラス。各レスポンスクラスのメンバで用いる。
    /// </summary>
    [DataContract]
    public class ErrorResult : IEquatable<ErrorResult>
    {
        /// <summary>
        /// エラーコード
        /// </summary>
        /// <value>OKの場合:0、エラー有り:0以外のコード</value>
        [Required]
        [DataMember(Name = "errorCode")]
        public int? errorCode { get; set; }

        /// <summary>
        /// エラーコードメッセージ
        /// </summary>
        /// <value>正常終了:"なし"、エラー有り:エラー内容</value>
        [Required]
        [DataMember(Name = "errorMessage")]
        public string errorMessage { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ErrorResult()
        {
            //メンバの初期値を設定する
            this.errorCode = Convert.ToInt32(HttpRes.Success);
            this.errorMessage = string.Empty;
        }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ErrrorResult {\n");
            sb.Append("  errorCode: ").Append(errorCode).Append("\n");
            sb.Append("  errorMessage: ").Append(errorMessage).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ErrorResult)obj);
        }

        /// <summary>
        /// Returns true if ErrrorResult instances are equal
        /// </summary>
        /// <param name="other">Instance of ErrrorResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ErrorResult? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    errorCode == other.errorCode ||
                    errorCode != null &&
                    errorCode.Equals(other.errorCode)
                ) &&
                (
                    errorMessage == other.errorMessage ||
                    errorMessage != null &&
                    errorMessage.Equals(other.errorMessage)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                if (errorCode != null)
                    hashCode = hashCode * 59 + errorCode.GetHashCode();
                if (errorMessage != null)
                    hashCode = hashCode * 59 + errorMessage.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
#pragma warning disable 1591

        public static bool operator ==(ErrorResult left, ErrorResult right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ErrorResult left, ErrorResult right)
        {
            return !Equals(left, right);
        }

#pragma warning restore 1591
        #endregion Operators
    }
}
