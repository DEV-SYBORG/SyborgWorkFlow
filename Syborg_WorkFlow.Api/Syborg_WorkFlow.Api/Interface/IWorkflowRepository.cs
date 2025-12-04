using Syborg_WorkFlow.Api.Model;
using System;
using System.Threading.Tasks;

namespace Syborg_WorkFlow.Api.Repositories
{
    public interface IWorkflowRepository
    {
        Task<bool> IsNameTakenAsync(string name, Guid? excludeWorkflowId = null);
        Task CreateWorkflowAsync(Workflow workflow);
        Task<bool> IsWorkflowIdExistsAsync(Guid workflowId);
        Task UpdateWorkflowAsync(Workflow workflow);
        Task<List<GetAllWorkflow>> GetAllWorkflowsAsync();
        Task<List<WorkflowListDto>> GetWorkflowListAsync();
        Task<GetAllWorkflow?> GetWorkflowByIdAsync(Guid workflowId);
        Task DeleteWorkflowAsync(Guid workflowId, Guid updatedBy);
    }
}
