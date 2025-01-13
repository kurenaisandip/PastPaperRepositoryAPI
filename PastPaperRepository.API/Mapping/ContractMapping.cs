using PastPaperRepository.Application.Models;
using PastPaperRepository.Application.Models.EducationalEntities;
using PastPaperRepository.Application.Models.Payment;
using PastPaperRepository.Application.Models.SpacedRepetition;
using PastPaperRepository.Contracts.Requests;
using PastPaperRepository.Contracts.Requests.EducationalEntities;
using PastPaperRepository.Contracts.Requests.Payments;
using PastPaperRepository.Contracts.Requests.SpacedRepetition;
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

    public static PastPapersResponse MapToPastPapersResponse(this IEnumerable<PastPapers> pastPapers, int page,
        int pageSize, int totalCount)
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
            SortOrder = request.SortBy is null ? SortOrder.UnSorted :
                request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
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

    public static LearningDeck MapToLearningDeck(this CreateLearningDeckRequest request)
    {
        return new LearningDeck
        {
            UserId = request.UserId,
            PastPaperId = request.PastPaperId,
            AddedDate = request.AddedDate,
            NextReviewDate = request.NextReviewDate,
            Status = request.Status,
        };
    }

    public static CreatePaymentModel MapToCreatePaymentModel(this CreatePaymentRequest request)
    {
        return new CreatePaymentModel
        {
            UserId = request.UserId,
            ProductName = request.ProductName,
            Price = request.Price,
            ValidUntil = request.ValidUntil,
        };
    }

    public static School MapToSchool(this CreateSchoolRequest request)
    {
        return new School
        {
            Name = request.Name,
            Address = request.Address,
        };
    }

    public static Subject MapToSubject(this CreateSubjectRequest request)
    {
        return new Subject
        {
            SubjectName = request.SubjectName,
            Description = request.Description
        };
    }

    public static Categories MapToCategories(this CreateCategoriesRequest request)
    {
        return new Categories
        {
            CategoryName = request.CategoryName,
            CategoryDescription = request.Description
        };
    }
    
    public static LoggedInUserDetails MapToLoggedInUserDetails(this SendUserDataRequest user)
    {
        return new LoggedInUserDetails
        {
            UserId = Convert.ToInt32(user.UserId),
            InstitutionName = user.InstitutionName,
            Grade = Convert.ToInt32(user.Grade),
            Semester = user.Semester,
            PhoneNumber = user.PhoneNumber,
            AcademicBackground = user.AcademicBackground
        };
    }
}