# DisplayEnum -レスポンスコードとメッセージをペアで管理する-

## はじめに

社内プロジェクトでは、ビジネスロジック層からエラーなどののレスポンスの返し方がよろしくなく、レスポンスメッセージがハードコーディングされ、同じコードでソースの箇所によって異なるメッセージが返ってくる事さえありました。これを改善したいと考えC#にはDisplayEnumというものを知りレスポンスのコード：メッセージを1対1で管理するようにしました。このコードでは例えばレスポンスコードから動的にレスポンスメッセージを生成できるようにしております。

### メリット
- クライアント側ソースの可読性が高い
- レスポンスメッセージを一元管理でき一箇所で確認できるため保守性が高い
- レスポンス処理の一貫性を担保できるため開発者に拠らず共通の処理を行える
- ジェネリクス型で様々なEnumに対応できるため再利用性が高い
- テストを効率的に行える

### 今回必要とする機能と対応
- API自体のレスポンスコードとは別でビジネスロジックのレスポンスコードを持たせたい
  → 複数の型を定義しジェネリクスとして取得に対応
- key, value(=code)の他に表示メッセージを合わせて管理する必要がある
  → C#ではDisplayAttributeというアノテーションが使用でき対応
- マルチな取得方法に対応する
    - keyからvalue(コード)を取得する ※通常のENUMで可能
    - Keyから表示メッセージを取得する ※自作関数で可能とする必要あり
    - valueから表示名を取得する ※自作関数で可能とする必要あり

## 実装例
### クライアント側呼出部と取得結果
クライアント側から以下のように型やkey,valueを入力すると取得できるようにしています
```
// keyからvalueを取得：PurchaseRes.LackOfItemError to -1000
int responseCode = Convert.ToInt32(PurchaseRes.LackOfItemError);

// Keyから表示メッセージを取得：PurchaseRes.LackOfItemError to この商品は売り切れのため、購入できませんでした。
string responseMessage = CommonUtils.DisplayEnum(PurchaseRes.LackOfItemError);

// valueから表示名を取得:-1000 to この商品は売り切れのため、購入できませんでした。
responseCode = -1000; 
string responseMessage = CommonUtils.DisplayEnumByInt(new PurchaseRes(), responseCode);
```

### ENUM変換処理部
- Keyから表示メッセージを取得
APIRes、PurchaseRes等任意のEnumに対応できるようジェネリクス対応をしています。Enumのフィールドを取得しそのままメソッドチェーンでアトリビュートに設定されたメッセージが取得できます。

```
public static string DisplayEnum<T>(T enumKey) where T : Enum
{
    if (enumKey == null) throw new ArgumentNullException(nameof(enumKey));

    string internalFormat = enumKey.ToString();
    if (string.IsNullOrEmpty(internalFormat)) throw new ArgumentException("Invalid enum key");

    var dispAttribute = enumKey.GetType().GetField(internalFormat)?.GetCustomAttribute<DisplayAttribute>();

    return dispAttribute?.Name ?? internalFormat;
}
```
-  valueから表示名を取得
こちらは値をEnum型に変更してから前述の関数をCallしています。加えてオプショナル引数（任意引数）を設け任意の文字列を繋げる事ができます。
実務で使用した際は、孫連携先のレスポンスコードとメッセージを入れ子にするために追加しており、任意の文字列を自由に追加する事は推奨していません。
```
public static string DisplayEnumByInt<T>(T enumType, int val, string outsideMsg = "") where T : Enum
{
    var key = (T)Enum.ToObject(typeof(T), val);
    var msg = DisplayEnum(key);

    msg = outsideMsg != "" ? $"{outsideMsg} [コード：{val}] [メッセージ：{msg}]" : msg;

    return msg;
}
```

### ENUM定義部
Enumに[Display(Name = "ほげほげ")]といったようなC＃特有のアトリビュトという機能でメタデータを付与しています。（クラス、メソッド、プロティなどに付与可能）

- 購入処理のレスポンス用
```
public enum PurchaseRes
{
    [Display(Name = "正常に処理を終了しました")]
    Success = 0,

    [Display(Name = "この商品は売り切れのため、購入できませんでした。")]
    LackOfItemError = -1000,

    [Display(Name = "外部連携エラーです。")]
    LINKfailedError = -9000,

    [Display(Name = "予期せぬエラーが発生しました")]
    UnexpectedError = -9999
}
```

- API自体のレスポンス用
```
public enum APIRes
{
    [Display(Name = "正常に処理を終了しました")]
    Success = 0,

    [Display(Name = "ユーザー認証に失敗しました")]
    AuthError = -1000,

    [Display(Name = "予期せぬエラーが発生いたしました")]
    ExceptionError = -9999,
}
```