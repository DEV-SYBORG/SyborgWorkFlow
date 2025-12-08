using Microsoft.Data.SqlClient;
using Syborg_WorkFlow.Api.Model;
using Syborg_WorkFlow.Api.Model.Syborg_WorkFlow.Api.Model;
using System.Data;

namespace Syborg_WorkFlow.Api.Repositories
{
    public class WorkflowStepRepository : IWorkflowStepRepository
    {
        private readonly string _connectionString;
        public WorkflowStepRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("WorkflowDB");
        }

        public async Task CreateWorkflowStepAsync(WorkflowStep step)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_InsertWorkflowStepWithTimestamp", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            
            cmd.Parameters.AddWithValue("@WorkflowName_Id", step.WorkflowName_Id);
            cmd.Parameters.AddWithValue("@StepName", step.StepName);
            cmd.Parameters.AddWithValue("@Sequence", step.Sequence);
            cmd.Parameters.AddWithValue("@Module_Id", step.Module_Id);
            cmd.Parameters.AddWithValue("@ApplicationPage_Id", step.ApplicationPage_Id);
            cmd.Parameters.AddWithValue("@Section_Id", step.Section_Id);
            cmd.Parameters.AddWithValue("@Role_Id", step.Role_Id);

            cmd.Parameters.AddWithValue("@User_Id", step.User_Id);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<GetAllWorkflowStep>> GetAllWorkflowStepsAsync()
        {
            var steps = new List<GetAllWorkflowStep>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_GetAllWorkflowSteps", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                steps.Add(new GetAllWorkflowStep
                {
                    WorkflowStep_Id = reader.GetGuid(reader.GetOrdinal("WorkflowStep_Id")),
                    WorkflowName_Id = reader.GetGuid(reader.GetOrdinal("WorkflowName_Id")),
                    StepName = reader["StepName"]?.ToString(),
                    Sequence = reader.GetInt32(reader.GetOrdinal("Sequence")),
                    Module_Id = reader.GetGuid(reader.GetOrdinal("Module_Id")),
                    ApplicationPage_Id = reader.GetGuid(reader.GetOrdinal("ApplicationPage_Id")),
                    Section_Id = reader.GetGuid(reader.GetOrdinal("Section_Id")),
                    Role_Id = reader.GetGuid(reader.GetOrdinal("Role_Id")),
                    TimeStamp_Id = reader.GetGuid(reader.GetOrdinal("TimeStamp_Id")),

                    Created_By = reader.GetGuid(reader.GetOrdinal("Created_By")),
                    Created_At = reader.GetDateTime(reader.GetOrdinal("Created_At")),
                    Updated_By = reader["Updated_By"] == DBNull.Value ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("Updated_By")),
                    Updated_At = reader["Updated_At"] == DBNull.Value ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Updated_At")),
                    Old_Data = reader["Old_Data"]?.ToString(),
                    Updated_Data = reader["Updated_Data"]?.ToString(),
                    Update_Status = reader["Update_Status"]?.ToString()
                });
            }

            return steps;
        }

        public async Task<List<WorkflowStepListDto>> GetWorkflowStepListAsync()
        {
            var steps = new List<WorkflowStepListDto>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_GetWorkflowStepList", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                steps.Add(new WorkflowStepListDto
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    StepName = reader["StepName"]?.ToString()
                });
            }

            return steps;
        }

        public async Task<GetAllWorkflowStep?> GetWorkflowStepByIdAsync(Guid workflowStepId)
        {
            GetAllWorkflowStep? step = null;

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_GetWorkflowStepById", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@WorkflowStep_Id", workflowStepId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                step = new GetAllWorkflowStep
                {
                    WorkflowStep_Id = reader.GetGuid(reader.GetOrdinal("WorkflowStep_Id")),
                    WorkflowName_Id = reader.GetGuid(reader.GetOrdinal("WorkflowName_Id")),
                    StepName = reader["StepName"]?.ToString(),
                    Sequence = reader.GetInt32(reader.GetOrdinal("Sequence")),
                    Module_Id = reader.GetGuid(reader.GetOrdinal("Module_Id")),
                    ApplicationPage_Id = reader.GetGuid(reader.GetOrdinal("ApplicationPage_Id")),
                    Section_Id = reader.GetGuid(reader.GetOrdinal("Section_Id")),
                    Role_Id = reader.GetGuid(reader.GetOrdinal("Role_Id")),
                    TimeStamp_Id = reader.GetGuid(reader.GetOrdinal("TimeStamp_Id")),

                    Created_By = reader.GetGuid(reader.GetOrdinal("Created_By")),
                    Created_At = reader.GetDateTime(reader.GetOrdinal("Created_At")),
                    Updated_By = reader["Updated_By"] == DBNull.Value ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("Updated_By")),
                    Updated_At = reader["Updated_At"] == DBNull.Value ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Updated_At")),
                    Old_Data = reader["Old_Data"]?.ToString(),
                    Updated_Data = reader["Updated_Data"]?.ToString(),
                    Update_Status = reader["Update_Status"]?.ToString()
                };
            }

            return step;
        }



        public async Task<bool> IsWorkflowStepExistsAsync(Guid workflowStepId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_CheckWorkflowStepExists", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@WorkflowStep_Id", workflowStepId);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result) == 1; // true = exists & not deleted
        }

        public async Task UpdateWorkflowStepAsync(WorkflowStep step)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_UpdateWorkflowStepWithTimestamp", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@WorkflowStep_Id", step.WorkflowStep_Id);
            cmd.Parameters.AddWithValue("@WorkflowName_Id", step.WorkflowName_Id);
            cmd.Parameters.AddWithValue("@StepName", step.StepName);
            cmd.Parameters.AddWithValue("@Sequence", step.Sequence);
            
            cmd.Parameters.AddWithValue("@Module_Id", step.Module_Id);
            cmd.Parameters.AddWithValue("@ApplicationPage_Id", step.ApplicationPage_Id);
            cmd.Parameters.AddWithValue("@Section_Id", step.Section_Id);
            cmd.Parameters.AddWithValue("@Role_Id", step.Role_Id);
            cmd.Parameters.AddWithValue("@User_Id", step.User_Id);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }


        public async Task DeleteWorkflowStepAsync(Guid workflowStepId, Guid updatedBy)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Sp_DeleteWorkflowStep", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@WorkflowStep_Id", workflowStepId);
                    command.Parameters.AddWithValue("@Updated_By", updatedBy);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
