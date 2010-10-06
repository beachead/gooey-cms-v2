using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Subscription
{
    public class CmsInviteDao : BaseDao
    {
        public CmsInvite FindByEmail(string email)
        {
            String hql = "select invite from CmsInvite invite where invite.Email = :email";
            return base.NewHqlQuery(hql).SetString("email", email).UniqueResult<CmsInvite>();
        }

        public CmsInvite FindByGuid(Data.Guid guid)
        {
            String hql = "select invite from CmsInvite invite where invite.Guid = :guid";
            return base.NewHqlQuery(hql).SetString("guid", guid.Value).UniqueResult<CmsInvite>();
        }

        public IList<CmsInvite> FindInvites()
        {
            String hql = "select invite from CmsInvite invite order by invite.Issued desc, invite.Responded, invite.Created asc";
            return base.NewHqlQuery(hql).List<CmsInvite>();
        }
    }
}
