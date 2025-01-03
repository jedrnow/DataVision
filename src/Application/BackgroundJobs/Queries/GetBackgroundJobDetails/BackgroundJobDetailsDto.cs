﻿using DataVision.Domain.Entities;
using DataVision.Domain.Enums;

namespace DataVision.Application.BackgroundJobs.Queries.GetBackgroundJobDetails;
public class BackgroundJobDetailsDto
{
    public int Id { get; set; }
    public string? ExternalJobId { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsSucceeded { get; set; }
    public string? Message { get; set; }
    public string? Result { get; set; }
    public int? DatabaseId { get; set; }
    public BackgroundJobType Type { get; set; }
    public DateTimeOffset Created { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<BackgroundJob, BackgroundJobDetailsDto>();
        }
    }
}
