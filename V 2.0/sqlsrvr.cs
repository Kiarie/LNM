using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using System.Data.SqlClient;
using System.Data.Odbc;

namespace iPayLNM
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbname = Keyval().GetKey(0);//use GetKey for getting the Key in position specified
            string tblnm = Keyval().Get(dbname);// Get(string) where string is the key and thus it get s the value where key is string
            string servr = Keyval()["server"];
            string UID = Keyval()["UID"];
            string PWD = Keyval()["PWD"];
            
            //SqlConnection con = new SqlConnection("Data Source=RAYZY\\MPESA;Initial Catalog=lnm;User ID=sa;Password=kennedy1;" + "MultipleActiveResultSets=true");
            var con = new OdbcConnection("Driver={SQL server};Server="+servr+";Database="+dbname+"; Uid="+UID+"; Pwd="+PWD+";");


            OdbcDataReader dr;
            OdbcCommand cmd;

            con.Open();

            //A select statement to get all rows that do not have code filled in as per said. Also These should contain the unused/ unchecked codes.

	    string strSQL = "USE "+dbname;
            cmd = new OdbcCommand(strSQL, con);

            strSQL = "SELECT * from "+dbname+"."+tblnm+" WHERE [code] IS NULL AND [SMS_TEXT] LIKE '%Confirmed.%' AND SENDER_NUMBER IS NOT NULL";
            cmd = new OdbcCommand(strSQL, con);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                string strText = dr[1].ToString();

                //Console.WriteLine(strText);
                //Console.ReadLine();

                string[] arrText = strText.Split(' ');
                //AA64SF914 Confirmed.on 17/2/14 at 12:02 PMKsh33,950.00 received from 254727137226 Mark ADAMS.New Account balance is Ksh2,550.00
                //These are the values that should be inserted to the NULL columns
                /*
                Response.Write(arrText[0]); //TranCode
                Response.Write(brk);
                Response.Write(arrText[5]); //amount
                Response.Write(brk);
                Response.Write(arrText[9]); //fname
                Response.Write(brk);
                Response.Write(arrText[10]); //lname
                Response.Write(brk);
                Response.Write(arrText[8]); //msisdn
                Response.Write(brk);
                */
                //Ignore. These are the other text fields to be used as tests

                //Here I'll be getting the row ID and the update will occur per Id That fulfills the said condition
                string ID1 = dr["ID"].ToString();
                //Response.Write(ID1);

                //****************************************
                var charsToRemove = new string[] { "\n", "PMKsh", "AMKsh", "Ksh", ",", ".New", "'" };
                string str = arrText[5];
                string strfn = arrText[9];
                string strln = arrText[10];
                foreach (var c in charsToRemove)
                {
                    str = str.Replace(c, string.Empty);
                    strfn = strfn.Replace(c, string.Empty);
                    strln = strln.Replace(c, string.Empty);
                }

                arrText[5] = str;
                arrText[9] = strfn;
                arrText[10] = strln;

                //****************************************

                //sqlsrv update query to fulfill the condition. Update code and register code as used
                strSQL = "Update "+dbname+"."+tblnm+" SET [code] = '" + arrText[0] + "', [amount] = '" + arrText[5] + "', [firstname] = '" + arrText[9] + "', [lastname] = '" + arrText[10] + "', [msisdn] = '" + arrText[8] + "' WHERE [ID] = " + ID1;

                var con2 = new OdbcConnection("Driver={SQL server};Server="+servr+";Database="+dbname+"; Uid="+UID+"; Pwd="+PWD+";");
                con2.Open();
                OdbcCommand update = new OdbcCommand(strSQL, con2);
                //Response.Write(brk);
                // Response.Write(strSQL);
                update.ExecuteNonQuery();
                con2.Close();
            }   //end of the while loop
            con.Close();
        }   //end of function
        //use dynamic when defining a method that returns a var
        static dynamic Keyval()
        {
            var dbname = ConfigurationManager.AppSettings;

            //Console.WriteLine(dbname.GetKey(0)+"==>"+dbname.Get(0));
            return dbname;
            
        }
    }      //end of class
}   // end of namespace
