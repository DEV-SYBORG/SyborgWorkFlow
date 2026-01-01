using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Syborg_WorkFlow.Api.Interface;
using Syborg_WorkFlow.Api.Model;

namespace Syborg_WorkFlow.Api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SetStepController : ControllerBase
    {
        private readonly ISetStepRepository _repository;
        private readonly IValidator<SetStep> _validator;

        public SetStepController(ISetStepRepository repository, IValidator<SetStep> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateSetStep([FromBody] SetStep step)
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
                await _repository.CreateSetStepAsync(step);
                return Ok(new { Message = "Set steps created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating set steps.", Details = ex.Message });
            }
        }

        [HttpGet("GetAllSetSteps")]
        public async Task<IActionResult> GetAllSetSteps()
        {
            try
            {
                var SetStep = await _repository.GetAllSetStepsAsync();
                return Ok(SetStep);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching set steps.", Details = ex.Message });
            }
        }

        //[HttpGet("GetSetStepList")]
        //public async Task<IActionResult> GetSetStepList()
        //{
        //    try
        //    {
        //        var SetSteps = await _repository.GetSetStepListAsync();

        //        if (SetSteps == null || !SetSteps.Any())
        //            return NotFound(new { Message = "No set steps found." });

        //        return Ok(SetSteps);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Message = "Error fetching set step list.", Details = ex.Message });
        //    }
        //}

        [HttpGet("GetSetStepById/{SetStep_id}")]
        public async Task<IActionResult> GetSetStepById(Guid SetStep_id)
        {
            try
            {
                var setStep = await _repository.GetSetStepByIdAsync(SetStep_id);

                if (setStep == null)
                    return NotFound(new { Message = "Set Step not found." });

                return Ok(setStep);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Error fetching set step details.",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("Update/{SetStepId}")]
        public async Task<IActionResult> UpdateSetStepById(Guid SetStepId, [FromBody] SetStep step)
        {
            if (SetStepId == Guid.Empty)  // 00000000-0000-0000-0000-000000000000
                return BadRequest(new { Message = "Invalid SetStepId." });

            if (step == null)
                return BadRequest(new { Message = "Request body is missing or invalid JSON format." });

            if (step.SetStep_Id != Guid.Empty && step.SetStep_Id != SetStepId)
                return BadRequest(new { Message = "URL SetStep ID and body SetStep_Id do not match." });

            step.SetStep_Id = SetStepId;

            bool exists = await _repository.IsSetStepExistsAsync(SetStepId);
            if (!exists)
                return NotFound(new { Message = "SetStep Id not found or record is deleted." });

            try
            {
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

                await _repository.UpdateSetStepAsync(step);
                return Ok(new { Message = "Set Step updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating set step.", Details = ex.Message });
            }
        }

        [HttpDelete("Delete/{setStepId}")]
        public async Task<IActionResult> DeleteSetStep(Guid setStepId)
        {
            if (setStepId == Guid.Empty)
                return BadRequest(new { Success = false, Message = "Invalid SetStep Id." });

            try
            {
                // dummy user id 
                var updatedBy = "11111111-1111-1111-1111-111111111111";


                await _repository.DeleteSetStepAsync(setStepId, Guid.Parse(updatedBy));

                return Ok(new { Success = true, Message = "SetStep deleted successfully." });
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {

                return NotFound(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Success = false, Message = "Error deleting SetStep.", Details = ex.Message });
            }
        }
    }
}
