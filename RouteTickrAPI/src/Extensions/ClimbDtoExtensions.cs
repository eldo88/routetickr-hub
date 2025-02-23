using System.ComponentModel;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;
using RouteTickrAPI.Enums;

namespace RouteTickrAPI.Extensions;

public static class ClimbDtoExtensions
{
    public static Climb ToEntity(this ClimbDto dto)
    {
        return dto.ClimbType switch
        {
            ClimbType.Sport => new SportRoute()
            {
                Name = dto.Name,
                Rating = dto.Rating,
                Location = dto.Location,
                Url = dto.Url,
                Height = dto.Height ?? 0,
                DangerRating = dto.DangerRating ?? ClimbDangerRating.Unknown,
                ClimbType = dto.ClimbType,
                NumberOfBolts = dto.NumberOfBolts ?? 0,
                NumberOfPitches = dto.NumberOfPitches ?? 1
            },
            ClimbType.Trad => new TradRoute()
            {
                Name = dto.Name,
                Rating = dto.Rating,
                Location = dto.Location,
                Url = dto.Url,
                Height = dto.Height ?? 0,
                DangerRating = dto.DangerRating ?? ClimbDangerRating.Unknown,
                ClimbType = dto.ClimbType,
                NumberOfPitches = dto.NumberOfPitches ?? 1,
                GearNeeded = dto.GearNeeded
            },
            ClimbType.Boulder => new Boulder()
            {
                Name = dto.Name,
                Rating = dto.Rating,
                Location = dto.Location,
                Url = dto.Url,
                Height = dto.Height ?? 0,
                DangerRating = dto.DangerRating ?? ClimbDangerRating.Unknown,
                ClimbType = dto.ClimbType,
                HasTopOut = dto.HasTopOut ?? true,
                NumberOfCrashPadsNeeded = dto.NumberOfCrashPadsNeeded ?? 1,
                IsTraverse = dto.IsTraverse ?? false
            },
            ClimbType.Ice => new IceClimb()
            {
                Name = dto.Name,
                Rating = dto.Rating,
                Location = dto.Location,
                Url = dto.Url,
                Height = dto.Height ?? 0,
                DangerRating = dto.DangerRating ?? ClimbDangerRating.Unknown,
                ClimbType = dto.ClimbType,
                IceType = dto.IceType,
                NumberOfPitches = dto.NumberOfPitches ?? 1
            },
            ClimbType.Alpine => new AlpineRockRoute()
            {
                Name = dto.Name,
                Rating = dto.Rating,
                Location = dto.Location,
                Url = dto.Url,
                Height = dto.Height ?? 0,
                DangerRating = dto.DangerRating ?? ClimbDangerRating.Unknown,
                ClimbType = dto.ClimbType,
                NumberOfPitches = dto.NumberOfPitches ?? 1,
                GearNeeded = dto.GearNeeded,
                ApproachDistance = dto.ApproachDistance ?? 0
            },
            _ => throw new InvalidEnumArgumentException("Invalid Climb Type")
        };
    }
}