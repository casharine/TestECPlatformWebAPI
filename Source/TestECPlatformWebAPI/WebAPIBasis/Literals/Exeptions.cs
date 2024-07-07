namespace WebAPIBasis.Literals
{

    public class APIException : Exception
    {
        /// <summary>
        /// 任意のメッセージ任意の例外を作成します
        /// </summary>
        /// <param name="iMessage"></param>
        public APIException(string iMessage) :base(iMessage){ }

        public APIException(string iMessage, string iInnerMessage) : base($"{iMessage}:{iInnerMessage}") { }

        /// <summary>
        /// 例外をネストし任意の例外を作成します
        /// </summary>
        /// <param name="iMessage"></param>
        /// <param name="innerException"></param>
        public APIException(string iMessage, Exception innerException) : base(iMessage, innerException) { }
    }

    /// <summary>
    /// 独自定義エクセプション
    /// </summary>
    public class MyException : Exception
    {
        public MyException()
            :base("-9010"){}
    }
}
