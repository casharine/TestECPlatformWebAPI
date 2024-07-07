namespace TestECPlatformWebAPI.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Text.Json.Serialization;

    namespace TestECPlatformWebAPI.Models
    {
        /// <summary>
        /// INbodyのモデル 
        /// </summary>
        /// <remarks></remarks>
        [DataContract]
        public class PostPurchaseLinkRequestBase : IEquatable<PostPurchaseLinkRequestBase>
        {
            /// <summary>
            /// ユーザID
            /// </summary>
            [Required]
            [DataMember(Name = "user_id")]
            [JsonPropertyName("user_id")]
            public string USER_ID { get; set; } = null!;

            /// <summary>
            /// 購入時使用した買参人のコード ※API化時追加
            /// </summary>
            [Required]
            [DataMember(Name = "shozoku_cd")]
            [JsonPropertyName("shozoku_cd")]
            public string SHOZOKU_CD { get; set; } = null!;

            /// <summary>
            /// 購入した商品コード ※※API化時変更：旧OROSI_CD
            /// </summary>
            [DataMember(Name = "shohin_cd")]
            [JsonPropertyName("shohin_cd")]
            public string SHOHIN_CD { get; set; } = null!;

            /// <summary>
            /// 購入したサイトコード※API化時追加
            /// </summary>
            [DataMember(Name = "site_cd")]
            [JsonPropertyName("site_cd")]
            public string SITE_CD { get; set; } = null!;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PostPurchaseLinkRequestBase()
            {
                this.USER_ID = "";
            }

            /// <summary>
            /// Returns the string presentation of the object
            /// </summary>
            /// <returns>String presentation of the object</returns>
            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.Append("class PostPurchaseLinkRequest {\n");
                sb.Append("  \"user_id\": \"").Append(USER_ID).Append("\",\n");
                sb.Append("  \"shozoku_cd\": \"").Append(SHOZOKU_CD).Append("\",\n");
                sb.Append("  \"shohin_cd\": \"").Append(SHOHIN_CD).Append("\",\n");
                sb.Append("  \"site_cd\": \"").Append(SITE_CD).Append("\",\n");
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
                return obj.GetType() == GetType() && Equals((PostPurchaseLinkRequestBase)obj);
            }

            /// <summary>
            /// Returns true if GetSeikInfoModelModel instances are equal
            /// </summary>
            /// <param name="other">Instance of GetSeikInfoModelModel to be compared</param>
            /// <returns>Boolean</returns>
            public bool Equals(PostPurchaseLinkRequestBase? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;

                return
                    (
                        USER_ID == other.USER_ID ||
                        USER_ID != null &&
                        USER_ID.Equals(other.USER_ID)
                    ) && (
                        SHOZOKU_CD == other.SHOZOKU_CD ||
                        SHOZOKU_CD != null &&
                        SHOZOKU_CD.Equals(other.SHOZOKU_CD)
                    ) && (
                        SITE_CD == other.SITE_CD ||
                        SITE_CD != null &&
                    SITE_CD.Equals(other.SITE_CD)
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
                    if (USER_ID != null)
                        hashCode = hashCode * 59 + USER_ID.GetHashCode();
                    if (SHOZOKU_CD != null)
                        hashCode = hashCode * 59 + SHOZOKU_CD.GetHashCode();
                    if (SITE_CD != null)
                        hashCode = hashCode * 59 + SITE_CD.GetHashCode();
                    if (SHOHIN_CD != null)
                        hashCode = hashCode * 59 + SHOHIN_CD.GetHashCode();
                    return hashCode;
                }
            }

            #region Operators
#pragma warning disable 1591

            public static bool operator ==(PostPurchaseLinkRequestBase left, PostPurchaseLinkRequestBase right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(PostPurchaseLinkRequestBase left, PostPurchaseLinkRequestBase right)
            {
                return !Equals(left, right);
            }

#pragma warning restore 1591
            #endregion Operators
        }
    }
}
