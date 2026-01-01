using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Syborg_WorkFlow.Api.Model;
using Syborg_WorkFlow.Api.Repositories;

namespace Syborg_WorkFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowController : ControllerBase
    {
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IValidator<Workflow> _validator;

        public WorkflowController(IWorkflowRepository workflowRepository, IValidator<Workflow> validator)
        {
            _workflowRepository = workflowRepository;
            _validator = validator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateWorkflow([FromBody] Workflow workflow)
        {
            if (workflow == null)
            {
                return BadRequest(new { Message = "Invalid request body or missing JSON data." });
            }

            var validationResult = await _validator.ValidateAsync(workflow);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
                return BadRequest(errors);
            }

            bool isNameTaken = await _workflowRepository.IsNameTakenAsync(workflow.Workflow_Name);
            if (isNameTaken)
            {
                return Conflict(new { Message = "Workflow name already exists. Please choose a different name." });
            }

            try
            {
                await _workflowRepository.CreateWorkflowAsync(workflow);
                return Ok(new { Message = "Workflow created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating workflow.", Details = ex.Message });
            }
        }

        [HttpGet("GetAllWorkflowsByApplicationId")]
        public async Task<IActionResult> GetAllWorkflowsByApplicationId([FromQuery] Guid? applicationId)
        {
            try
            {
                var workflows = await _workflowRepository.GetAllWorkflowsByApplicationIdAsync(applicationId);

                if (workflows == null || !workflows.Any())
                    return NotFound(new { Message = "No workflows found." });

                return Ok(workflows);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching workflows.", Details = ex.Message });
            }
        }

        [HttpGet("GetWorkflowListByApplicationId")]
        public async Task<IActionResult> GetWorkflowListByApplicationId([FromQuery] Guid? applicationId)
        {
            try
            {
                var workflows = await _workflowRepository.GetWorkflowListByApplicationIdAsync(applicationId);

                if (workflows == null || !workflows.Any())
                    return NotFound(new { Message = "No workflows found." });

                return Ok(workflows);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Error fetching workflow list.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("GetWorkflowById/{id}")]
        public async Task<IActionResult> GetWorkflowById(Guid id)
        {
            //if (id == Guid.Empty)
            //    return BadRequest(new { Message = "Invalid Workflow Id." });

            try
            {
                var workflow = await _workflowRepository.GetWorkflowByIdAsync(id);

                if (workflow == null)
                    return NotFound(new { Message = "Workflow not found." });

                return Ok(workflow);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching workflow details.", Details = ex.Message });
            }
        }

        [HttpPut("Update/{Workflow_Id}")]
        public async Task<IActionResult> UpdateWorkflow(Guid Workflow_Id, [FromBody] Workflow workflow)
        {
            if (Workflow_Id == Guid.Empty)  // 00000000-0000-0000-0000-000000000000
                return BadRequest(new { Message = "Invalid Workflow Id." });

            if (workflow == null)
                return BadRequest(new { Message = "Request body is missing or invalid JSON format." });

            if (workflow.Workflow_Id != Guid.Empty && workflow.Workflow_Id != Workflow_Id)
                return BadRequest(new { Message = "URL Workflow ID and body Workflow Id do not match." });

            workflow.Workflow_Id = Workflow_Id;

            bool exists = await _workflowRepository.IsWorkflowIdExistsAsync(workflow.Workflow_Id);
            if (!exists)
            {
                return NotFound(new { Message = "Workflow Id not found or record has been deleted." });
            }

            try
            {
                
                bool nameTaken = await _workflowRepository.IsNameTakenAsync(workflow.Workflow_Name, workflow.Workflow_Id);
                if (nameTaken)
                {
                    return Conflict(new { Message = "Workflow name already exists. Please choose a different name." });
                }

                var validationResult = await _validator.ValidateAsync(workflow);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );
                    return BadRequest(errors);
                }

                await _workflowRepository.UpdateWorkflowAsync(workflow);
                return Ok(new { Message = "Workflow updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating workflow.", Details = ex.Message });
            }
        }


        [HttpDelete("Delete/{workflowId}")]
        public async Task<IActionResult> DeleteWorkflow(Guid workflowId)
        {
            if (workflowId == Guid.Empty)
                return BadRequest(new { Success = false, Message = "Invalid WorkflowId." });

            try
            {
                // dummy user id 
                var updatedBy = "11111111-1111-1111-1111-111111111111";

                
                await _workflowRepository.DeleteWorkflowAsync(workflowId, Guid.Parse(updatedBy));
                 
                return Ok(new { Success = true, Message = "Workflow deleted successfully." });
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                
                return NotFound(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { Success = false, Message = "Error deleting workflow.", Details = ex.Message });
            }
        }

    }
}
