﻿using PastPaperRepository.Application.Models;
using PastPaperRepository.Application.Models.EducationalEntities;
using PastPaperRepository.Contracts.Requests;
using PastPaperRepository.Contracts.Requests.EducationalEntities;
using PastPaperRepository.Contracts.Responses;
using PastPaperRepository.Contracts.Responses.EducationalEntitiiesReponses;

namespace PastPaperRepository.API.Mapping;

public static class ContractMapping
{
    public static PastPapers MapToPastPapers(this CreatePastPaperRequest request)
    {
        return new PastPapers()
        {
            PastPaperId = Guid.NewGuid().ToString(),
            Title = request.Title,
            SubjectId = request.SubjectId,
            CategoryId = request.CategoryId,
            Year = request.Year,
            ExamType = request.ExamType,
            DifficultyLevel = request.DifficultyLevel,
            ExamBoard = request.ExamBoard,
            FilePath = request.FilePath
        };
    }

    public static PastPaperResponse MapToResponsePastPaper(this PastPapers pastPaper)
    {
        return new PastPaperResponse
        {
            PastPaperId = pastPaper.PastPaperId,
            Title = pastPaper.Title,
            Slug = pastPaper.Slug,
            SubjectId = pastPaper.SubjectId,
            CategoryId = pastPaper.CategoryId,
            Year = pastPaper.Year,
            ExamType = pastPaper.ExamType,
            DifficultyLevel = pastPaper.DifficultyLevel,
            ExamBoard = pastPaper.ExamBoard,
            FilePath = pastPaper.FilePath
        };

    }

    public static PastPapersResponse MapToPastPapersResponse(this IEnumerable<PastPapers> pastPapers, int page, int pageSize,int totalCount)
    {
        return new PastPapersResponse()
        {
            PastPapers = pastPapers.Select(p => p.MapToResponsePastPaper()),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    
    public static PastPapers MapToPastPapers(this UpdatePastPaperRequest request, string id)
    {
        return new PastPapers()
        {
            PastPaperId = id,
            Title = request.Title,
            SubjectId = request.SubjectId,
            CategoryId = request.CategoryId,
            Year = request.Year,
            ExamType = request.ExamType,
            DifficultyLevel = request.DifficultyLevel,
            ExamBoard = request.ExamBoard,
            FilePath = request.FilePath
        };
    }
    
    public static UpdatePastPaperResponse MapToUpdatedPastPaper(this PastPapers response)
    {
        return new UpdatePastPaperResponse()
        {
            PastPaperId = response.PastPaperId,
            Title = response.Title,
            SubjectId = response.SubjectId,
            CategoryId = response.CategoryId,
            Year = response.Year,
            ExamType = response.ExamType,
            DifficultyLevel = response.DifficultyLevel,
            ExamBoard = response.ExamBoard,
            FilePath = response.FilePath
        };
    }
    
    public static UsersLogin MapToUsersLogin(this LogInUser request)
    {
        return new UsersLogin()
        {
            Email = request.Email,
            Password = request.Password
        };
    } 
    public static UsersLogin MapToUsersRegister(this RegisterUser request)
    {
        return new UsersLogin()
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password
        };
    }
    
    public static GetAllPastPapersOptions MapToGetAllPastPapersOptions(this GetAllPastPapersRequest request)
    {
        return new GetAllPastPapersOptions()
        {
            Title = request.Title,
            Year = request.Year,
            SortField = request.SortBy?.Trim('+', '-'),
            SortOrder = request.SortBy is null ? SortOrder.UnSorted : request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
            Page = request.Page,
            PageSize = request.PageSize,
            
        };
    }
    
    public static GetAllPastPapersOptions WithUserId(this GetAllPastPapersOptions options, string? userId)
    {
        options.UserID = userId;
        return options;
    }

    public static Roles MapToRoles(this CreateRoleRequest request)
    {
        return new Roles
        {
            Name = request.Name
        };
    }  
    public static RoleResponse MapToResponseRole(this Roles request)
    {
        return new RoleResponse
        {
            Name = request.Name
        };
    }
    
}