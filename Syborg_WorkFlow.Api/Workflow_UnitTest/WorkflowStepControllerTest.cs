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
    public class WorkflowStepControllerTests
    {
        private readonly Mock<IWorkflowStepRepository> _stepmockRepo;
        private readonly Mock<IValidator<WorkflowStep>> _stepmockValidator;
        private readonly WorkflowStepController _stepcontroller;

        public WorkflowStepControllerTests()
        {
            _stepmockRepo = new Mock<IWorkflowStepRepository>();
            _stepmockValidator = new Mock<IValidator<WorkflowStep>>();
            _stepcontroller = new WorkflowStepController(_stepmockRepo.Object, _stepmockValidator.Object);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenUserIdIsEmpty()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                User_Id = Guid.Empty,
                WorkflowName_Id = Guid.NewGuid()
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("User_Id", errors.Keys);
            Assert.Contains("User Name is required.", errors["User_Id"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenUserIdIsInvalidGuid()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                User_Id = Guid.Empty, 
                WorkflowName_Id = Guid.NewGuid()
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("User_Id", errors.Keys);
            Assert.Contains("Invalid User Name GUID.", errors["User_Id"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenWorkflowNameIdIsEmpty()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                User_Id = Guid.NewGuid(),
                WorkflowName_Id = Guid.Empty
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("WorkflowName_Id", errors.Keys);
            Assert.Contains("Workflow Name is required.", errors["WorkflowName_Id"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenWorkflowNameIdIsInvalidGuid()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                User_Id = Guid.NewGuid(),
                WorkflowName_Id = Guid.Empty // triggers Must rule
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("WorkflowName_Id", errors.Keys);
            Assert.Contains("Invalid Workflow Name GUID.", errors["WorkflowName_Id"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenStepNameIsEmpty()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                StepName = "" 
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("StepName", errors.Keys);
            Assert.Contains("Step Name is required.", errors["StepName"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenSequenceIsEmpty()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                Sequence = 0 // triggers NotEmpty rule (assuming 0 is invalid)
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Sequence", errors.Keys);
            Assert.Contains("Sequence is required.", errors["Sequence"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenSequenceIsOutOfRange()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                Sequence = 100 // triggers Between 1 to 99 rule
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Sequence", errors.Keys);
            Assert.Contains("Sequence must be a number between 1 and 99.", errors["Sequence"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenApplicationIdIsEmpty()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                Application_Id = Guid.Empty // NotEmpty rule
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Application_Id", errors.Keys);
            Assert.Contains("Application is required.", errors["Application_Id"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenApplicationIdIsInvalidGuid()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                Application_Id = Guid.Empty // Must rule
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Application_Id", errors.Keys);
            Assert.Contains("Invalid Application GUID.", errors["Application_Id"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenModuleIdIsEmpty()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                Module_Id = Guid.Empty // NotEmpty rule
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Module_Id", errors.Keys);
            Assert.Contains("Module is required.", errors["Module_Id"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenModuleIdIsInvalidGuid()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                Module_Id = Guid.Empty // Must rule
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Module_Id", errors.Keys);
            Assert.Contains("Invalid Module GUID.", errors["Module_Id"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenApplicationPageIdIsEmpty()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                ApplicationPage_Id = Guid.Empty // NotEmpty rule
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("ApplicationPage_Id", errors.Keys);
            Assert.Contains("Application Page is required.", errors["ApplicationPage_Id"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenApplicationPageIdIsInvalidGuid()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                ApplicationPage_Id = Guid.Empty // Must rule
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("ApplicationPage_Id", errors.Keys);
            Assert.Contains("Invalid Application Page GUID.", errors["ApplicationPage_Id"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenSectionIdIsEmpty()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                Section_Id = Guid.Empty // NotEmpty rule
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Section_Id", errors.Keys);
            Assert.Contains("Section is required.", errors["Section_Id"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenSectionIdIsInvalidGuid()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                Section_Id = Guid.Empty // Must rule
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Section_Id", errors.Keys);
            Assert.Contains("Invalid Section GUID.", errors["Section_Id"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenRoleIdsIsEmpty()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                RoleIds = new List<Guid>() // NotEmpty rule
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("RoleIds", errors.Keys);
            Assert.Contains("At least one role must be selected.", errors["RoleIds"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenRoleIdsContainsInvalidGuid()
        {
            var validator = new WorkflowStepValidator();
            var repoMock = new Mock<IWorkflowStepRepository>();
            var controller = new WorkflowStepController(repoMock.Object, validator);

            var step = new WorkflowStep
            {
                RoleIds = new List<Guid> { Guid.Empty } // Must rule
            };

            var result = await controller.CreateWorkflowStep(step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("RoleIds", errors.Keys);
            Assert.Contains("One or more Role IDs are invalid.", errors["RoleIds"]);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnBadRequest_WhenWorkflowStepIsNull()
        {
            var result = await _stepcontroller.CreateWorkflowStep(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        //UpdateWorkflowStep Tests

        [Fact]
        public async Task UpdateWorkflowStep_ShouldReturnBadRequest_WhenWorkflowStepIdIsEmpty()
        {
            var step = new WorkflowStep();

            var result = await _stepcontroller.UpdateWorkflowStep(Guid.Empty, step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task UpdateWorkflowStep_ShouldReturnBadRequest_WhenStepIsNull()
        {
            var result = await _stepcontroller.UpdateWorkflowStep(Guid.NewGuid(), null);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task UpdateWorkflowStep_ShouldReturnBadRequest_WhenUrlIdAndBodyIdDoNotMatch()
        {
            var urlId = Guid.NewGuid();

            var step = new WorkflowStep
            {
                WorkflowStep_Id = Guid.NewGuid()
            };

            var result = await _stepcontroller.UpdateWorkflowStep(urlId, step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task UpdateWorkflowStep_ShouldReturnNotFound_WhenWorkflowStepDoesNotExist()
        {
            var id = Guid.NewGuid();

            var step = new WorkflowStep
            {
                WorkflowStep_Id = id
            };

            _stepmockRepo
                .Setup(r => r.IsWorkflowStepExistsAsync(id))
                .ReturnsAsync(false);

            var result = await _stepcontroller.UpdateWorkflowStep(id, step);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFound.StatusCode);
        }

        [Fact]
        public async Task CreateWorkflowStep_ShouldReturnOk_WhenStepIsValid()
        {
            var step = new WorkflowStep
            {
                WorkflowStep_Id = Guid.NewGuid(),
                WorkflowName_Id = Guid.NewGuid(),
                StepName = "step1",
                Sequence = 1,
                Application_Id = Guid.NewGuid(),
                Module_Id = Guid.NewGuid(),
                ApplicationPage_Id = Guid.NewGuid(),
                Section_Id = Guid.NewGuid(),
                RoleIds = new List<Guid> { Guid.NewGuid() },
                User_Id = Guid.NewGuid()
            };

            _stepmockValidator
                .Setup(v => v.ValidateAsync(step, default))
                .ReturnsAsync(new ValidationResult());

            _stepmockRepo
                .Setup(r => r.CreateWorkflowStepAsync(step))
                .Returns(Task.CompletedTask);

            var result = await _stepcontroller.CreateWorkflowStep(step);

            Assert.IsType<OkObjectResult>(result);
        }

        private WorkflowStep GetValidStep()
        {
            return new WorkflowStep
            {
                WorkflowStep_Id = Guid.NewGuid(),
                WorkflowName_Id = Guid.NewGuid(),
                StepName = "step1",
                Sequence = 1,
                Application_Id = Guid.NewGuid(),
                Module_Id = Guid.NewGuid(),
                ApplicationPage_Id = Guid.NewGuid(),
                Section_Id = Guid.NewGuid(),
                RoleIds = new List<Guid> { Guid.NewGuid() },
                User_Id = Guid.NewGuid()
            };
        }
    }
}
