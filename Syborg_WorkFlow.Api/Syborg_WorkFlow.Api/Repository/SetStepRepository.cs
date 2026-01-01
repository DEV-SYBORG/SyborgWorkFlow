using Microsoft.Data.SqlClient;
using Syborg_WorkFlow.Api.Interface;
using Syborg_WorkFlow.Api.Model;
using System.Data;

namespace Syborg_WorkFlow.Api.Repository
{

    public class SetStepRepository : ISetStepRepository
    {
        private readonly string _connectionString;

        public SetStepRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("WorkflowDB");
        }

        public async Task CreateSetStepAsync(SetStep step)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_InsertSetStepWithTimestamp", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@WorkflowName_Id", step.WorkflowName_Id);
            cmd.Parameters.AddWithValue("@WorkflowStep_Id", step.WorkflowStep_Id);
            cmd.Parameters.AddWithValue("@Is_Conditional", step.Is_Conditional);
            cmd.Parameters.AddWithValue("@NextStep_Yes", step.NextStep_Yes);

            // If Is_Conditional = Yes → take NextStep_No  
            // If No → set DB NULL
            if (step.Is_Conditional == "Yes")
                cmd.Parameters.AddWithValue("@NextStep_No", (object)step.NextStep_No ?? DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@NextStep_No", DBNull.Value);

            cmd.Parameters.AddWithValue("@User_Id", step.User_Id);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<GetAllSetStep>> GetAllSetStepsAsync()
        {
            var setSteps = new List<GetAllSetStep>();
            using var con = new SqlConnection(_connectionString);

            using var cmd = new SqlCommand("Sp_GetAllSetSteps", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var step = new GetAllSetStep
                {
                    SetStep_Id = reader.GetGuid(reader.GetOrdinal("SetStep_Id")),
                    WorkflowName_Id = reader.GetGuid(reader.GetOrdinal("WorkflowName_Id")),
                    WorkflowStep_Id = reader.GetGuid(reader.GetOrdinal("WorkflowStep_Id")),
                    Is_Conditional = reader.GetString(reader.GetOrdinal("Is_Conditional")),
                    NextStep_Yes = reader.GetGuid(reader.GetOrdinal("NextStep_Yes")),
                    NextStep_No = reader.IsDBNull(reader.GetOrdinal("NextStep_No")) ? null : reader.GetGuid(reader.GetOrdinal("NextStep_No")),
                    TimeStamp_Id = reader.GetGuid(reader.GetOrdinal("TimeStamp_Id")),
                    Created_By = reader.GetGuid(reader.GetOrdinal("Created_By")),
                    Created_At = reader.GetDateTime(reader.GetOrdinal("Created_At")),
                    Updated_By = reader["Updated_By"] == DBNull.Value ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("Updated_By")),
                    Updated_At = reader["Updated_At"] == DBNull.Value ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Updated_At")),
                    Old_Data = reader["Old_Data"] == DBNull.Value ? null : reader["Old_Data"].ToString(),
                    Updated_Data = reader["Updated_Data"] == DBNull.Value ? null : reader["Updated_Data"].ToString(),
                    Update_Status = reader["Update_Status"] == DBNull.Value ? null : reader["Update_Status"].ToString()
                };
                setSteps.Add(step);
            }
            return setSteps;
        }

        //public async Task<List<SetStepList>> GetSetStepListAsync()
        //{
        //    var steps = new List<SetStepList>();

        //    using var conn = new SqlConnection(_connectionString);
        //    using var cmd = new SqlCommand("Sp_GetWorkflowStepList", conn) // SetStepList SP Left
        //    {
        //        CommandType = CommandType.StoredProcedure
        //    };

        //    await conn.OpenAsync();
        //    using var reader = await cmd.ExecuteReaderAsync();

        //    while (await reader.ReadAsync())
        //    {
        //        steps.Add(new SetStepList
        //        {
        //            SetStep_Id = reader.GetGuid(reader.GetOrdinal("Id")),
        //            SetStepName = reader["StepName"]?.ToString()
        //        });
        //    }

        //    return steps;
        //}

        public async Task<GetAllSetStep?> GetSetStepByIdAsync(Guid setStepId)
        {
            GetAllSetStep? step = null;

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_GetSetStepById", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@SetStep_Id", setStepId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                step = new GetAllSetStep
                {
                    SetStep_Id = reader.GetGuid(reader.GetOrdinal("SetStep_Id")),
                    WorkflowName_Id = reader.GetGuid(reader.GetOrdinal("WorkflowName_Id")),
                    WorkflowStep_Id = reader.GetGuid(reader.GetOrdinal("WorkflowStep_Id")),
                    Is_Conditional = reader.GetString(reader.GetOrdinal("Is_Conditional")),
                    NextStep_Yes = reader.GetGuid(reader.GetOrdinal("NextStep_Yes")),
                    NextStep_No = reader.IsDBNull(reader.GetOrdinal("NextStep_No")) ? null : reader.GetGuid(reader.GetOrdinal("NextStep_No")),
                    TimeStamp_Id = reader.GetGuid(reader.GetOrdinal("TimeStamp_Id")),
                    Created_By = reader.GetGuid(reader.GetOrdinal("Created_By")),
                    Created_At = reader.GetDateTime(reader.GetOrdinal("Created_At")),
                    Updated_By = reader["Updated_By"] == DBNull.Value ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("Updated_By")),
                    Updated_At = reader["Updated_At"] == DBNull.Value ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Updated_At")),
                    Old_Data = reader["Old_Data"] == DBNull.Value ? null : reader["Old_Data"].ToString(),
                    Updated_Data = reader["Updated_Data"] == DBNull.Value ? null : reader["Updated_Data"].ToString(),
                    Update_Status = reader["Update_Status"] == DBNull.Value ? null : reader["Update_Status"].ToString()
                };
            }
            return step;
        }

        public async Task<bool>IsSetStepExistsAsync(Guid SetStepId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_CheckSetStepExists", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@SetStep_Id", SetStepId);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result) == 1; // true = exists & not deleted
        }

        public async Task UpdateSetStepAsync(SetStep step)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Sp_UpdateSetStepWithTimestamp", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@SetStep_Id", step.SetStep_Id);
            cmd.Parameters.AddWithValue("@WorkflowName_Id", step.WorkflowName_Id);
            cmd.Parameters.AddWithValue("@WorkflowStep_Id", step.WorkflowStep_Id);
            cmd.Parameters.AddWithValue("@Is_Conditional", step.Is_Conditional);
            cmd.Parameters.AddWithValue("@NextStep_Yes", step.NextStep_Yes);

            // If Is_Conditional = Yes → take NextStep_No
            if (step.Is_Conditional == "Yes")
                cmd.Parameters.AddWithValue("@NextStep_No", (object)step.NextStep_No ?? DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@NextStep_No", DBNull.Value);

            cmd.Parameters.AddWithValue("@User_Id", step.User_Id);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

        }

        public async Task DeleteSetStepAsync(Guid setStepId, Guid updatedBy)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Sp_DeleteSetStep", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@SetStep_Id", setStepId);
                    command.Parameters.AddWithValue("@Updated_By", updatedBy);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
