using FluentValidation;
using Syborg_WorkFlow.Api.Model;

namespace Syborg_WorkFlow.Api.Validators
{
    public class WorkflowStepValidator : AbstractValidator<WorkflowStep>
    {
        public WorkflowStepValidator()
        {
            RuleFor(x => x.User_Id)
                .NotEmpty().WithMessage("User Name is required.")
                .Must(id => id != Guid.Empty).WithMessage("Invalid User Name GUID.");

            RuleFor(x => x.WorkflowName_Id)
                .NotEmpty().WithMessage("Workflow Name is required.")
                .Must(id => id != Guid.Empty).WithMessage("Invalid Workflow Name GUID.");

            RuleFor(x => x.StepName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Step Name is required.");

            RuleFor(x => x.Sequence)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Sequence is required.")
                .Must(x => x >= 1 && x <= 99)
                .WithMessage("Sequence must be a number between 1 and 99.");

            RuleFor(x => x.Application_Id)
                .NotEmpty().WithMessage("Application is required.")
                .Must(id => id != Guid.Empty).WithMessage("Invalid Application GUID.");

            RuleFor(x => x.Module_Id)
                .NotEmpty().WithMessage("Module is required.")
                .Must(id => id != Guid.Empty).WithMessage("Invalid Module GUID.");

            RuleFor(x => x.ApplicationPage_Id)
                .NotEmpty().WithMessage("Application Page is required.")
                .Must(id => id != Guid.Empty).WithMessage("Invalid Application Page GUID.");

            RuleFor(x => x.Section_Id)
                .NotEmpty().WithMessage("Section is required.")
                .Must(id => id != Guid.Empty).WithMessage("Invalid Section GUID.");

            RuleFor(x => x.RoleIds)
                .NotEmpty().WithMessage("At least one role must be selected.")
                .Must(list => list.All(id => id != Guid.Empty))
                .WithMessage("One or more Role IDs are invalid.");

        }
    }

}
