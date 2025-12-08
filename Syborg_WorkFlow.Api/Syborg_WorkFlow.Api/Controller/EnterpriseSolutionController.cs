using Microsoft.AspNetCore.Mvc;
using Syborg_WorkFlow.Api.Service;

namespace Syborg_WorkFlow.Api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnterpriseSolutionController : ControllerBase
    {
        private readonly EnterpriseSolutionService _enterpriseService;
        public EnterpriseSolutionController(EnterpriseSolutionService enterpriseService)
        {
            _enterpriseService = enterpriseService;
        }

        [HttpGet("GetApplicationLists")]
        public async Task<IActionResult> GetApplicationList()
        {
            try
            {
                var sections = await _enterpriseService.GetApplicationListAsync();
                return Ok(sections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving page sections.", Details = ex.Message });
            }
        }

        [HttpGet("GetModuleLists")]
        public async Task<IActionResult> GetModuleList()
        {
            try
            {
                var sections = await _enterpriseService.GetModuleListAsync();
                return Ok(sections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving page sections.", Details = ex.Message });
            }
        }

        [HttpGet("GetModuleByApplicationId/{applicationId}")]
        public async Task<IActionResult> GetModuleByApplicationId(Guid applicationId)
        {
            try
            {
                var sections = await _enterpriseService.GetModuleByApplicationIdAsync(applicationId);

                if (sections == null || !sections.Any())
                {
                    return NotFound(new
                    {
                        message = "No Module found for the given Application Id."
                    });
                }

                return Ok(sections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving sections.", Details = ex.Message });
            }
        }

        [HttpGet("GetApplicationPageList")]
        public async Task<IActionResult> GetApplicationPageList()
        {
            try
            {
                var pages = await _enterpriseService.GetApplicationPageListAsync();
                return Ok(pages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving application pages.", Details = ex.Message });
            }
        }

        [HttpGet("GetApplicationPageByModuleId/{moduleId}")]
        public async Task<IActionResult> GetApplicationPageByModuleId(Guid moduleId)
        {
            try
            {
                var sections = await _enterpriseService.GetApplicationPageByModuleIdAsync(moduleId);

                if (sections == null || !sections.Any())
                {
                    return NotFound(new
                    {
                        message = "No ApplicationPage found for the given Module Id."
                    });
                }

                return Ok(sections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving sections.", Details = ex.Message });
            }
        }

        [HttpGet("GetPageSectionList")]
        public async Task<IActionResult> GetPageSectionsList()
        {
            try
            {
                var sections = await _enterpriseService.GetPageSectionListAsync();
                return Ok(sections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving page sections.", Details = ex.Message });
            }
        }

        [HttpGet("GetSectionByApplicationPageId/{applicationPageId}")]
        public async Task<IActionResult> GetSectionByApplicationPageId(Guid applicationPageId)
        {
            try
            {
                var sections = await _enterpriseService.GetSectionByApplicationPageIdAsync(applicationPageId);

                if (sections == null || !sections.Any())
                {
                    return NotFound(new
                    {
                        message = "No Section found for the given ApplicationPage Id."
                    });
                }

                return Ok(sections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving sections.", Details = ex.Message });
            }
        }


        [HttpGet("GetRoleListList")]
        public async Task<IActionResult> GetRoleList()
        {
            try
            {
                var sections = await _enterpriseService.GetRoleListAsync();
                return Ok(sections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving page sections.", Details = ex.Message });
            }
        }
    }
}
