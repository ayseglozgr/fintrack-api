namespace FinTrack.Application.Common.Models;

public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();

    // Hızlıca başarılı cevap üretmek için static metotlar
    public static ServiceResponse<T> Success(T data, string message = "")
        => new() { Data = data, IsSuccess = true, Message = message };

    public static ServiceResponse<T> Failure(string message, List<string>? errors = null)
        => new() { IsSuccess = false, Message = message, Errors = errors ?? new List<string>() };
}

// Data dönmeyen, sadece işlem sonucu (başarılı/başarısız) dönen durumlar için (Örn: Delete işlemi)
public class ServiceResponse
{
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();

    public static ServiceResponse Success(string message = "")
        => new() { IsSuccess = true, Message = message };

    public static ServiceResponse Failure(string message, List<string>? errors = null)
        => new() { IsSuccess = false, Message = message, Errors = errors ?? new List<string>() };
}