using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;
using Antlr.Runtime;

namespace BuildApp.Models.DAL
{
    public class DBservices
    {
        public SqlDataAdapter da;
        public DataTable dt;

        public DBservices()
        {
        }
        private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

            return cmd;
        }
        public SqlConnection connect(String conString)
        {
            string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }
        public void update()
        {
            da.Update(dt);
        }

        //Login
        public bool IsUserNameExist(string un)
        {
            SqlConnection con = null;
            bool isEx = false;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM BuildApp_Users";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    if (((string)dr["UserName"]).ToLower() == un.ToLower())
                    {
                        isEx = true;
                    }
                }

                return isEx;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public User IsSignIn(string userName, string password)
        {
            SqlConnection con = null;
            bool isEx = false;
            User u = null;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM BuildApp_Users";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    if (((string)dr["UserName"]).ToLower() == userName.ToLower() && ((string)dr["Password"]).ToLower() == password.ToLower())
                    {
                        isEx = true;
                        u = new User((string)dr["UserName"], (string)dr["FirstName"], (string)dr["LastName"], Convert.ToDateTime(dr["Birthday"]), (string)dr["Gender"], (string)dr["Password"], (string)dr["PicUrl"]);
                    }
                }

                return isEx ? u : null;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }
        public string AddUser(User u)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string cStr = $"INSERT INTO BuildApp_Users ([UserName],[FirstName],[LastName],[Birthday],[Gender],[Password],[PicUrl]) VALUES('{u.UserName}', '{u.FirstName}', '{u.LastName}', '{u.Birthday.ToString("yyyy-MM-ddTHH:mm:ss")}', '{u.Gender}', '{u.Password}', '{u.PicUrl}')";

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                cmd.ExecuteNonQuery(); // execute the command
                return "ok";
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }


        }

        //Address
        public int AddAddress(Address a)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string cStr = $"INSERT INTO BuildApp_Address ([PlaceId],[Country],[Locality],[Street],[FloorsNum],[AddedUser],[HouseNum]) VALUES('{a.PlaceId}', '{a.Country}', '{a.Locality}', '{a.Street}', '{a.FloorsNum}', '{a.AddedUser}', '{a.HouseNum}')";

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                cmd.ExecuteNonQuery(); // execute the command
                return GetAddressIdByPlaceId(a);
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }


        }
        public int GetAddressIdByPlaceId(Address address)
        {
            SqlConnection con = null;
            int addressId = 0;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM BuildApp_Address";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    if (((string)dr["PlaceId"]) == address.PlaceId)
                    {
                        addressId = (int)dr["AddressId"];
                    }
                }

                return addressId;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }


        }
        public string IsPlaceIdExist(string placeId)
        {
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM BuildApp_Address";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    if (((string)dr["PlaceId"]).ToLower() == placeId.ToLower())
                    {
                        return "B" + (10000 - Convert.ToInt32(dr["AddressId"]));
                    }
                }

                return "false";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public bool IsAddressIdExist(string buildingCode)
        {
            SqlConnection con = null;
            bool isEx = false;
            int addressId = 0;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM BuildApp_Address";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                addressId = 10000 - int.Parse(buildingCode.Substring(1));
                while (dr.Read())
                {
                    if (((int)dr["AddressId"]) == addressId)
                    {
                        isEx = true;
                    }
                }

                return isEx;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public int GetAddressIdByUN(string un)
        {
            SqlConnection con = null;
            int addressId = 0;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = @"select [AddressId]
                                     from [BuildApp_UserInAddress]
                                     where [UserName]='" + un + "'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    addressId = (int)dr["AddressId"];
                }

                return addressId;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        //User In Address
        public bool IsUserInAddressExist(string userName)
        {
            SqlConnection con = null;
            bool isEx = false;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM BuildApp_UserInAddress";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    if (((string)dr["UserName"]) == userName)
                    {
                        isEx = true;
                    }
                }

                return isEx;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public string AddUserInAddress(UserInAddress u)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string cStr = $"INSERT INTO BuildApp_UserInAddress ([UserName],[AddressId],[Floor],[Apartment],[PhoneNum]) VALUES('{u.UserName}', '{u.AddressId}', '{u.Floor}', '{u.Apartment}', '{u.PhoneNum}')";

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                cmd.ExecuteNonQuery(); // execute the command
                return "ok";
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }


        }
        public string UpdateUserInAddress(UserInAddress u)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string cStr = $"UPDATE BuildApp_UserInAddress SET AddressId={u.AddressId} ,Floor={u.Floor} ,Apartment={u.Apartment} ,PhoneNum='{u.PhoneNum}' WHERE UserName='{u.UserName}'";

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                cmd.ExecuteNonQuery(); // execute the command
                return "ok";
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }


        }
        public int GetAddressFloor(int addressId)
        {
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString");
                int floorsNum = 0;
                String selectSTR = $"SELECT * FROM [bgroup9_test1].[dbo].[BuildApp_Address] where AddressId='{addressId}'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    if (addressId == (int)dr["AddressId"])
                    {
                        floorsNum = (int)dr["FloorsNum"];
                    }
                }


                return floorsNum;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }



        }
        public string UpdateAddressFloor(int addressId, int FloorsNum)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string cStr = $"UPDATE [bgroup9_test1].[dbo].[BuildApp_Address] SET FloorsNum='{FloorsNum}' WHERE AddressId='{addressId}'";

            try
            {
                cmd = CreateCommand(cStr, con);             // create the command
                cmd.ExecuteNonQuery(); // execute the command

                return "ok";
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }

            }
        }
        public List<string> GetUserNamesByAddressId(int addressId)
        {
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT * FROM BuildApp_UserInAddress UIA inner join BuildApp_Users U on UIA.UserName = U.UserName where AddressId = {addressId}";
                List<string> usersInAddress = new List<string>();
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    string userName = (string)dr["UserName"];
                    usersInAddress.Add(userName);
                }

                return usersInAddress;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }
        public List<string> GetFullNamesByAddressId(int addressId)
        {
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT * FROM BuildApp_UserInAddress UIA inner join BuildApp_Users U on UIA.UserName = U.UserName where AddressId = { addressId}";
                List<string> users = new List<string>();
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    string fullName = (string)dr["FirstName"] + " " + (string)dr["LastName"];
                    users.Add(fullName);
                }

                return users;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        //Settings
        public bool IsUserSettingsExist(string userName)
        {
            SqlConnection con = null;
            bool isEx = false;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM BuildApp_UserSettings";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    if (((string)dr["UserName"]) == userName)
                    {
                        isEx = true;
                    }
                }

                return isEx;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public string AddUserSettings(Settings s)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string cStr = $"INSERT INTO BuildApp_UserSettings ([UserName],[Babysitter],[Dogwalker],[Carpool],[Groceries],[Availability]) VALUES('{s.UserName}', '{s.BabySitter}', '{s.Dogwalker}', '{s.Carpool}', '{s.Groceries}', '{s.Availability}')";

            try
            {
                cmd = CreateCommand(cStr, con);             // create the command
                cmd.ExecuteNonQuery(); // execute the command
                AddUserSkill(s.UserName, s.Skills);
                return "ok";
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }

            }


        }
        public string UpdateUserSettings(Settings s)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string cStr = $"UPDATE BuildApp_UserSettings SET Babysitter='{s.BabySitter}',Dogwalker='{s.Dogwalker}',Carpool='{s.Carpool}',Groceries='{s.Groceries}',Availability='{s.Availability}'  WHERE UserName='{s.UserName}'";

            try
            {
                cmd = CreateCommand(cStr, con);             // create the command
                cmd.ExecuteNonQuery(); // execute the command
                UpdateUserSkills(s.UserName, s.Skills);
                return "ok";
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }

            }


        }
        public bool GetSettingByUNAndRequestType(string userName, string type)
        {
            SqlConnection con = null;
            bool readyToDo = false;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT {type} FROM BuildApp_UserSettings where UserName='{userName}'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    readyToDo = (bool)dr[$"{type}"];
                }
                return readyToDo;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        //Skill
        public string AddUserSkill(string userName, List<Skills> s)
        {
            SqlConnection con;
            SqlCommand cmd;


            string cStr = "";

            foreach (var skill in s)
            {
                cStr += $"INSERT INTO BuildApp_UserSkills ([UserName],[SkillNum]) VALUES('{userName}', '{skill.SkillNum}') ";

            }


            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            try
            {
                cmd = CreateCommand(cStr, con);             // create the command
                cmd.ExecuteNonQuery(); // execute the command

                return "ok";
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }





        }
        public string UpdateUserSkills(string userName, List<Skills> s)
        {
            SqlConnection con;
            SqlCommand cmd;


            string cStr = "";
            cStr += $"DELETE FROM BuildApp_UserSkills WHERE UserName='{userName}' ";
            foreach (var skill in s)
            {
                cStr += $"INSERT INTO BuildApp_UserSkills ([UserName],[SkillNum]) VALUES('{userName}', '{skill.SkillNum}') ";

            }


            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            try
            {
                cmd = CreateCommand(cStr, con);             // create the command
                cmd.ExecuteNonQuery(); // execute the command

                return "ok";
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }





        }
        public List<Skills> GetSkills()
        {
            List<Skills> skills = new List<Skills>();
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM BuildApp_Skills";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    Skills s = new Skills();
                    s.SkillNum = (int)dr["SkillNum"];
                    s.SkillName = (string)dr["Skillname"];



                    skills.Add(s);
                }

                return skills;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public int GetSkillNumBySkillName(string skillName)
        {
            SqlConnection con = null;
            int SkillNum = 0;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT SkillNum FROM BuildApp_Skills where SkillName='{skillName}'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    SkillNum = (int)dr["SkillNum"];
                }

                return SkillNum;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public List<UserSkills> GetkUserSkillsByUserName(string un)
        {
            SqlConnection con = null;
            List<UserSkills> usL = new List<UserSkills>();
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT * FROM BuildApp_UserSkills where UserName='{un}'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    UserSkills us = new UserSkills();
                    us.UserName = (string)dr["UserName"];
                    us.SkillNum = (int)dr["SkillNum"];

                    usL.Add(us);
                }

                return usL;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        //Request
        public List<Request> GetAllRequests()
        {
            SqlConnection con = null;
            List<Request> requests = new List<Request>();
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT * FROM BuildApp_Request order by DueDate";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    Request req = new Request();
                    req.SerialNum = (int)dr["SerialNum"];
                    req.AddressId = (int)dr["AddressId"];
                    req.FromUserName = (string)dr["FromUserName"];
                    req.Type = (string)dr["Type"];
                    req.DueDate = Convert.ToDateTime(dr["DueDate"]);
                    req.IsItPaid = (bool)dr["IsItPaid"];
                    req.Note = (string)dr["Note"];
                    req.ExecutingUser = (string)dr["ExecutingUser"];
                    req.IsActive = (bool)dr["IsActive"];
                    req.RequestLong = (double)dr["Long"];
                    if (req.Type == "skill")
                    {
                        req.SkillNum = (int)dr["SkillNum"];
                    }

                    requests.Add(req);
                }

                return requests;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public List<Request> GetAllRequestsByUN(string userName)
        {
            SqlConnection con = null;
            List<Request> requests = new List<Request>();
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT * FROM BuildApp_Request where FromUserName='{userName}' order by DueDate";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    Request req = new Request();
                    req.SerialNum = (int)dr["SerialNum"];
                    req.AddressId = (int)dr["AddressId"];
                    req.FromUserName = (string)dr["FromUserName"];
                    req.Type = (string)dr["Type"];
                    req.DueDate = Convert.ToDateTime(dr["DueDate"]);
                    req.IsItPaid = (bool)dr["IsItPaid"];
                    req.Note = (string)dr["Note"];
                    req.ExecutingUser = (string)dr["ExecutingUser"];
                    req.IsActive = (bool)dr["IsActive"];
                    req.RequestLong = (double)dr["Long"];
                    if (req.Type == "skill")
                    {
                        req.SkillNum = (int)dr["SkillNum"];
                    }

                    requests.Add(req);
                }

                return requests;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public List<Request> GetActiveRequestsByUserName(string userName)
        {
            SqlConnection con = null;
            List<Request> requests = new List<Request>();
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT * FROM BuildApp_Request where FromUserName='{userName}' and IsActive=1 order by DueDate";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    Request req = new Request();
                    req.SerialNum = (int)dr["SerialNum"];
                    req.AddressId = (int)dr["AddressId"];
                    req.FromUserName = (string)dr["FromUserName"];
                    req.Type = (string)dr["Type"];
                    req.DueDate = Convert.ToDateTime(dr["DueDate"]);
                    req.IsItPaid = (bool)dr["IsItPaid"];
                    req.Note = (string)dr["Note"];
                    req.ExecutingUser = (string)dr["ExecutingUser"];
                    req.IsActive = (bool)dr["IsActive"];
                    req.RequestLong = (double)dr["Long"];
                    if (req.Type == "skill")
                    {
                        req.SkillNum = (int)dr["SkillNum"];
                    }


                    requests.Add(req);

                }

                return requests;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public List<Request> GetAllActiveRequest()
        {
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT * FROM [dbo].[BuildApp_Request] where IsActive=1";

                SqlCommand cmd = new SqlCommand(selectSTR, con);
                List<Request> activeRequests = new List<Request>();
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    Request r = new Request();
                    r.SerialNum = (int)dr["SerialNum"];
                    r.AddressId = (int)dr["AddressId"];
                    r.FromUserName = (string)dr["FromUserName"];
                    r.Type = (string)dr["Type"];
                    r.DueDate = Convert.ToDateTime(dr["DueDate"]);
                    r.IsItPaid = (bool)dr["IsItPaid"];
                    r.Note = (string)dr["Note"];
                    r.ExecutingUser = (string)dr["ExecutingUser"];
                    r.IsActive = (bool)dr["IsActive"];
                    r.RequestLong = (double)dr["Long"];
                    if (r.Type == "skill")
                    {
                        r.SkillNum = (int)dr["SkillNum"];
                    }

                    activeRequests.Add(r);
                }

                return activeRequests;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public List<Request> GetTasksByUN(string userName)
        {
            SqlConnection con = null;
            List<Request> requests = new List<Request>();
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT * FROM BuildApp_Request where ExecutingUser='{userName}' order by DueDate";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    Request req = new Request();
                    req.SerialNum = (int)dr["SerialNum"];
                    req.AddressId = (int)dr["AddressId"];
                    req.FromUserName = (string)dr["FromUserName"];
                    req.Type = (string)dr["Type"];
                    req.DueDate = Convert.ToDateTime(dr["DueDate"]);
                    req.IsItPaid = (bool)dr["IsItPaid"];
                    req.Note = (string)dr["Note"];
                    req.ExecutingUser = (string)dr["ExecutingUser"];
                    req.IsActive = (bool)dr["IsActive"];
                    req.RequestLong = (double)dr["Long"];
                    if (req.Type == "skill")
                    {
                        req.SkillNum = (int)dr["SkillNum"];
                    }

                    requests.Add(req);
                }

                return requests;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public string UpdateExecutingUser(int serialNum, string userName)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string cStr = $"UPDATE BuildApp_Request SET [ExecutingUser]='{userName}',[IsActive]='false'  WHERE [SerialNum]={serialNum}";


            try
            {
                cmd = CreateCommand(cStr, con);             // create the command
                cmd.ExecuteNonQuery(); // execute the command
                DeleteStatuses(serialNum);
                return "ok";
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }

            }

        }
        public string AddRequest(RequestToPush rtp)
        {
            SqlConnection con;
            SqlCommand cmd;
            string cStr = "";

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            if (rtp.Skill is null)
            {
                cStr = $"INSERT INTO BuildApp_Request ([AddressId],[FromUserName],[Type],[DueDate],[IsItPaid],[Note],[ExecutingUser],[IsActive],[Long]) VALUES({GetAddressIdByUN(rtp.FromUserName)}, '{rtp.FromUserName}', '{rtp.Type}',' {rtp.DueDate.ToString("yyyy-MM-ddTHH:mm:ss")}', '{rtp.IsItPaid}', '{rtp.Note}','', 1, {rtp.RequestLong})";
            }
            else
            {
                cStr = $"INSERT INTO BuildApp_Request ([AddressId],[FromUserName],[Type],[DueDate],[IsItPaid],[Note],[ExecutingUser],[IsActive],[Long],[SkillNum]) VALUES({GetAddressIdByUN(rtp.FromUserName)}, '{rtp.FromUserName}', '{rtp.Type}',' {rtp.DueDate.ToString("yyyy-MM-ddTHH:mm:ss")}', '{rtp.IsItPaid}', '{rtp.Note}','', 1, {rtp.RequestLong},{GetSkillNumBySkillName(rtp.Skill)})";
            }


            try
            {
                cmd = CreateCommand(cStr, con);       // create the command
                cmd.ExecuteNonQuery();               // execute the command

                return InsertStatusesForRequest(GetTheLastRequest());

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }

            }
        }
        public Request GetTheLastRequest()
        {
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT TOP 1 * FROM [dbo].[BuildApp_Request] ORDER BY [SerialNum] DESC";

                SqlCommand cmd = new SqlCommand(selectSTR, con);
                Request r = new Request();
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {


                    r.SerialNum = (int)dr["SerialNum"];
                    r.AddressId = (int)dr["AddressId"];
                    r.FromUserName = (string)dr["FromUserName"];
                    r.Type = (string)dr["Type"];
                    r.DueDate = Convert.ToDateTime(dr["DueDate"]);
                    r.IsItPaid = (bool)dr["IsItPaid"];
                    r.Note = (string)dr["Note"];
                    r.ExecutingUser = (string)dr["ExecutingUser"];
                    r.IsActive = (bool)dr["IsActive"];
                    r.RequestLong = (double)dr["Long"];
                    if (r.Type == "skill")
                    {
                        r.SkillNum = (int)dr["SkillNum"];
                    }
                    else
                    {
                        continue;
                    }



                }

                return r;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public string InsertStatusesForRequest(Request r)
        {
            List<string> usersInAddress = GetUserNamesByAddressId(r.AddressId);

            for (int i = 0; i < usersInAddress.Count; i++)//ריצה על כל המשתמשים שגרים בבניין של פותח הבקשה
            {
                int status = 0;
                bool hasTheSkill = false;
                if (usersInAddress.ElementAt(i) == r.FromUserName)//על מנת לא להכניס רשומה של פותח הבקשה לטבלת סטטוס 
                {
                    continue;
                }
                else
                {
                    if (IsUserSettingsExist(usersInAddress.ElementAt(i)))//בדיקה אם המשתמש קיים בטבלת הגדרות משתמש
                    {
                        if (r.Type == "skill")//בדיקה אם זאת בקשה מסוג skill
                        {

                            for (int j = 0; j < GetkUserSkillsByUserName(usersInAddress.ElementAt(i)).Count; j++)//ריצה על כל הכישורים של אותו משתמש
                            {
                                if (r.SkillNum == GetkUserSkillsByUserName(usersInAddress.ElementAt(i)).ElementAt(j).SkillNum)//בדיקה  אם המשתמש בעל הכישור המתאים לבקשה
                                {
                                    hasTheSkill = true;
                                }
                            }
                            if (hasTheSkill == true)//בדיקה האם המשתמש מוכן לבצע בקשת skill מסוג זה
                            {
                                status = 2;
                                InsertStatus(r, usersInAddress.ElementAt(i), status);

                            }
                            else
                            {
                                InsertStatus(r, usersInAddress.ElementAt(i), status);

                            }

                        }
                        else if (GetSettingByUNAndRequestType(usersInAddress.ElementAt(i), r.Type))//בדיקה האם המשתמש מוכן לבצע בקשה מסוג זה
                        {
                            status = 2;
                            InsertStatus(r, usersInAddress.ElementAt(i), status);

                        }
                        else
                        {
                            InsertStatus(r, usersInAddress.ElementAt(i), status);

                        }
                    }

                }
            }

            return "ok";
        }
        public string UpdateIsActive(Request r)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string cStr = $"UPDATE BuildApp_Request SET [IsActive]='false'  WHERE [SerialNum]={r.SerialNum}";


            try
            {
                cmd = CreateCommand(cStr, con);             // create the command
                cmd.ExecuteNonQuery(); // execute the command
                DeleteStatuses(r.SerialNum);
                return "ok";
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }

            }
        }
        public Request GetRequestDetails(int serialNum)
        {
            SqlConnection con = null;
            Request req = new Request();
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT * FROM BuildApp_Request where SerialNum='{serialNum}'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    req = new Request();
                    req.SerialNum = (int)dr["SerialNum"];
                    req.AddressId = (int)dr["AddressId"];
                    req.FromUserName = (string)dr["FromUserName"];
                    req.Type = (string)dr["Type"];
                    req.DueDate = Convert.ToDateTime(dr["DueDate"]);
                    req.IsItPaid = (bool)dr["IsItPaid"];
                    req.Note = (string)dr["Note"];
                    req.ExecutingUser = (string)dr["ExecutingUser"];
                    req.IsActive = (bool)dr["IsActive"];
                    req.RequestLong = (double)dr["Long"];
                    if (req.Type == "skill")
                    {
                        req.SkillNum = (int)dr["SkillNum"];
                    }

                }

                return req;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }


        //Status
        public string InsertStatus(Request r, string un, int status)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string cStr = $"INSERT INTO BuildApp_Status ([RequestSerialNum],[ReceiverUsername],[AddressId],[RequestStatus]) VALUES({r.SerialNum}, '{un}', {r.AddressId}, {status})";

            try
            {
                cmd = CreateCommand(cStr, con);       // create the command
                cmd.ExecuteNonQuery();               // execute the command
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }

            }
            return "ok";
        }
        public List<List<List<Status>>> GetRequestBuildingByRequestId(int requestId)
        {  //תחזיר לפי מספר בקשה את הסטטוס של כל דייר בבניין לגבי אותה בקשה מסודרים לפי קומה ודירה
            SqlConnection con = null;

            List<Status> allStatuses = new List<Status>();

            int count = 0;
            int numOfFloors = 0;


            try
            {
                con = connect("DBConnectionString");
                String selectSTR = @"select BuildApp_Status.RequestSerialNum, BuildApp_Status.ReceiverUsername,BuildApp_Status.AddressId,
                                                BuildApp_Status.RequestStatus,BuildApp_Request.FromUserName, BuildApp_Request.Type, BuildApp_Request.DueDate,
                                                BuildApp_UserInAddress.Floor, BuildApp_UserInAddress.Apartment, BuildApp_Address.FloorsNum
                                         from BuildApp_Status inner join BuildApp_Request on BuildApp_Status.RequestSerialNum = BuildApp_Request.SerialNum
                                                              inner join BuildApp_UserInAddress on BuildApp_Status.ReceiverUsername = BuildApp_UserInAddress.UserName
                                                              inner join BuildApp_Address on BuildApp_Request.AddressId = BuildApp_Address.AddressId
                                         where BuildApp_Status.RequestSerialNum = '" + requestId + @"'
                                         order by BuildApp_UserInAddress.Floor, BuildApp_UserInAddress.Apartment";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    count++;

                    Status s = new Status();
                    s.RequestSerialNum = (int)dr["RequestSerialNum"];
                    s.ReceiverUserName = (string)dr["ReceiverUserName"];
                    s.AddressId = (int)dr["AddressId"];
                    s.RequestStatus = (int)dr["RequestStatus"];
                    allStatuses.Add(s);

                    numOfFloors = (int)dr["FloorsNum"];
                }

                Tuple<int, int, string>[] floorApartmentReceiverUN = new Tuple<int, int, string>[count];
                Tuple<int, int, string>[] fullFloorApartmentReceiverUN = GetFloorApartmentReceiverUN(floorApartmentReceiverUN, requestId);
                int maxApartInFloor = GetMaxApartInFloor(fullFloorApartmentReceiverUN);

                List<List<List<Status>>> matrixOfStatuses = new List<List<List<Status>>>();

                count = 0;

                for (int i = 1; i <= numOfFloors; i++)//מעבר על כל הקומות בבניין
                {
                    matrixOfStatuses.Add(SplitByFloor(fullFloorApartmentReceiverUN, allStatuses, i, numOfFloors, maxApartInFloor));
                }

                return matrixOfStatuses;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }
        public Tuple<int, int, string>[] GetFloorApartmentReceiverUN(Tuple<int, int, string>[] floorApartmentReceiverUN, int requestId)
        {
            SqlConnection con = null;
            int index = 0;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = @"select BuildApp_Status.RequestSerialNum, BuildApp_Status.ReceiverUsername,BuildApp_Status.AddressId,
                                                BuildApp_Status.RequestStatus,BuildApp_Request.FromUserName, BuildApp_Request.Type, BuildApp_Request.DueDate,
                                                BuildApp_UserInAddress.Floor, BuildApp_UserInAddress.Apartment, BuildApp_Address.FloorsNum
                                         from BuildApp_Status inner join BuildApp_Request on BuildApp_Status.RequestSerialNum = BuildApp_Request.SerialNum
                                                              inner join BuildApp_UserInAddress on BuildApp_Status.ReceiverUsername = BuildApp_UserInAddress.UserName
                                                              inner join BuildApp_Address on BuildApp_Request.AddressId = BuildApp_Address.AddressId
                                         where BuildApp_Status.RequestSerialNum = '" + requestId + @"'
                                         order by BuildApp_UserInAddress.Floor, BuildApp_UserInAddress.Apartment";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Tuple<int, int, string> temp = new Tuple<int, int, string>((int)dr["Floor"], (int)dr["Apartment"], (string)dr["ReceiverUsername"]);
                    floorApartmentReceiverUN[index] = temp;

                    index++;
                }
                return floorApartmentReceiverUN;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public int GetMaxApartInFloor(Tuple<int, int, string>[] fullFloorApartmentReceiverUN)
        {

            int maxApartInFloor = 0;
            int count = 1;

            for (int i = 0; i < fullFloorApartmentReceiverUN.Length - 1; i++)//ריצה על גודל המערך פחות אחד כדי למנוע חריגה בהמשך
            {
                if (i != 0 && fullFloorApartmentReceiverUN[i].Item1 > fullFloorApartmentReceiverUN[i - 1].Item1)//בדיקה מתי הקומה מתחלפת
                {
                    count = 1;
                }

                if (fullFloorApartmentReceiverUN[i].Item1 == fullFloorApartmentReceiverUN[i + 1].Item1)//בדיקת כל צמד דירות אם הן נמצאות באותה קומה
                {
                    count += fullFloorApartmentReceiverUN[i + 1].Item2 - fullFloorApartmentReceiverUN[i].Item2;

                }

                if (count > maxApartInFloor)
                {
                    maxApartInFloor = count;
                }
            }

            return maxApartInFloor;
        }
        public List<List<Status>> SplitByFloor(Tuple<int, int, string>[] fullFloorApartmentReceiverUN, List<Status> AllStatuses, int floor, int numOfFloors, int maxApartInFloor)
        {
            List<Status> statusesByFloor = new List<Status>();
            List<Status> statusesByApartment = new List<Status>();

            for (int i = 0; i < fullFloorApartmentReceiverUN.Length; i++)
            {
                if (fullFloorApartmentReceiverUN[i].Item1 == floor)
                {
                    statusesByFloor.Add(AllStatuses.ElementAt(i));//יצירת רשימה של כל הדירות בקומה
                }
            }

            return SplitByApartment(fullFloorApartmentReceiverUN, statusesByFloor, maxApartInFloor);
        }
        public List<List<Status>> SplitByApartment(Tuple<int, int, string>[] fullFloorApartmentReceiverUN, List<Status> statusesByFloor, int maxApartInFloor)
        {
            List<List<Status>> statusesByFloorByApart = new List<List<Status>>();
            int count = 0;
            if (statusesByFloor.Count != 0)
            {

                for (int j = 0; j < fullFloorApartmentReceiverUN.Length; j++)
                {
                    if (fullFloorApartmentReceiverUN[j].Item3 == statusesByFloor.ElementAt(0).ReceiverUserName)
                    {
                        for (int i = 0; i < statusesByFloor.Count; i++)
                        {

                            List<Status> statusByApartment = new List<Status>();
                            try
                            {
                                statusByApartment.Add(statusesByFloor.ElementAt(i));
                                count++;
                                if (count + (statusesByFloor.Count - i) < maxApartInFloor && fullFloorApartmentReceiverUN[j].Item1 == fullFloorApartmentReceiverUN[j + 1].Item1 && fullFloorApartmentReceiverUN[j].Item2 - fullFloorApartmentReceiverUN[j + 1].Item2 > 1)
                                {
                                    statusesByFloorByApart.Add(new List<Status>());
                                    count++;
                                }
                                while (fullFloorApartmentReceiverUN[j].Item1 == fullFloorApartmentReceiverUN[j + i + 1].Item1 && fullFloorApartmentReceiverUN[j].Item2 == fullFloorApartmentReceiverUN[j + i + 1].Item2)
                                {
                                    statusByApartment.Add(statusesByFloor.ElementAt(i + 1));
                                    i++;
                                }
                            }
                            catch (Exception)
                            {
                            }
                            statusesByFloorByApart.Add(statusByApartment);

                        }


                    }

                }
            }
            //הכנסת רשימות ריקות במידה ולא כל הדירות באותה קומה נרשמו למערכת
            if (count == 0)
            {
                for (int i = 0; i < maxApartInFloor; i++)
                {
                    List<Status> emptyStatusByApartment = new List<Status>();
                    statusesByFloorByApart.Add(emptyStatusByApartment);
                }
            }
            else if (count > 0 && count < maxApartInFloor)
            {
                for (int i = 0; i < maxApartInFloor - count; i++)
                {
                    Random rnd = new Random();
                    List<Status> emptyStatusByApartment = new List<Status>();
                    statusesByFloorByApart.Insert(rnd.Next(0, maxApartInFloor - 1), emptyStatusByApartment);
                }
            }

            return statusesByFloorByApart;
        }
        public string SawAllNotification(string un)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string cStr = $"update [dbo].[buildapp_status] set [requeststatus] = 1 where [receiverusername] = '{un}' and[requestserialnum] in  (select [requestserialnum] from [dbo].[buildapp_status] inner join [dbo].[buildapp_request] on[dbo].[buildapp_request].serialnum=[dbo].[buildapp_status].requestserialnum where[receiverusername] = '{un}' and [requeststatus]=2 and [dbo].[buildapp_request].isactive = 1)";


            try
            {
                cmd = CreateCommand(cStr, con);             // create the command
                cmd.ExecuteNonQuery(); // execute the command

                return "ok";
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        public int GetNumOfNotification(string un)
        {
            SqlConnection con = null;
            UserToShow uts = new UserToShow();
            int numOfNotification = 0;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"select COUNT([RequestSerialNum]) as NumOfNotification from[dbo].[BuildApp_Status] inner join[dbo].[BuildApp_Request] on[SerialNum]=[RequestSerialNum]  where[ReceiverUsername]='{un}' and [RequestStatus]=2 and[dbo].[BuildApp_Request].IsActive=1";


                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    numOfNotification = (int)dr["NumOfNotification"];
                }

                return numOfNotification;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public string AcceptRequest(string un, int serialNum)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string cStr = $"UPDATE BuildApp_Status SET [RequestStatus]=3  WHERE [RequestSerialNum]={serialNum} and ReceiverUserName='{un}'";
            try
            {
                cmd = CreateCommand(cStr, con);             // create the command
                cmd.ExecuteNonQuery(); // execute the command

                return "ok";
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }

            }

        }
        public string DeleteStatuses(int serialNum)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string cStr = $"delete from BuildApp_Status where RequestSerialNum={serialNum}";

            try
            {
                cmd = CreateCommand(cStr, con);       // create the command
                cmd.ExecuteNonQuery();               // execute the command
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }

            }
            return "ok";
        }

        //User
        public UserToShow GetUserToShow(string userName)
        {
            SqlConnection con = null;
            UserToShow uts = new UserToShow();
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT * FROM BuildApp_Users where UserName='{userName}'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    uts.UserName = (string)dr["UserName"];
                    uts.FirstName = (string)dr["FirstName"];
                    uts.LastName = (string)dr["LastName"];
                    uts.Birthday = Convert.ToDateTime(dr["Birthday"]);
                    uts.PicUrl = (string)dr["PicUrl"];
                    uts.Gender = (string)dr["Gender"];
                }

                return uts;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public UserDetails GetUserDetails(string userName)
        {
            SqlConnection con = null;
            UserDetails ud = new UserDetails();
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT * FROM BuildApp_Users U inner join BuildApp_UserInAddress UIA on  U.UserName=UIA.UserName where U.UserName='{userName}'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    ud.UserName = (string)dr["UserName"];
                    ud.FirstName = (string)dr["FirstName"];
                    ud.LastName = (string)dr["LastName"];
                    ud.Birthday = Convert.ToDateTime(dr["Birthday"]);
                    ud.PicUrl = (string)dr["PicUrl"];
                    ud.Gender = (string)dr["Gender"];
                    ud.PhoneNum = (string)dr["PhoneNum"];
                }

                return ud;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        //Notification
        public List<Notification> GetNotificationsByUN(string un)
        {
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT * FROM BuildApp_Request inner join BuildApp_Users on BuildApp_Request.FromUserName = BuildApp_Users.UserName where AddressId = (select AddressId from[dbo].[BuildApp_UserInAddress] where UserName = '{un}') and IsActive = 1 and FromUserName!='{un}' and GETDATE()<DueDate order by DueDate";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                List<Notification> notifications = new List<Notification>();
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    Notification n = new Notification();
                    Request r = new Request();
                    UserToShow u = new UserToShow();
                    n.Req = r;
                    n.Uts = u;
                    n.Req.SerialNum = (int)dr["SerialNum"];
                    n.Req.AddressId = (int)dr["AddressId"];
                    n.Req.FromUserName = (string)dr["FromUserName"];
                    n.Req.Type = (string)dr["Type"];
                    n.Req.DueDate = Convert.ToDateTime(dr["DueDate"]);
                    n.Req.IsItPaid = (bool)dr["IsItPaid"];
                    n.Req.Note = (string)dr["Note"];
                    n.Req.ExecutingUser = (string)dr["ExecutingUser"];
                    n.Req.IsActive = (bool)dr["IsActive"];
                    n.Req.RequestLong = (double)dr["Long"];
                    if (n.Req.Type == "skill")
                    {
                        n.Req.SkillNum = (int)dr["SkillNum"];
                    }
                    n.Uts.UserName = (string)dr["UserName"];
                    n.Uts.FirstName = (string)dr["FirstName"];
                    n.Uts.LastName = (string)dr["LastName"];
                    n.Uts.Gender = (string)dr["Gender"];
                    n.Uts.Birthday = Convert.ToDateTime(dr["Birthday"]);
                    n.Uts.PicUrl = (string)dr["PicUrl"];

                    notifications.Add(n);

                }

                return notifications;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public bool CheckIfNotificationIsRelevantForUN(Notification n, string un)
        {
            bool readyToDo = false;
            if (n.Req.Type == "skill")//בדיקה אם זאת בקשה מסוג skill
            {
                List<UserSkills> userSkills = new List<UserSkills>();
                userSkills = GetkUserSkillsByUserName(un);
                if (userSkills.Count > 0)//בדיקה אם המשתמש בעל כישורים
                {
                    for (int j = 0; j < GetkUserSkillsByUserName(un).Count; j++)//ריצה על כל הכישורים של אותו משתמש
                    {
                        if (n.Req.SkillNum == GetkUserSkillsByUserName(un).ElementAt(j).SkillNum)//בדיקה  אם המשתמש בעל הכישור המתאים לבקשה
                        {
                            readyToDo = true;
                            return readyToDo;
                        }

                    }
                    return readyToDo;
                }
                else
                {
                    return readyToDo;
                }

            }
            if (IsUserSettingsExist(un))//בדיקה אם המשתמש קיים בטבלת הגדרות משתמש
            {

                if (GetSettingByUNAndRequestType(un, n.Req.Type))//בדיקה האם המשתמש מוכן לבצע בקשה מסוג זה
                {
                    readyToDo = true;
                    return readyToDo;

                }

            }
            return readyToDo
    ;
        }
        public int GetStatusForSeen(Notification n, string un)
        {
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT RequestStatus FROM BuildApp_Status where[RequestSerialNum] = {n.Req.SerialNum} and [ReceiverUsername] = '{un}'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                int seen = -1;
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    seen = (int)dr["RequestStatus"];

                }

                return seen;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        //timer
        public string CheckRequestDueDate()
        {
            List<Request> activeRequests = GetAllActiveRequest();
            for (int i = 0; i < activeRequests.Count; i++)
            {
                if (activeRequests.ElementAt(i).DueDate < DateTime.Now)
                {
                    UpdateIsActive(activeRequests.ElementAt(i));
                }
            }

            return "ok";
        }

        //send token
        public bool IsTokenExist(string userName)
        {
            SqlConnection con = null;
            bool isEx = false;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM BuildApp_Token";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    if (((string)dr["UserName"]).ToLower() == userName.ToLower())
                    {
                        isEx = true;
                    }
                }

                return isEx;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public string UpdateToken(string userName, string token)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string cStr = $"UPDATE BuildApp_Token SET Token='{token}' WHERE UserName='{userName}'";

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                cmd.ExecuteNonQuery(); // execute the command
                return "ok";
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }


        }
        public string InsertToken(string userName, string token)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string cStr = $"INSERT INTO BuildApp_Token ([UserName],[Token]) VALUES('{userName}', '{token}')";

            try
            {
                cmd = CreateCommand(cStr, con);       // create the command
                cmd.ExecuteNonQuery();               // execute the command
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }

            }
            return "ok";
        }
        //push notification
        public List<string> GetTokens(List<string> userNames)
        {
            string t;
            List<string> tokens = new List<string>();
            foreach (string userName in userNames)
            {
                t = GetToken(userName);
                if (t != "")
                {
                    tokens.Add(t);
                }
            }
            return tokens;

        }
        public string GetToken(string userName)
        {
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT * FROM BuildApp_Token where UserName='{userName}'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    return (string)dr["Token"];
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
            return "";
        }
        public List<string> GetUserNamesByRequest(int serialNum)
        {

            SqlConnection con = null;
            List<string> userNames = new List<string>();
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"select * from BuildApp_Status where RequestSerialNum={serialNum} and RequestStatus!=0";


                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    userNames.Add((string)dr["ReceiverUsername"]);
                }

                return userNames;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        //Rank
        public int GetRate(int requestSerialNum)
        {
            SqlConnection con = null;
            UserToShow uts = new UserToShow();
            int rate = -1;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = $"SELECT * FROM BuildApp_Rank where RequestSerialNum = {requestSerialNum}";


                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    rate = (int)dr["Rate"];
                }

                return rate;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public string PostRank(int requestSerialNum, int rate)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string cStr = $"INSERT INTO BuildApp_Rank ([RequestSerialNum],[Rate]) VALUES({requestSerialNum}, {rate})";

            try
            {
                cmd = CreateCommand(cStr, con);       // create the command
                cmd.ExecuteNonQuery();               // execute the command
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }

            }
            return "ok";
        }
        public bool IsRankExist(int requestSerialNum)
        {
            SqlConnection con = null;
            bool isEx = false;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM BuildApp_Rank";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    if ((int)dr["RequestSerialNum"] == requestSerialNum)
                    {
                        isEx = true;
                    }
                }

                return isEx;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        //public bool IsTopRanking(string userName)
        //{

        //}

    }

}


