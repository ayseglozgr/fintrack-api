namespace FinTrack.Application.DTOs.ProxyTransaction;

public class ProxyOrchestrationResponseDto
{
    public string ProxyCaseUid { get; set; } = string.Empty;
    public string? LatestSettlementUid { get; set; }
}