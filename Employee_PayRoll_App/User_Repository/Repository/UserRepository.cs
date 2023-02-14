﻿using Employee_Payroll_Model;
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
        public User_Ticket CreateTicketForPassword(string emailID, string token)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                User_Ticket ticket = new User_Ticket();
                using (connection)
                {
                    SqlCommand command = new SqlCommand("SPForgotPassword", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmailID", emailID);

                    connection.Open();
                    SqlDataReader Reader = command.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            ticket.FullName = Reader.IsDBNull("FullName") ? string.Empty : Reader.GetString("FullName");
                            ticket.EmailId = Reader.IsDBNull("EmailID") ? string.Empty : Reader.GetString("EmailID");
                            ticket.Token = token;
                            ticket.IssueAt = DateTime.Now;

                        }
                        return ticket;
                    }
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string ForgotPassword(string emailID)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                UserModel userModel = new UserModel();

                using (connection)
                {
                    SqlCommand command = new SqlCommand("SPForgotPassword", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmailID", emailID);

                    connection.Open();
                    SqlDataReader Reader = command.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            userModel.UserID = Reader.IsDBNull("UserID") ? 0 : Reader.GetInt32("UserID");
                            userModel.FullName = Reader.IsDBNull("FullName") ? string.Empty : Reader.GetString("FullName");
                        }
                        string token = GenerateJWTToken(emailID, userModel.UserID);
                        //MSMQModel mSMQModel = new MSMQModel();
                        //mSMQModel.SendMessage(token, emailID, userSignUp.FullName);
                        return token.ToString();
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
        public bool ResetPassword(string Password, string emailID)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    SqlCommand command = new SqlCommand("SPResetPassword", connection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmailID", emailID);
                    command.Parameters.AddWithValue("@Password", EncryptPassword(Password));
                    connection.Open();
                    int resetOrNot = command.ExecuteNonQuery();

                    if (resetOrNot >= 1)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<UserModel> GetAllUser()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                List<UserModel> getUser = new List<UserModel>();
                using (connection)
                {
                    SqlCommand command = new SqlCommand("SPGetAllUser", connection);

                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    SqlDataReader Reader = command.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            UserModel user = new UserModel()
                            {
                                UserID = Reader.IsDBNull("UserID") ? 0 : Reader.GetInt32("UserID"),
                                FullName = Reader.IsDBNull("FullName") ? string.Empty : Reader.GetString("FullName"),
                                EmailID = Reader.IsDBNull("EmailID") ? string.Empty : Reader.GetString("EmailID"),
                                Password = Reader.IsDBNull("Password") ? string.Empty : Reader.GetString("Password"),
                                MobileNumber = Reader.IsDBNull("MobileNumber") ? 0 : Reader.GetInt64("MobileNumber"),
                               
                            };
                            getUser.Add(user);
                        }
                        return getUser;
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
