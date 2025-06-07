using AssetManagement.Core.Shared;

namespace AssetManagement.Infrastructure.Extensions;

public static class ReportExtensions
{
    public static IQueryable<Report> Sort(this IQueryable<Report> reports, string? orderBy)
    {
        return orderBy?.ToLower() switch
        {
            "categorynameasc" => reports.OrderBy(r => r.CategoryName),
            "categorynamedesc" => reports.OrderByDescending(r => r.CategoryName),
            
            "totalasc" => reports.OrderBy(r => r.Total),
            "totaldesc" => reports.OrderByDescending(r => r.Total),
            
            "totalavailableasc" => reports.OrderBy(r => r.TotalAvailable),
            "totalavailabledesc" => reports.OrderByDescending(r => r.TotalAvailable),
            
            "totalnotavailableasc" => reports.OrderBy(r => r.TotalNotAvailable),
            "totalnotavailabledesc" => reports.OrderByDescending(r => r.TotalNotAvailable),
            
            "totalassignedasc" => reports.OrderBy(r => r.TotalAssigned),
            "totalassigneddesc" => reports.OrderByDescending(r => r.TotalAssigned),
            
            "totalwaitingforrecyclingasc" => reports.OrderBy(r => r.TotalWaitingForRecycling),
            "totalwaitingforrecyclingdesc" => reports.OrderByDescending(r => r.TotalWaitingForRecycling),
            
            "totalrecycledasc" => reports.OrderBy(r => r.TotalRecycled),
            "totalrecycleddesc" => reports.OrderByDescending(r => r.TotalRecycled),
            
            _ => reports.OrderBy(r => r.CategoryName)
        };
    }
}
