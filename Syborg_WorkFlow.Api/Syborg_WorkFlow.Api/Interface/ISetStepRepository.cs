using Syborg_WorkFlow.Api.Model;

namespace Syborg_WorkFlow.Api.Interface
{
    public interface ISetStepRepository
    {
        Task CreateSetStepAsync(SetStep step);
        Task<List<GetAllSetStep>> GetAllSetStepsAsync();
        //Task<List<SetStepList>> GetSetStepListAsync();
        Task<GetAllSetStep?> GetSetStepByIdAsync(Guid setStepId);
        Task<bool>IsSetStepExistsAsync(Guid SetStepId);
        Task UpdateSetStepAsync(SetStep step);
        Task DeleteSetStepAsync(Guid setStepId, Guid updatedBy);
    }
}
