using Microsoft.EntityFrameworkCore;

namespace Webport.ERP.Common.Application.Database;

public interface IDbContextProvider
{
    DbContext GetContext();
}