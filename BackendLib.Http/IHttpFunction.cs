namespace BackendLib.Http;

public interface IHttpFunction<TRequest, TResponse>
{
    Task<ApiResponse<TResponse>> ExecuteAsync(TRequest request);
}
