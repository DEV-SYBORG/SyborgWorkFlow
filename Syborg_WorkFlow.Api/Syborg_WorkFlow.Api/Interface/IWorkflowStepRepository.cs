using Syborg_WorkFlow.Api.Model;
using Syborg_WorkFlow.Api.Model.Syborg_WorkFlow.Api.Model;

namespace Syborg_WorkFlow.Api.Repositories
{
    public interface IWorkflowStepRepository
    {
        Task CreateWorkflowStepAsync(WorkflowStep step);
        Task<List<GetAllWorkflowStep>> GetAllWorkflowStepsAsync();
        Task<List<WorkflowStepListDto>> GetWorkflowStepListAsync();
        Task<GetAllWorkflowStep?> GetWorkflowStepByIdAsync(Guid workflowStepId);
        Task<bool> IsWorkflowStepExistsAsync(Guid workflowStepId);
        Task UpdateWorkflowStepAsync(WorkflowStep step);

    }
}
