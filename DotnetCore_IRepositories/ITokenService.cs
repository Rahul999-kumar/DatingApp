using DotnetCore_BusinessModels;
using DotnetCore_Database.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetCore_IRepositories
{
    public interface ITokenService
    {
        string CreateToken(Users user);
    }
}
