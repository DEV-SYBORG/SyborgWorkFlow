using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Syborg_WorkFlow.Api.Model;
using Syborg_WorkFlow.Api.Model.Syborg_WorkFlow.Api.Model;
using Syborg_WorkFlow.Api.Repositories;
using Syborg_WorkFlow.Api.Service;

namespace Syborg_WorkFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowStepController : ControllerBase
    {
        private readonly WorkflowStepRepository _repository;
        private readonly IValidator<WorkflowStep> _validator;

        public WorkflowStepController(WorkflowStepRepository repository, IValidator<WorkflowStep> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateWorkflowStep([FromBody] WorkflowStep step)
        {
            if (step == null)
            {
                return BadRequest(new { Message = "Invalid request body or missing JSON data." });
            }

            // Validate using FluentValidation
            var validationResult = await _validator.ValidateAsync(step);

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

            try
            {
                await _repository.CreateWorkflowStepAsync(step);
                return Ok(new { Message = "Workflow Steps created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating Workflow Steps.", Details = ex.Message });
            }
        }

        [HttpGet("GetAllWorkflowStep")]
        public async Task<IActionResult> GetAllWorkflows()
        {
            try
            {
                var workflows = await _repository.GetAllWorkflowStepsAsync();
                return Ok(workflows);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching workflows.", Details = ex.Message });
            }
        }

        [HttpGet("GetWorkflowStepList")]
        public async Task<IActionResult> GetWorkflowStepList()
        {
            try
            {
                var workflowSteps = await _repository.GetWorkflowStepListAsync();

                if (workflowSteps == null || !workflowSteps.Any())
                    return NotFound(new { Message = "No workflow steps found." });

                return Ok(workflowSteps);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching workflow step list.", Details = ex.Message });
            }
        }

        [HttpGet("GetWorkflowStepById/{id}")]
        public async Task<IActionResult> GetWorkflowStepById(Guid id)
        {
            try
            {
                var workflowStep = await _repository.GetWorkflowStepByIdAsync(id);

                if (workflowStep == null)
                    return NotFound(new { Message = "WorkflowStep not found." });

                return Ok(workflowStep);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Error fetching workflow step details.",
                    Details = ex.Message
                });
            }
        }


        [HttpPut("UpdateWorkflowStep")]
        public async Task<IActionResult> UpdateWorkflowStep([FromBody] WorkflowStep step)
        {
            if (step == null)
            {
                return BadRequest(new { Message = "Request body is missing or invalid JSON format." });
            }

            var validationResult = await _validator.ValidateAsync(step);
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
            try
            {
                bool exists = await _repository.IsWorkflowStepExistsAsync(step.WorkflowStep_Id);

                if (!exists)
                {
                    return NotFound(new
                    {
                        Message = "Workflow Step Id not found or record is deleted."
                    });
                }
                await _repository.UpdateWorkflowStepAsync(step);
                return Ok(new { Message = "Workflow Step updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching workflowSteps.", Details = ex.Message });
            }
        }

        [HttpDelete("Delete/{workflowStepId}")]
        public async Task<IActionResult> DeleteWorkflowStep(Guid workflowStepId)
        {
            if (workflowStepId == Guid.Empty)
                return BadRequest(new { Success = false, Message = "Invalid workflowStepId." });

            try
            {
                // dummy user id 
                var updatedBy = "11111111-1111-1111-1111-111111111111";


                await _repository.DeleteWorkflowStepAsync(workflowStepId, Guid.Parse(updatedBy));

                return Ok(new { Success = true, Message = "WorkflowStep deleted successfully." });
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {

                return NotFound(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Success = false, Message = "Error deleting WorkflowStep.", Details = ex.Message });
            }
        }

    }
}

    

