using Microsoft.Data.SqlClient;
using Syborg_WorkFlow.Api.Model;
using System;
using System.Data;

namespace Syborg_WorkFlow.Api.Repositories
{
    public class WorkflowRepository : IWorkflowRepository
    {
        private readonly string _connectionString;

        public WorkflowRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("WorkflowDB");
        }

        // Check if Workflow Name is already exists using stored procedure
        public async Task<bool> IsNameTakenAsync(string name, Guid? excludeWorkflowId = null)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_CheckWorkflowNameExists", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Workflow_Name", name);
            if (excludeWorkflowId.HasValue)
                cmd.Parameters.AddWithValue("@ExcludeWorkflowId", excludeWorkflowId.Value);
            else
                cmd.Parameters.AddWithValue("@ExcludeWorkflowId", DBNull.Value);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result) > 0; // true = name already exists
        }


        // Create Workflow using stored procedure
        public async Task CreateWorkflowAsync(Workflow workflow)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_InsertWorkflowWithTimestamp", conn)
            {
                CommandType = CommandType.StoredProcedure
            };


            cmd.Parameters.AddWithValue("@Workflow_Name", workflow.Workflow_Name);
            cmd.Parameters.AddWithValue("@Description", workflow.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Application_Id", workflow.Application_Id);
            cmd.Parameters.AddWithValue("@Module_Id", workflow.Module_Id);
            cmd.Parameters.AddWithValue("@StartingPage_Id", workflow.StartingPage_Id);
            cmd.Parameters.AddWithValue("@User_Id", workflow.User_Id);
            cmd.Parameters.Add("@Status", SqlDbType.Bit).Value = workflow.Status;


            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // GetAll Workflow and Get All Workflows By Application By Id using stored procedure
        public async Task<List<GetAllWorkflow>> GetAllWorkflowsByApplicationIdAsync(Guid? applicationId)
        {
            var workflows = new List<GetAllWorkflow>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_GetAllWorkflowsByApplicationId", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Guid.Empty OR null → send NULL to SP
            if (!applicationId.HasValue || applicationId == Guid.Empty)
            {
                cmd.Parameters.Add("@Application_Id", SqlDbType.UniqueIdentifier).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@Application_Id", SqlDbType.UniqueIdentifier).Value = applicationId.Value;
            }

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                workflows.Add(new GetAllWorkflow
                {
                    Workflow_Id = reader.GetGuid(reader.GetOrdinal("Workflow_Id")),
                    Workflow_Name = reader["Workflow_Name"]?.ToString(),
                    Description = reader["Description"]?.ToString(),
                    Application_Id = reader.GetGuid(reader.GetOrdinal("Application_Id")),
                    Module_Id = reader.GetGuid(reader.GetOrdinal("Module_Id")),
                    StartingPage_Id = reader.GetGuid(reader.GetOrdinal("StartingPage_Id")),
                    TimeStamp_Id = reader.GetGuid(reader.GetOrdinal("TimeStamp_Id")),
                    Status = reader.GetBoolean(reader.GetOrdinal("Status")),

                    Created_By = reader.GetGuid(reader.GetOrdinal("Created_By")),
                    Created_At = reader.GetDateTime(reader.GetOrdinal("Created_At")),
                    Updated_By = reader["Updated_By"] == DBNull.Value ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("Updated_By")),
                    Updated_At = reader["Updated_At"] == DBNull.Value ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Updated_At")),
                    Old_Data = reader["Old_Data"] == DBNull.Value ? null : reader["Old_Data"].ToString(),
                    Updated_Data = reader["Updated_Data"] == DBNull.Value ? null : reader["Updated_Data"].ToString(),
                    Update_Status = reader["Update_Status"] == DBNull.Value ? null : reader["Update_Status"].ToString()
                });
            }

            return workflows;
        }

        // GetAll Workflows List and Get All Workflows List By Application By Id using stored procedure
        public async Task<List<WorkflowListDto>> GetWorkflowListByApplicationIdAsync(Guid? applicationId)
        {
            var workflows = new List<WorkflowListDto>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_GetWorkflowListByApplication_Id", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Guid.Empty OR null → send NULL to SP
            if (!applicationId.HasValue || applicationId == Guid.Empty)
            {
                cmd.Parameters.Add("@Application_Id", SqlDbType.UniqueIdentifier).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@Application_Id", SqlDbType.UniqueIdentifier).Value = applicationId.Value;
            }

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                workflows.Add(new WorkflowListDto
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader["Name"]?.ToString()
                });
            }

            return workflows;
        }

        public async Task<GetAllWorkflow?> GetWorkflowByIdAsync(Guid workflowId)
        {
            GetAllWorkflow? workflow = null;

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_GetWorkflowById", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Workflow_Id", workflowId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                workflow = new GetAllWorkflow
                {
                    Workflow_Id = reader.GetGuid(reader.GetOrdinal("Workflow_Id")),
                    Workflow_Name = reader["Workflow_Name"]?.ToString(),
                    Description = reader["Description"]?.ToString(),
                    Status = reader.GetBoolean(reader.GetOrdinal("Status")),
                    Application_Id = reader.GetGuid(reader.GetOrdinal("Application_Id")),
                    Module_Id = reader.GetGuid(reader.GetOrdinal("Module_Id")),
                    StartingPage_Id = reader.GetGuid(reader.GetOrdinal("StartingPage_Id")),
                    TimeStamp_Id = reader.GetGuid(reader.GetOrdinal("TimeStamp_Id")),

                    Created_By = reader.GetGuid(reader.GetOrdinal("Created_By")),
                    Created_At = reader.GetDateTime(reader.GetOrdinal("Created_At")),
                    Updated_By = reader["Updated_By"] == DBNull.Value ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("Updated_By")),
                    Updated_At = reader["Updated_At"] == DBNull.Value ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Updated_At")),

                    Old_Data = reader["Old_Data"] == DBNull.Value ? null : reader["Old_Data"].ToString(),
                    Updated_Data = reader["Updated_Data"] == DBNull.Value ? null : reader["Updated_Data"].ToString(),
                    Update_Status = reader["Update_Status"] == DBNull.Value ? null : reader["Update_Status"].ToString()

                }; ;
            }

            return workflow;
        }

        // Check if WorkflowId exists and not deleted using stored procedure
        public async Task<bool> IsWorkflowIdExistsAsync(Guid workflowId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_CheckWorkflowExists", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Workflow_Id", workflowId);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result) == 1; // true = exists & not deleted
        }

        // Update Workflow using stored procedure
        public async Task UpdateWorkflowAsync(Workflow workflow)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_UpdateWorkflowWithTimestamp", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Workflow_Id", workflow.Workflow_Id);
            cmd.Parameters.AddWithValue("@Workflow_Name", workflow.Workflow_Name);
            cmd.Parameters.AddWithValue("@Description", workflow.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Application_Id", workflow.Application_Id);
            cmd.Parameters.AddWithValue("@Module_Id", workflow.Module_Id);
            cmd.Parameters.AddWithValue("@StartingPage_Id", workflow.StartingPage_Id);
            cmd.Parameters.AddWithValue("@User_Id", workflow.User_Id);
            cmd.Parameters.Add("@Status", SqlDbType.Bit).Value = workflow.Status;

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // DeleteWorkflow using stored procedure
        public async Task DeleteWorkflowAsync(Guid workflowId, Guid updatedBy)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Sp_DeleteWorkflowWithTimestamp", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Workflow_Id", workflowId);
                    command.Parameters.AddWithValue("@User_Id", updatedBy);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
