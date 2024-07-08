using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;
using Microsoft.VisualBasic;
using Oracle.ManagedDataAccess.Client;

namespace WebAPIBasis.Accesors
{
    /// <summary>
    /// 【 概要 】アクセサオブジェクト群、SiteAccesorsを一括更新する、基底インターフェイス、抽象ファクトリ
    /// 【 対象 】DB接続情報、AppXXX.jsonアクセサ、DB接続プール、ロガーオブジェクト 【！！要クラス内readme参照！！】
    /// </summary>
    /// 
    /// <readme>
    /// 0.カプセル化しており、getterはグローバル、setterはAccesorクラス専用（正確にはintarnal,プロジェクト内）
    /// 1.CLS_Purchaseインスタンス化時に必ずSiteAccesorsGeneratorFactoryで更新する事
    /// 2.再帰的にCLS_Purchase呼出(=接続先、SITE_CDを変更)したの場合、retun前後に呼出元SITE_CDで元に戻す事
    /// </readme>
    public interface IAccesorsFactory
    {
        IAccesors CreateAccessors(string siteCode);
    }

    public class ServerAccesorsFactory : IAccesorsFactory
    {
        public IAccesors CreateAccessors(string siteCode)
        {
            return new ServerAccesors(siteCode);
        }
    }

    public class ClientAccesorsFactory : IAccesorsFactory
    {
        public IAccesors CreateAccessors(string siteCode)
        {
            return new ClientAccesors(siteCode);
        }
    }
}
