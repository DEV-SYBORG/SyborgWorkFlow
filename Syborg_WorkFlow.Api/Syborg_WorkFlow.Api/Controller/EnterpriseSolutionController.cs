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
