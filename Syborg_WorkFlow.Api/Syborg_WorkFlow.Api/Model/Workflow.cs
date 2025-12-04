using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Syborg_WorkFlow.Api.Model
{
    public class Workflow
    {
        public Guid Workflow_Id { get; set; }
        public string Workflow_Name { get; set; }
        public string Description { get; set; }

        [DefaultValue("Active")]
        public string Status { get; set; } = "Active";
        public Guid StartingPage_Id { get; set; }
        public Guid ApplicationPage_Id { get; set; }
        public Guid User_Id { get; set; }
    }

    public class GetAllWorkflow
    {
        public Guid Workflow_Id { get; set; }
        public string Workflow_Name { get; set; }
        public string Description { get; set; }
        public Guid StartingPage_Id { get; set; }
        public Guid ApplicationPage_Id { get; set; }
        public Guid TimeStamp_Id { get; set; }

        // Active / Inactive status as string
        public string Status { get; set; } = string.Empty;

        
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
