
namespace Gooeycms.Data.Model.Help
{
    public class HelpPageDao : BaseDao
    {
        public HelpPage FindByPageHash(Hash hash)
        {
            string hql = "select help from HelpPage help where help.Hash = :hash";
            return base.NewHqlQuery(hql).SetString("hash", hash.Value).UniqueResult<HelpPage>();
        }
    }
}
