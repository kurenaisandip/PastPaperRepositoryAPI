namespace PastPaperRepository.Contracts.Responses.EducationalEntitiiesReponses;

public class RoleResponse
{
    public int RoleId { get; init; }
    
    public string Name { get; init; }
}

public class RoleResponses
{
    public IEnumerable<RoleResponse> RolesAsEnumerable => Enumerable.Empty<RoleResponse>();
}