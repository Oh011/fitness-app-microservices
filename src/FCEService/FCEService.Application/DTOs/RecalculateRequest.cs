namespace FCEService.Application.DTOs;

public class RecalculateRequest
{
    public string? Reason { get; set; }
    public double? NewWeight { get; set; }
    public string? TriggeredBy { get; set; }
}
