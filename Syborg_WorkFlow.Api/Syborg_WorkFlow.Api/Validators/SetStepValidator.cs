using FluentValidation;
using Syborg_WorkFlow.Api.Model;

namespace Syborg_WorkFlow.Api.Validators
{
    public class SetStepValidator : AbstractValidator<SetStep>
    {
        public SetStepValidator()
        {
            RuleFor(x => x.WorkflowName_Id)
                .NotEmpty().WithMessage("Workflow Name is required.")
                .Must(id => id != Guid.Empty).WithMessage("Invalid Workflow Name GUID.");

            RuleFor(x => x.WorkflowStep_Id)
                .NotEmpty().WithMessage("Workflow Step is required.")
                .Must(id => id != Guid.Empty).WithMessage("Invalid Workflow Step GUID.");

            RuleFor(x => x.Is_Conditional)
                .NotEmpty().WithMessage("Is_Conditional is required.")
                .Must(value => value == "Yes" || value == "No")
                .WithMessage("Is_Conditional must be either 'Yes' or 'No'.");

            RuleFor(x => x.NextStep_Yes)
                .NotEmpty().WithMessage("NextStep_Yes is required.")
                .Must(id => id != Guid.Empty).WithMessage("Invalid NextStep_Yes GUID.");

            When(x => x.Is_Conditional == "Yes", () =>
            {
                RuleFor(x => x.NextStep_No)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("NextStep_No is required when Is_Conditional = 'Yes'.")
                    .Must(id => id != Guid.Empty).WithMessage("Invalid NextStep_No GUID.");
            });

            When(x => x.Is_Conditional == "No", () =>
            {
                RuleFor(x => x.NextStep_No)
                    .Cascade(CascadeMode.Stop)
                    .Must(id => id == null || id == Guid.Empty)
                    .WithMessage("NextStep_No is not allowed when Is_Conditional = 'No'.");
            });
        }

    }
}
