using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Syborg_WorkFlow.Api.Controllers;
using Syborg_WorkFlow.Api.Model;
using Syborg_WorkFlow.Api.Repositories;
using Syborg_WorkFlow.Api.Validators;
using Xunit;


namespace Workflow_UnitTest
{
    public class WorkflowTests
    {
        private readonly Mock<IWorkflowRepository> _mockRepo;
        private readonly Mock<IValidator<Workflow>> _mockValidator;
        private readonly WorkflowController _controller;

        public WorkflowTests()
        {
            _mockRepo = new Mock<IWorkflowRepository>();
            _mockValidator = new Mock<IValidator<Workflow>>();
            _controller = new WorkflowController(_mockRepo.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task CreateWorkflow_ShouldReturnBadRequest_WhenWorkflowNameIsEmpty()
        {
            var validator = new WorkflowValidator();
            var repoMock = new Mock<IWorkflowRepository>();
            var controller = new WorkflowController(repoMock.Object, validator);

            var workflow = new Workflow
            {
                Workflow_Name = ""  // empty string
            };

            var result = await controller.CreateWorkflow(workflow);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);

            // assuming controller returns Dictionary<string,string[]>
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Workflow_Name", errors.Keys);
            Assert.Contains("Workflow Name is required and must be at least 2 characters long.", errors["Workflow_Name"]);
        }


        [Fact]
        public async Task CreateWorkflow_ShouldReturnBadRequest_WhenWorkflowNameIsTooShort()
        {
            var validator = new WorkflowValidator();
            var repoMock = new Mock<IWorkflowRepository>();
            var controller = new WorkflowController(repoMock.Object, validator);

            var workflow = new Workflow
            {
                Workflow_Name = "A"
            };

            var result = await controller.CreateWorkflow(workflow);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Workflow_Name", errors.Keys);
            Assert.Contains("Workflow Name is required and must be at least 2 characters long.", errors["Workflow_Name"]);
        }

        [Fact]
        public async Task CreateWorkflow_ShouldReturnBadRequest_WhenWorkflowNameIsTooLong()
        {
            var validator = new WorkflowValidator();
            var repoMock = new Mock<IWorkflowRepository>();
            var controller = new WorkflowController(repoMock.Object, validator);

            var workflow = new Workflow
            {
                Workflow_Name = new string('A', 30)
            };

            var result = await controller.CreateWorkflow(workflow);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Workflow_Name", errors.Keys);
            Assert.Contains("Workflow must not exceed 25 characters.", errors["Workflow_Name"]);
        }


        [Fact]
        public async Task CreateWorkflow_ShouldReturnBadRequest_WhenDescriptionExceeds500Characters()
        {
            
            var validator = new WorkflowValidator();
            var repoMock = new Mock<IWorkflowRepository>();
            var controller = new WorkflowController(repoMock.Object, validator);

            var workflow = new Workflow
            {
                Workflow_Name = "Valid Workflow",
                Description = new string('A', 501)   // ❌ more than 500
            };

            var result = await controller.CreateWorkflow(workflow);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Description", errors.Keys);
            Assert.Contains("Description must not exceed 500 characters.", errors["Description"]);
        }

        //[Fact]
        //public async Task CreateWorkflow_ShouldReturnOk_WhenStatusIsTrueOrFalse()
        //{
        //    var validator = new WorkflowValidator();
        //    var repoMock = new Mock<IWorkflowRepository>();
        //    repoMock.Setup(r => r.CreateWorkflowAsync(It.IsAny<Workflow>())).ReturnsAsync(true);

        //    var controller = new WorkflowController(repoMock.Object, validator);

        //    var workflow = new Workflow
        //    {
        //        Workflow_Name = "Valid Workflow",
        //        Status = true
        //    };

        //    var result = await controller.CreateWorkflow(workflow);

        //    Assert.IsType<CreatedAtActionResult>(result);
        //}


        [Fact]
        public async Task CreateWorkflow_ShouldReturnBadRequest_WhenApplicationIdIsEmpty()
        {
            var validator = new WorkflowValidator();
            var repoMock = new Mock<IWorkflowRepository>();
            var controller = new WorkflowController(repoMock.Object, validator);

            var workflow = new Workflow
            {
                Workflow_Name = "Test Workflow",
                Application_Id = Guid.Empty
            };

            var result = await controller.CreateWorkflow(workflow);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Application_Id", errors.Keys);
            Assert.Contains("Application is required.", errors["Application_Id"]);
        }

        [Fact]
        public async Task CreateWorkflow_ShouldReturnBadRequest_WhenApplicationIdIsInvalidGuid()
        {
            var validator = new WorkflowValidator();
            var repoMock = new Mock<IWorkflowRepository>();
            var controller = new WorkflowController(repoMock.Object, validator);

            var workflow = new Workflow
            {
                Workflow_Name = "Test Workflow",
                Application_Id = Guid.Empty
            };

            var result = await controller.CreateWorkflow(workflow);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Application_Id", errors.Keys);
            Assert.Contains("Invalid Application GUID.", errors["Application_Id"]);
        }

        [Fact]
        public async Task CreateWorkflow_ShouldReturnBadRequest_WhenModuleIdIsEmpty()
        {
            var validator = new WorkflowValidator();
            var repoMock = new Mock<IWorkflowRepository>();
            var controller = new WorkflowController(repoMock.Object, validator);

            var workflow = new Workflow
            {
                Workflow_Name = "Test Workflow",
                Module_Id = Guid.Empty
            };

            var result = await controller.CreateWorkflow(workflow);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Module_Id", errors.Keys);
            Assert.Contains("Module is required.", errors["Module_Id"]);
        }

        [Fact]
        public async Task CreateWorkflow_ShouldReturnBadRequest_WhenStartingPageIdIsEmpty()
        {
            var validator = new WorkflowValidator();
            var repoMock = new Mock<IWorkflowRepository>();
            var controller = new WorkflowController(repoMock.Object, validator);

            var workflow = new Workflow
            {
                Workflow_Name = "Test Workflow",
                StartingPage_Id = Guid.Empty
            };

            var result = await controller.CreateWorkflow(workflow);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("StartingPage_Id", errors.Keys);
            Assert.Contains("Starting Page is required.", errors["StartingPage_Id"]);
        }

        [Fact]
        public async Task CreateWorkflow_ShouldReturnBadRequest_WhenUserIdIsEmpty()
        {
            var validator = new WorkflowValidator();
            var repoMock = new Mock<IWorkflowRepository>();
            var controller = new WorkflowController(repoMock.Object, validator);

            var workflow = new Workflow
            {
                Workflow_Name = "Test Workflow",
                User_Id = Guid.Empty
            };

            var result = await controller.CreateWorkflow(workflow);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("User_Id", errors.Keys);
            Assert.Contains("User Name is required.", errors["User_Id"]);
        }

        [Fact]
        public async Task CreateWorkflow_ShouldReturnBadRequest_WhenWorkflowIsNull()
        {
            var result = await _controller.CreateWorkflow(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateWorkflow_ShouldReturnConflict_WhenWorkflowNameAlreadyExists()
        {
            var workflow = GetValidWorkflow();

            _mockValidator
                .Setup(v => v.ValidateAsync(workflow, default))
                .ReturnsAsync(new ValidationResult());

            _mockRepo
                .Setup(r => r.IsNameTakenAsync(workflow.Workflow_Name, null))
                .ReturnsAsync(true);

            var result = await _controller.CreateWorkflow(workflow);

            Assert.IsType<ConflictObjectResult>(result);
        }

        //UpdateWorkflow Tests 

        [Fact]
        public async Task UpdateWorkflow_ShouldReturnBadRequest_WhenWorkflowIdIsEmpty()
        {
            var workflow = new Workflow();

            var result = await _controller.UpdateWorkflow(Guid.Empty, workflow);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task UpdateWorkflow_ShouldReturnBadRequest_WhenUrlIdAndBodyIdDoNotMatch()
        {
            var urlId = Guid.NewGuid();

            var workflow = new Workflow
            {
                Workflow_Id = Guid.NewGuid() // different from URL ID
            };

            var result = await _controller.UpdateWorkflow(urlId, workflow);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task UpdateWorkflow_ShouldReturnNotFound_WhenWorkflowDoesNotExist()
        {
            var id = Guid.NewGuid();

            var workflow = new Workflow
            {
                Workflow_Id = id
            };

            _mockRepo
                .Setup(r => r.IsWorkflowIdExistsAsync(id))
                .ReturnsAsync(false);

            var result = await _controller.UpdateWorkflow(id, workflow);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFound.StatusCode);
        }



        [Fact]
        public async Task CreateWorkflow_ShouldReturnOk_WhenWorkflowIsValid()
        {
            var workflow = new Workflow
            {
                Workflow_Name = "Valid Workflow",
                Description = "Test",
                Status = true,
                Application_Id = Guid.NewGuid(),
                Module_Id = Guid.NewGuid(),
                StartingPage_Id = Guid.NewGuid(),
                User_Id = Guid.NewGuid()
            };

            _mockValidator
                .Setup(v => v.ValidateAsync(workflow, default))
                .ReturnsAsync(new ValidationResult());

            _mockRepo
                .Setup(r => r.IsNameTakenAsync(workflow.Workflow_Name, null))
                .ReturnsAsync(false);

            _mockRepo
                .Setup(r => r.CreateWorkflowAsync(workflow))
                .Returns(Task.CompletedTask);

            var result = await _controller.CreateWorkflow(workflow);

            Assert.IsType<OkObjectResult>(result);
        }

        private Workflow GetValidWorkflow()
        {
            return new Workflow
            {
                Workflow_Id = Guid.NewGuid(),
                Workflow_Name = "Test Workflow",
                Description = "Valid description",
                Status = true,
                Application_Id = Guid.NewGuid(),
                Module_Id = Guid.NewGuid(),
                StartingPage_Id = Guid.NewGuid(),
                User_Id = Guid.NewGuid()
            };
        }
    }
}
