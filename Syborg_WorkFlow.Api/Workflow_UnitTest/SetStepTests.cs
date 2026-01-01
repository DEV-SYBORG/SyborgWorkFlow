using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Syborg_WorkFlow.Api.Controller;
using Syborg_WorkFlow.Api.Interface;
using Syborg_WorkFlow.Api.Model;
using Syborg_WorkFlow.Api.Validators;
using Xunit;

namespace Workflow_UnitTest
{
    public class SetStepTests
    {
        private readonly Mock<ISetStepRepository> _mockRepo;
        private readonly Mock<IValidator<SetStep>> _mockValidator;
        private readonly SetStepController _controller;

        public SetStepTests()
        {
            _mockRepo = new Mock<ISetStepRepository>();
            _mockValidator = new Mock<IValidator<SetStep>>();
            _controller = new SetStepController(_mockRepo.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task CreateSetStep_ShouldReturnBadRequest_WhenWorkflowNameIdIsEmpty()
        {
            var validator = new SetStepValidator();
            var repoMock = new Mock<ISetStepRepository>();
            var controller = new SetStepController(repoMock.Object, validator);

            var setStep = new SetStep
            {
                WorkflowName_Id = Guid.Empty
            };

            var result = await controller.CreateSetStep(setStep);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("WorkflowName_Id", errors.Keys);
            Assert.Contains("Workflow Name is required.", errors["WorkflowName_Id"]);
        }


        [Fact]
        public async Task CreateSetStep_ShouldReturnBadRequest_WhenWorkflowNameIdIsInvalidGuid()
        {
            var validator = new SetStepValidator();
            var repoMock = new Mock<ISetStepRepository>();
            var controller = new SetStepController(repoMock.Object, validator);

            var setStep = new SetStep
            {
                WorkflowName_Id = Guid.Empty
            };

            var result = await controller.CreateSetStep(setStep);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("WorkflowName_Id", errors.Keys);
            Assert.Contains("Invalid Workflow Name GUID.", errors["WorkflowName_Id"]);
        }

        [Fact]
        public async Task CreateSetStep_ShouldReturnBadRequest_WhenWorkflowStepIdIsEmpty()
        {
            var validator = new SetStepValidator();
            var repoMock = new Mock<ISetStepRepository>();
            var controller = new SetStepController(repoMock.Object, validator);

            var setStep = new SetStep
            {
                WorkflowStep_Id = Guid.Empty
            };

            var result = await controller.CreateSetStep(setStep);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("WorkflowStep_Id", errors.Keys);
            Assert.Contains("Workflow Step is required.", errors["WorkflowStep_Id"]);
        }

        [Fact]
        public async Task CreateSetStep_ShouldReturnBadRequest_WhenWorkflowStepIdIsInvalidGuid()
        {
            var validator = new SetStepValidator();
            var repoMock = new Mock<ISetStepRepository>();
            var controller = new SetStepController(repoMock.Object, validator);

            var setStep = new SetStep
            {
                WorkflowStep_Id = Guid.Empty
            };

            var result = await controller.CreateSetStep(setStep);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("WorkflowStep_Id", errors.Keys);
            Assert.Contains("Invalid Workflow Step GUID.", errors["WorkflowStep_Id"]);
        }

        [Fact]
        public async Task CreateSetStep_ShouldReturnBadRequest_WhenIsConditionalIsEmpty()
        {
            var validator = new SetStepValidator();
            var repoMock = new Mock<ISetStepRepository>();
            var controller = new SetStepController(repoMock.Object, validator);

            var setStep = new SetStep
            {
                Is_Conditional = ""
            };

            var result = await controller.CreateSetStep(setStep);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Is_Conditional", errors.Keys);
            Assert.Contains("Is_Conditional is required.", errors["Is_Conditional"]);
        }

        [Fact]
        public async Task CreateSetStep_ShouldReturnBadRequest_WhenIsConditionalIsInvalid()
        {
            var validator = new SetStepValidator();
            var repoMock = new Mock<ISetStepRepository>();
            var controller = new SetStepController(repoMock.Object, validator);

            var setStep = new SetStep
            {
                Is_Conditional = "Maybe"
            };

            var result = await controller.CreateSetStep(setStep);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("Is_Conditional", errors.Keys);
            Assert.Contains(
                "Is_Conditional must be either 'Yes' or 'No'.",
                errors["Is_Conditional"]
            );
        }

        [Fact]
        public async Task CreateSetStep_ShouldReturnBadRequest_WhenNextStepYesIsEmpty()
        {
            var validator = new SetStepValidator();
            var repoMock = new Mock<ISetStepRepository>();
            var controller = new SetStepController(repoMock.Object, validator);

            var setStep = new SetStep
            {
                NextStep_Yes = Guid.Empty
            };

            var result = await controller.CreateSetStep(setStep);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("NextStep_Yes", errors.Keys);
            Assert.Contains("NextStep_Yes is required.", errors["NextStep_Yes"]);
        }

        [Fact]
        public async Task CreateSetStep_ShouldReturnBadRequest_WhenNextStepYesIsInvalidGuid()
        {
            var validator = new SetStepValidator();
            var repoMock = new Mock<ISetStepRepository>();
            var controller = new SetStepController(repoMock.Object, validator);

            var setStep = new SetStep
            {
                NextStep_Yes = Guid.Empty
            };

            var result = await controller.CreateSetStep(setStep);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("NextStep_Yes", errors.Keys);
            Assert.Contains("Invalid NextStep_Yes GUID.", errors["NextStep_Yes"]);
        }

        //[Fact]
        //public async Task CreateSetStep_ShouldReturnBadRequest_WhenIsConditionalYes_AndNextStepNoIsEmpty()
        //{
        //    var validator = new SetStepValidator();
        //    var repoMock = new Mock<ISetStepRepository>();
        //    var controller = new SetStepController(repoMock.Object, validator);

        //    var setStep = new SetStep
        //    {
        //        Is_Conditional = "Yes",
        //        NextStep_Yes = Guid.NewGuid(),   
        //        NextStep_No = Guid.Empty         
        //    };

        //    var result = await controller.CreateSetStep(setStep);

        //    var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        //    var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

        //    Assert.Contains("NextStep_No", errors.Keys);
        //    Assert.Contains(
        //        "NextStep_No is required when Is_Conditional = 'Yes'.",
        //        errors["NextStep_No"]
        //    );
        //}

        [Fact]
        public async Task CreateSetStep_ShouldReturnBadRequest_WhenIsConditionalYes_AndNextStepNoIsInvalidGuid()
        {
            var validator = new SetStepValidator();
            var repoMock = new Mock<ISetStepRepository>();
            var controller = new SetStepController(repoMock.Object, validator);

            var setStep = new SetStep
            {
                Is_Conditional = "Yes",
                NextStep_No = Guid.Empty
            };

            var result = await controller.CreateSetStep(setStep);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("NextStep_No", errors.Keys);
            Assert.Contains("Invalid NextStep_No GUID.", errors["NextStep_No"]);
        }

        [Fact]
        public async Task CreateSetStep_ShouldReturnBadRequest_WhenIsConditionalNo_AndNextStepNoIsProvided()
        {
            var validator = new SetStepValidator();
            var repoMock = new Mock<ISetStepRepository>();
            var controller = new SetStepController(repoMock.Object, validator);

            var setStep = new SetStep
            {
                Is_Conditional = "No",
                NextStep_No = Guid.NewGuid()
            };

            var result = await controller.CreateSetStep(setStep);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<Dictionary<string, string[]>>(badRequest.Value);

            Assert.Contains("NextStep_No", errors.Keys);
            Assert.Contains(
                "NextStep_No is not allowed when Is_Conditional = 'No'.",
                errors["NextStep_No"]
            );
        }

        // Update SetStep Tests
        [Fact]
        public async Task UpdateSetStep_ShouldReturnBadRequest_WhenSetStepIdIsEmpty()
        {
            var step = new SetStep();

            var result = await _controller.UpdateSetStepById(Guid.Empty, step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task UpdateSetStep_ShouldReturnBadRequest_WhenStepIsNull()
        {
            var result = await _controller.UpdateSetStepById(Guid.NewGuid(), null);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task UpdateSetStep_ShouldReturnBadRequest_WhenUrlIdAndBodyIdDoNotMatch()
        {
            var urlId = Guid.NewGuid();
            var step = new SetStep { SetStep_Id = Guid.NewGuid() };

            var result = await _controller.UpdateSetStepById(urlId, step);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task UpdateSetStep_ShouldReturnNotFound_WhenSetStepDoesNotExist()
        {
            var id = Guid.NewGuid();
            var step = new SetStep { SetStep_Id = id };

            // 🔹 Mock repository to return false (does not exist)
            _mockRepo.Setup(r => r.IsSetStepExistsAsync(id)).ReturnsAsync(false);

            var result = await _controller.UpdateSetStepById(id, step);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFound.StatusCode);
        }

    }
}
