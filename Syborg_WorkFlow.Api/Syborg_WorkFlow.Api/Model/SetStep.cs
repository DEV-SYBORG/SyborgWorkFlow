namespace Syborg_WorkFlow.Api.Model
{
    public class SetStep
    {
        public Guid SetStep_Id { get; set; }
        public Guid WorkflowName_Id { get; set; }
        public Guid WorkflowStep_Id { get; set; }
        public string Is_Conditional { get; set; }
        public Guid NextStep_Yes { get; set; }   // Nullable to allow for no next step and indicate end of workflow
        public Guid? NextStep_No { get; set; }    // Store ApplicationPage_Id of the next step
        public Guid User_Id { get; set; }
    }

    public class GetAllSetStep
    {
        public Guid SetStep_Id { get; set; }
        public Guid WorkflowName_Id { get; set; }
        public Guid WorkflowStep_Id { get; set; }
        public string Is_Conditional { get; set; }
        public Guid NextStep_Yes { get; set; }
        public Guid? NextStep_No { get; set; }
        public Guid TimeStamp_Id { get; set; }
        public Guid Created_By { get; set; }
        public DateTime Created_At { get; set; }
        public Guid? Updated_By { get; set; }
        public DateTime? Updated_At { get; set; }
        public string Old_Data { get; set; }
        public string Updated_Data { get; set; }
        public string Update_Status { get; set; }
    }


    //public class  SetStepList
    //{
    //    public Guid SetStep_Id { get; set; }
    //    public string SetStepName { get; set; }
    //}
}
