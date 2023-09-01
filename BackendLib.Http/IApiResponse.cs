namespace BackendLib.Http;

public interface IApiResponse
{
    public Error[] Errors { get; }
}

public interface IApiResponse<out T> : IApiResponse
{
    public T? Data { get; }
}