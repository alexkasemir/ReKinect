using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormTestConnection
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            string connectionString = null;
            SqlConnection cnn ;
		    connectionString = "Server=tcp:podium1.database.windows.net,1433;Database=ReKinect;User ID=podium@podium1;Password=I<3rekinect;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
            cnn = new SqlConnection(connectionString);
            try
            {
                cnn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            }

            try
            { 

                var cmd = cnn.CreateCommand();
                // cmd.CommandText = @"
                //INSERT podiumKinect.Users (name, score)
                //OUTPUT INSERTED.id
                //VALUES (@name, @score)";
                cmd.CommandText = "INSERT into dbo.Users (userID, username, firstName, password) VALUES (@uID, @uname, @fname, @pass)";
                cmd.Parameters.AddWithValue("@uID", 4);
                cmd.Parameters.AddWithValue("@uname", "dfeers01");
                cmd.Parameters.AddWithValue("@fname", "Danielle");
                cmd.Parameters.AddWithValue("@pass", "podium");

                var inserted = cmd.ExecuteScalar();

                MessageBox.Show ("Connection Open!... ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Query might be broken!");
            }
            finally
            {
                cnn.Close();
            }



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
