using Core.Entities.Concrete;
using Core.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IUserDal:IEntityRepository<User>
    {
        List<OperationClaim> GetClaims(User user); // join operasyonu
    }
}
