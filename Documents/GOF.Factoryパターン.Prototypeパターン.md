# ［GOF］Factoryパターン - Prototypeパターン -


## はじめに
購入処理時のロックオブジェクトの生成・取得にはSingletonFactoryパターンを採用しています。本パターンもGOFのFactoryパターンの一種で、カプセル化を行いながらアプリケーション全体で唯一のインスタンスを担保していきます。これにより複数のスレッドから同時にアクセスされても、正しく動作するようにスレッドセーフに実装することが可能で、競合状態や同期の問題を回避できます。

また、最終的にはサイト毎にロックオブジェクトをシングルトンにしたいので動的にロックオブジェクトを生成・取得するようにアレンジしています。

## クラス図
```
 ---------------------------------------
|             ICloneable               |
 ---------------------------------------
| + Clone(): object                    |
 ---------------------------------------
          ^
          | implements
 ---------------------------------------
|           PurchaseResult             |
 ---------------------------------------
| + ErrorCode: int                     |
| + ErrorMessage: string               |
| + CountLockFailed: long              |
 ---------------------------------------
| + Clone(): object                    |
 ---------------------------------------
          ^
          |
 ---------------------------------------
|            呼出し元                   |
 ---------------------------------------
```

## 実装例
### 呼出し元 PurchaseConnector.purchaseLink
連携先の連携購入結果情報を連携元のインスタンスへクローンしています。

```
public void purchaseLink(PurchaseParameter iPurchaseParameter, ref PurchaseResult rPurchaseResult)
{
    IPurchase Purchase = null!;

    // 連携先購入クラス、パラメータ生成
    try
    {
        Purchase = new LinkedPurchaseFactory().createPurchase(iPurchaseParameter);
    }catch (APIException ex){
        throw ex;
    }

    // 連携購入処理実行
    var wPurchaseResult = Purchase.purchase();

    //プロトタイプパターンで連携先のエラー情報に更新
    rPurchaseResult = (PurchaseResult)wPurchaseResult.Clone();

    // エラー処理
    if (wPurchaseResult.ErrorCode != 0)
    {
        rPurchaseResult.ErrorMessage = CommonUtils.DisplayEnumByInt(new PurchaseRes(), rPurchaseResult.ErrorCode, "外部連携エラー ");
        rPurchaseResult.ErrorCode = -9000;
    }
}
```

### 購入結果クラス PurchaseResult
連携購入結果をクローンするクローンメソッドを作成します。.NetにはICloneableというインターフェイスが予め用意されているので特にインターフェイスは作成する必要はありません。

#### PurchaseResult
```
public class PurchaseResult : ICloneable
{
    public int ErrorCode { get; set; } = 0;
    public string ErrorMessage { get; set; } = string.Empty;
    public long CountLockFailed { get; set; } = 0;


    // クローンメソッドの実装
    public object Clone()
    {
        return new PurchaseResult
        {
            ErrorCode = this.ErrorCode,
            ErrorMessage = this.ErrorMessage,
            CountLockFailed = this.CountLockFailed
        };
    }
}
```
#### ICloneable(.Net標準インターフェイス)
```
public interface ICloneable
{
    object Clone();
}
```