using FluentValidation;
using Syborg_WorkFlow.Api.Model;


namespace Syborg_WorkFlow.Api.Validators
{
    public class WorkflowValidator : AbstractValidator<Workflow>
    {
        public WorkflowValidator()
        {
            RuleFor(x => x.Workflow_Name)
                .NotEmpty().WithMessage("Workflow Name is required and must be at least 2 characters long.")
                .MinimumLength(2).WithMessage("Workflow Name is required and must be at least 2 characters long.")
                .MaximumLength(25).WithMessage("Workflow must not exceed 25 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.Status)
                .Must(status => status == true || status == false)
                .WithMessage("Status is required. Status must be either 'True' or 'False'.");


            RuleFor(x => x.Application_Id)
                .NotEmpty().WithMessage("Application is required.")
                .Must(id => id != Guid.Empty).WithMessage("Invalid Application GUID.");

            RuleFor(x => x.Module_Id)
                .NotEmpty().WithMessage("Module is required.")
                .Must(id => id != Guid.Empty).WithMessage("Invalid Module GUID.");

            RuleFor(x => x.StartingPage_Id)
                .NotEmpty().WithMessage("Starting Page is required.")
                .Must(id => id != Guid.Empty).WithMessage("Invalid Starting Page GUID.");

            RuleFor(x => x.User_Id)
                .NotEmpty().WithMessage("User Name is required.")
                .Must(id => id != Guid.Empty).WithMessage("Invalid User Name GUID.");
        }
    }
}