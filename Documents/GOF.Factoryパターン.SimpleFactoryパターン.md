# ［GOF］Factoryパターン -SimpleFactoryパターン-

## はじめに
最終的な購入クラス生成にはSimpleFactoryパターンを採用しています。本パターンもGOFのFactoryパターンの一種で、単純な方法でオブジェクト生成ロジックを一箇所に集約しカプセル化を行う事できます。そのためクライアントが直接具象クラスをインスタンス化できないようになっています。
※ internalはJavaでいうpackage-privateと同義です。

## クラス図
```
+---------------------+
| PurchaseUtilities   |
|---------------------|
| + createSelectedPurchase( ) |
+---------------------+
        ^
        |
+---------------------+                  +------------------+
|      IPurchase      |<-- implement --- | PurchaseHOGE     |
|---------------------|               |  +------------------+
| + purchase( )       |               |  | + purchase( )    |
+---------------------+               |  +------------------+
        |                             |  +------------------+
        |                             |--| PurchaseFUGA     |
        |                             |  +------------------+
        |                             |  | + purchase( )    |
        |                             |  +------------------+
        |                             |  +------------------+
        |                             ---| PurchasePIYO     |
        |                                +------------------+
        |                                | + purchase( )    |
        |                                +------------------+
        |
+----------------------+    +---------------------+    +---------------------+
|   PurchaseResult     |    |   PurchaseParameter |    |      IAccesors      |
|----------------------|    |---------------------|    |---------------------|
| - ErrorCode          |    | - SITE_CD           |    | + getLoggerClient() |
| - ErrorMessage       |    | etc...              |    | etc...              |
| - CountLockFailed    |    +---------------------+    +---------------------+
+----------------------+
```

## 実装例

### IPurchase インターフェース
最終的に購入処理を実装する具象クラス（例: PurchaseHOGE, PurchaseFUGA, PurchasePIYO）を実装するためのインターフェースをまず作成します。
```
public interface IPurchase
{
    public PurchaseResult purchase();
}
```

### PurchaseFactory クラス
本クラスは、具象クラスのインスタンス化を行うファクトリーメソッドCreatePurchaseを持ちます。本Methodは与えられた siteCode に基づいて適切な具象クラス（PurchaseHOGE, PurchaseFUGA, PurchasePIYO）のインスタンスを生成します。このSwitch文の部分がこのソースにおいてはSimpleFactoryパターンの特徴点となり、生成方法を意識する必要がなく、引数から動的に目的のオブジェクトを生成し取得することができます。
※ simpleFactoryパターンにおいて必ずSwitch文が必要なわけではありません。、

```
internal static class PurchaseUtilities
{
    public static IPurchase createSelectedPurchase(PurchaseParameter iPurchaseParameter, IAccesors accesors)
    {
        IPurchase result = null!;

        try
        {
            // 購入クラス選定
            var target = iPurchaseParameter.SITE_CD switch
            {
                "1001" => result = new PurchaseHOGE(iPurchaseParameter, accesors),
                "1002" => result = new PurchaseFUGA(iPurchaseParameter, accesors),
                "1003" => result = new PurchasePIYO(iPurchaseParameter, accesors),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        catch (Exception ex)
        {
            accesors.getLoggerClient().WriteLog("createSelectedPurchase", $"コード{accesors.getCode()}の購入クラス生成に失敗しました");
            throw;
        }
        return result;
    }
}
```

## PurchaseResult クラス
本ファクトリで生成される具象クラス（PurchaseHOGE, PurchaseFUGA, PurchasePIYO）の戻り値として設定されるクラスです。
```
public class PurchaseResult
{
    public int ErrorCode { get; set; } = 0;
    public string ErrorMessage { get; set; } = string.Empty;
    public long CountLockFailed { get; set; } = 0;
}
```