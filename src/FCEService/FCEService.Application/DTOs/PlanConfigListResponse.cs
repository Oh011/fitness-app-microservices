namespace FCEService.Application.DTOs;

public class PlanConfigListResponse
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public List<PlanConfigDto> Items { get; set; } = new();
}
