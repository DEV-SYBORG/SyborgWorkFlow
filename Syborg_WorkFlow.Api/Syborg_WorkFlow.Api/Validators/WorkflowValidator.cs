using FluentValidation;
using Syborg_WorkFlow.Api.Model;


namespace Syborg_WorkFlow.Api.Validators
{
    public class WorkflowValidator : AbstractValidator<Workflow>
    {
        public WorkflowValidator()
        {
            RuleFor(x => x.Workflow_Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(3).WithMessage("Workflow Name must be at least 3 characters.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.")
                .Matches(@"^[A-Za-z]+(?: [A-Za-z]+)*$")
                .WithMessage("Name can only contain letters and single spaces between words.");

            
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");


            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status =>
                    status.Equals("Active", StringComparison.OrdinalIgnoreCase) ||
                    status.Equals("Inactive", StringComparison.OrdinalIgnoreCase)
                )
                .WithMessage("Status must be either 'Active' or 'Inactive'.");

        }
       
    }
}