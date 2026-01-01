namespace Syborg_WorkFlow.Api.Model
{
    public class WorkflowStep
    {
        public Guid WorkflowStep_Id { get; set; }
        public Guid WorkflowName_Id { get; set; }
        public string StepName { get; set; }
        public int Sequence { get; set; }
        public Guid Application_Id { get; set; }
        public Guid Module_Id { get; set; }
        public Guid ApplicationPage_Id { get; set; }
        public Guid Section_Id { get; set; }
        public List<Guid> RoleIds { get; set; } = new List<Guid>();
        public Guid User_Id { get; set; }
    }

    public class GetAllWorkflowStep
    {
        public Guid WorkflowStep_Id { get; set; }
        public Guid WorkflowName_Id { get; set; }
        public string StepName { get; set; }
        public int Sequence { get; set; }
        public Guid Application_Id { get; set; }
        public Guid Module_Id { get; set; }
        public Guid ApplicationPage_Id { get; set; }
        public Guid Section_Id { get; set; }
        public string Role_Ids { get; set; }
        public Guid TimeStamp_Id { get; set; }

        public Guid Created_By { get; set; }
        public DateTime Created_At { get; set; }
        public Guid? Updated_By { get; set; }
        public DateTime? Updated_At { get; set; }
        public string Old_Data { get; set; }
        public string Updated_Data { get; set; }
        public string Update_Status { get; set; }
    }

    public class WorkflowStepListDto
    {
        public Guid Id { get; set; }
        public string StepName { get; set; }
    }


    
}
