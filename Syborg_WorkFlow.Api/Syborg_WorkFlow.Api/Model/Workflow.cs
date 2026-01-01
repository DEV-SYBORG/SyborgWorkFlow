using System.ComponentModel;

namespace Syborg_WorkFlow.Api.Model
{
    public class Workflow
    {
        public Guid Workflow_Id { get; set; }
        public string Workflow_Name { get; set; }
        public string Description { get; set; }

        [DefaultValue("true")]
        public bool Status { get; set; } = true;
        public Guid Application_Id { get; set; }
        public Guid Module_Id { get; set; }
        public Guid StartingPage_Id { get; set; }
        public Guid User_Id { get; set; }
    }

    public class GetAllWorkflow
    {
        public Guid Workflow_Id { get; set; }
        public string Workflow_Name { get; set; }
        public string Description { get; set; }
        public Guid Application_Id { get; set; }
        public Guid Module_Id { get; set; }
        public Guid StartingPage_Id { get; set; }
        public Guid TimeStamp_Id { get; set; }

        // True / False status as string
        public bool? Status { get; set; }

        
        public Guid Created_By { get; set; }
        public DateTime Created_At { get; set; }
        public Guid? Updated_By { get; set; }
        public DateTime? Updated_At { get; set; }
        public string? Old_Data { get; set; }
        public string? Updated_Data { get; set; }
        public string? Update_Status { get; set; }
    }

    public class WorkflowListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

}
