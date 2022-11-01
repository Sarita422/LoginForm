using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class LoginModelManager
    {
        public LoginModel UserAuth(LoginModel loginModel)
        {
            string scn = ConfigurationManager.ConnectionStrings["scn"].ConnectionString;
            using(SqlConnection cn=new SqlConnection(scn))
            {
                using(SqlCommand cmd = new SqlCommand("Sp_VerifyUserinfo", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserEmail",System.Data.SqlDbType.VarChar, 40).Value = loginModel.UserEmail;
                    cmd.Parameters.Add("@UserPassword",System.Data.SqlDbType.VarChar, 40).Value = loginModel.UserPassword;

                    try
                    {
                        cn.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            if(dr["UserEmail"]!=DBNull.Value&& dr["Role"] != DBNull.Value)
                            {
                                loginModel.Isvalid = 1;
                                loginModel.UserEmail = Convert.ToString(dr["UserEmail"]);
                                loginModel.UserRole = Convert.ToString(dr["Role"]);
                                loginModel.Username = Convert.ToString(dr["Username"]);
                            }
                            else
                            {
                                loginModel.Isvalid = 0;
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (cn.State == System.Data.ConnectionState.Open)
                        {
                            cn.Close();
                        }
                    }
                    return loginModel;
                }
            }
        }
    }
}