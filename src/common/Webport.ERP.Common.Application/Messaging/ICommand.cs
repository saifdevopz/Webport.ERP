namespace Webport.ERP.Common.Application.Messaging;

public interface ICommand : ICommand<Result>;

public interface ICommand<TResponse> : IBaseCommand;

public interface IBaseCommand;
