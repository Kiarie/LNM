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

            //SqlConnection con = new SqlConnection("Data Source=RAYZY\\MPESA;Initial Catalog=lnm;User ID=sa;Password=kennedy1;" + "MultipleActiveResultSets=true");
            var con = new OdbcConnection("DSN=modemsqlserver;MultipleActiveResultSets=true");


            OdbcDataReader dr;
            OdbcCommand cmd;

            con.Open();

            //A select statement to get all rows that do not have code filled in as per said. Also These should contain the unused/ unchecked codes.

	    string strSQL = "USE [lnm]";
            cmd = new OdbcCommand(strSQL, con);

            strSQL = "SELECT * from [dbo].[SMS_IN] WHERE [code] IS NULL AND [SMS_TEXT] LIKE '%Confirmed.%' AND SENDER_NUMBER IS NOT NULL";
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
                strSQL = "Update [dbo].[SMS_IN] SET [code] = '" + arrText[0] + "', [amount] = '" + arrText[5] + "', [firstname] = '" + arrText[9] + "', [lastname] = '" + arrText[10] + "', [msisdn] = '" + arrText[8] + "' WHERE [ID] = " + ID1;

                var con2 = new OdbcConnection("DSN=modemsqlserver;MultipleActiveResultSets=true");
                con2.Open();
                OdbcCommand update = new OdbcCommand(strSQL, con2);
                //Response.Write(brk);
                // Response.Write(strSQL);
                update.ExecuteNonQuery();
                con2.Close();
            }   //end of the while loop
            con.Close();
        }   //end of function
    }      //end of class
}   // end of namespace
