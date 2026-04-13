using FluentValidation;

namespace task_manager.DTO.Validators
{
    public class CreateTaskDtoValidator : AbstractValidator <CreateTaskDto>
    {
        public CreateTaskDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Status required");
        }
    }
}
