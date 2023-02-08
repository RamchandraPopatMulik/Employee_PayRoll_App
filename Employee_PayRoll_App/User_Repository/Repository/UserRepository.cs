using Employee_Payroll_Model;
using Employee_Payroll_Repository.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Employee_Payroll_Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration config;
        private string connectionString;
        public UserRepository(IConfiguration configuration, IConfiguration config)
        {
            connectionString = configuration.GetConnectionString("UserDBConnection");
            this.config = config;
        }
        public static string EncryptPassword(string password)
        {
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(encode);
        }
        public string GenerateJWTToken(string emailID, int UserID)
        {
            try
            {
                var loginSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.config[("Jwt:key")]));
                var loginTokenDescripter = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Role,"User"),
                        new Claim(ClaimTypes.Email, emailID),
                        new Claim("UserID",UserID.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(5),
                    SigningCredentials = new SigningCredentials(loginSecurityKey, SecurityAlgorithms.HmacSha256Signature)
                };
                var token = new JwtSecurityTokenHandler().CreateToken(loginTokenDescripter);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        public UserModel Register(UserModel userModel)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    SqlCommand command = new SqlCommand("SPRegisterUser", connection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FullName", userModel.FullName);
                    command.Parameters.AddWithValue("@EmailID", userModel.EmailID);
                    command.Parameters.AddWithValue("@MobileNumber", userModel.MobileNumber);
                    command.Parameters.AddWithValue("@Password", EncryptPassword(userModel.Password));

                    connection.Open();
                    int SignOrNot = command.ExecuteNonQuery();

                    if (SignOrNot >= 1)
                    {
                        return userModel;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        public string Login(string EmailID, string Password)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                int UserID = 0;
                using (connection)
                {
                    SqlCommand command = new SqlCommand("SPLoginUser", connection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmailID", EmailID);
                    command.Parameters.AddWithValue("@Password", EncryptPassword(Password));

                    connection.Open();
                    SqlDataReader Reader = command.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            UserID = Reader.IsDBNull("UserID") ? 0 : Reader.GetInt32("UserID");
                        }
                        string token = GenerateJWTToken(EmailID, UserID);
                        return token;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}
