using MonTraApi.Common;

namespace MonTraApi.Domains.DTOs;

public class ResultDTO<T>
{
    public CodeValue Code { get; set; } = CodeValue.Success;
    public string? ErrorCode { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

}
