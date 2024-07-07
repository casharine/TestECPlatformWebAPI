using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace TestECPlatformWebAPI.Models
{
    /// <summary>
    /// 購入連携データ取得レスポンス
    /// </summary>
    [DataContract]
    public class PostPurchaseLinkResponseBase : IEquatable<PostPurchaseLinkResponseBase>
    {
        /// <summary>
        /// エラーコード
        /// </summary>
        [Required]
        [DataMember(Name = "errorCode")]
        [JsonPropertyName("errorCode")]
        public int ErrorCode { get; set; }

        /// <summary>
        /// メッセージ
        /// </summary>
        [Required]
        [DataMember(Name = "errorMessage")]
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks></remarks>
        public PostPurchaseLinkResponseBase()
        {
            this.ErrorCode = 0;
            this.ErrorMessage = string.Empty;
        }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PostPurchaseLinkResponse {\n");
            sb.Append("  \"ErrorCode\" : ").Append(ErrorCode).Append(",\n");
            sb.Append("  \"ErrorMessage\" : \"").Append(ErrorMessage).Append("\" ,\n");
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
            return obj.GetType() == GetType() && Equals((PostPurchaseLinkResponseBase)obj);
        }

        /// <summary>
        /// Returns true if PostPurchaseLinkRequestModel instances are equal
        /// </summary>
        /// <param name="other">Instance of PostPurchaseLinkRequestModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PostPurchaseLinkResponseBase? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    ErrorCode == other.ErrorCode ||
                    ErrorCode != null &&
                    ErrorCode.Equals(other.ErrorCode)
                    ) &&
                (
                    ErrorMessage == other.ErrorMessage ||
                    ErrorMessage != null &&
                    ErrorMessage.Equals(other.ErrorMessage)
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
                if (ErrorCode != null)
                    hashCode = hashCode * 59 + ErrorCode.GetHashCode();
                if (ErrorMessage != null)
                    hashCode = hashCode * 59 + ErrorMessage.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
#pragma warning disable 1591

        public static bool operator ==(PostPurchaseLinkResponseBase left, PostPurchaseLinkResponseBase right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PostPurchaseLinkResponseBase left, PostPurchaseLinkResponseBase right)
        {
            return !Equals(left, right);
        }

#pragma warning restore 1591
        #endregion Operators
    }
}