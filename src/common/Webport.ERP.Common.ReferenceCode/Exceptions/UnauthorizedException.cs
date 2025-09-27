//using System.Collections.ObjectModel;
//using System.Net;

//namespace BuildingBlocks.Domain.Exceptions;

//public class UnauthorizedException : BaseException
//{
//    public UnauthorizedException()
//    : base("authentication failed", new Collection<string>(), HttpStatusCode.Unauthorized)
//    {
//    }
//    public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
//    {
//    }

//    public UnauthorizedException(string message)
//        : base(message, new Collection<string>(), HttpStatusCode.Unauthorized)
//    {
//    }
//}

//public class PaginatedResult<TEntity>
//    (int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
//    where TEntity : class
//{
//    public int PageIndex { get; } = pageIndex;
//    public int PageSize { get; } = pageSize;
//    public long Count { get; } = count;
//    public IEnumerable<TEntity> Data { get; } = data;
//}

//public record PaginationRequest(int PageIndex = 0, int PageSize = 10);



