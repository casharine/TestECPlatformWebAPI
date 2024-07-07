using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Microsoft.Extensions.Primitives;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using System.Configuration;

namespace LineWebAPI.Models
{
	/// <summary>
	/// Lineレスポンス
	/// </summary>
	[DataContract]
	public class GetTestResponseBase : IEquatable<GetTestResponseBase>
	{
		/// <summary>
		/// エラーコード
		/// </summary>
		[Required]
		[DataMember(Name = "errorCode")]
		[JsonPropertyName("errorCode")]
		public int? errorCode { get; set; }

		/// <summary>
		/// エラーメッセージ
		/// </summary>
		[Required]
		[DataMember(Name = "errorMessage")]
		[JsonPropertyName("errorMessage")]
		public string errorMessage { get; set; }

		/// <summary> 
		/// 結果メッセージ 
		/// </summary>
		[DataMember(Name = "message")]
		[JsonPropertyName("message")]
		public string message { get; set; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <remarks></remarks>
		public GetTestResponseBase()
		{
			this.errorCode = 0;
			this.errorMessage = string.Empty;
			this.message = "疎通を確認しました";
		}

		/// <summary>
		/// Returns the string presentation of the object
		/// </summary>
		/// <returns>String presentation of the object</returns>
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("class TestGetResponse {\n");
			sb.Append("  \"errorCode\" : ").Append(errorCode).Append(",\n");
			sb.Append("  \"errorMessage\" : \"").Append(errorMessage).Append("\" ,\n");
			sb.Append("  \"message\" : \"").Append(message).Append("\" ,\n");
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
			return obj.GetType() == GetType() && Equals((GetTestResponseBase)obj);
		}

		/// <summary>
		/// Returns true if PostPurchaseLinkRequestModel instances are equal
		/// </summary>
		/// <param name="other">Instance of PostPurchaseLinkRequestModel to be compared</param>
		/// <returns>Boolean</returns>
		public bool Equals(GetTestResponseBase? other)
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
					) &&
				(
					message == other.message ||
					message != null &&
					message.Equals(other.message)

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
				// Suitable nullity checks etc, of course:)
				if (errorCode != null)
					hashCode = hashCode * 59 + errorCode.GetHashCode();
				if (errorMessage != null)
					hashCode = hashCode * 59 + errorMessage.GetHashCode();
				hashCode = hashCode * 59 + message.GetHashCode();
				return hashCode;
			}
		}

		#region Operators
#pragma warning disable 1591

		public static bool operator ==(GetTestResponseBase left, GetTestResponseBase right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(GetTestResponseBase left, GetTestResponseBase right)
		{
			return !Equals(left, right);
		}

#pragma warning restore 1591
		#endregion Operators
	}
}