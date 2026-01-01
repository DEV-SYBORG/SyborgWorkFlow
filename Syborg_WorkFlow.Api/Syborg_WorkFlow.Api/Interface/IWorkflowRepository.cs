using Syborg_WorkFlow.Api.Model;

namespace Syborg_WorkFlow.Api.Repositories
{
    public interface IWorkflowRepository
    {
        Task<bool> IsNameTakenAsync(string name, Guid? excludeWorkflowId = null);
        Task CreateWorkflowAsync(Workflow workflow);
        Task<bool> IsWorkflowIdExistsAsync(Guid workflowId);
        Task UpdateWorkflowAsync(Workflow workflow);
        Task<List<GetAllWorkflow>> GetAllWorkflowsByApplicationIdAsync(Guid? applicationId);
        Task<List<WorkflowListDto>> GetWorkflowListByApplicationIdAsync(Guid? applicationId);
        Task<GetAllWorkflow?> GetWorkflowByIdAsync(Guid workflowId);
        Task DeleteWorkflowAsync(Guid workflowId, Guid updatedBy);
    }
}
