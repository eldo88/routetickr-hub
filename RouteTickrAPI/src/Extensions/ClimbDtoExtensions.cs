using System.ComponentModel;
using RouteTickrAPI.DTOs;
using RouteTickr.Entities;
using RouteTickr.Messages.Enums;

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
    
    public static ClimbDto ToDto(this Climb climb)
    {
        return climb switch
        {
            SportRoute sport => new ClimbDto
            {
                Id = sport.Id,
                Name = sport.Name,
                Rating = sport.Rating,
                Location = sport.Location,
                Url = sport.Url,
                Height = sport.Height,
                DangerRating = sport.DangerRating,
                ClimbType = sport.ClimbType,
                NumberOfBolts = sport.NumberOfBolts,
                NumberOfPitches = sport.NumberOfPitches
            },
            TradRoute trad => new ClimbDto
            {
                Id = trad.Id,
                Name = trad.Name,
                Rating = trad.Rating,
                Location = trad.Location,
                Url = trad.Url,
                Height = trad.Height,
                DangerRating = trad.DangerRating,
                ClimbType = trad.ClimbType,
                NumberOfPitches = trad.NumberOfPitches,
                GearNeeded = trad.GearNeeded
            },
            Boulder boulder => new ClimbDto
            {
                Id = boulder.Id,
                Name = boulder.Name,
                Rating = boulder.Rating,
                Location = boulder.Location,
                Url = boulder.Url,
                Height = boulder.Height,
                DangerRating = boulder.DangerRating,
                ClimbType = boulder.ClimbType,
                HasTopOut = boulder.HasTopOut,
                NumberOfCrashPadsNeeded = boulder.NumberOfCrashPadsNeeded,
                IsTraverse = boulder.IsTraverse
            },
            IceClimb ice => new ClimbDto
            {
                Id = ice.Id,
                Name = ice.Name,
                Rating = ice.Rating,
                Location = ice.Location,
                Url = ice.Url,
                Height = ice.Height,
                DangerRating = ice.DangerRating,
                ClimbType = ice.ClimbType,
                IceType = ice.IceType,
                NumberOfPitches = ice.NumberOfPitches
            },
            AlpineRockRoute alpine => new ClimbDto
            {
                Id = alpine.Id,
                Name = alpine.Name,
                Rating = alpine.Rating,
                Location = alpine.Location,
                Url = alpine.Url,
                Height = alpine.Height,
                DangerRating = alpine.DangerRating,
                ClimbType = alpine.ClimbType,
                NumberOfPitches = alpine.NumberOfPitches,
                GearNeeded = alpine.GearNeeded,
                ApproachDistance = alpine.ApproachDistance
            },
            _ => throw new InvalidEnumArgumentException("Invalid Climb Type")
        };
    }
}