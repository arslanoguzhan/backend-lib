using System.Text.Json.Serialization;

namespace BackendLib.Http;

public class ApiResponse<T> : IApiResponse<T>
{
    public T? Data { get; protected set; }

    public Error[] Errors { get; protected set; }

    public ApiResponse(T data)
    {
        Data = data;
        Errors = Array.Empty<Error>();
    }

    public ApiResponse(Error[] errors)
    {
        Errors = errors;
        Data = default;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse{T}"/> class.
    /// This constructor is reserved for JsonSerializers only
    /// </summary>
    [JsonConstructor]
    public ApiResponse(T data, Error[] errors)
    {
        if (errors is not null && errors.Length > 0)
        {
            Errors = errors;
            Data = default;
        }
        else
        {
            Data = data;
            Errors = Array.Empty<Error>();
        }
    }
}