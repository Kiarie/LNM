using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


using System.Data.Odbc;

namespace iPayLNM_MySQL
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
            
			string constr = "server=localhost;user="+UID+";database="+dbname+";port=3306;password="+PWD+";";
                //MySqlConnection conn = new MySqlConnection(constr);
                var conn = new OdbcConnection("DSN=modemmysql");

            Console.WriteLine("Connecting to Mysql");
            conn.Open();
            //A select statement to get all rows that do not have code filled in as per said. Also These should contain the unused/ unchecked codes.
            string strSQL = "SELECT * FROM "+dbname+"."+tblnm+" WHERE `sms_text` LIKE '%Confirmed.%' AND `sender_number` IS NOT NULL AND (`code` = '' OR `code` IS NULL)";
			OdbcCommand cmd = new OdbcCommand(strSQL, conn);
			OdbcDataReader dr =  cmd.ExecuteReader();


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
                string ID1 = dr["id"].ToString();
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


                /*//****************************************
                        Response.Write(brk);
                        Response.Write(str);
                        Response.Write(brk);
                        Response.Write(strln);
                    //****************************************
                */

                strSQL = "Update "+dbname+"."+tblnm+" SET `code` = '" + arrText[0] + "', `amount` = '" + arrText[5] + "', `firstname` = '" + arrText[9] + "', `lastname` = '" + arrText[10] + "', `msisdn` = '" + arrText[8] + "' WHERE `id` = " + ID1;
                OdbcCommand update = new OdbcCommand(strSQL, conn);
                //Response.Write(brk);
                //Response.Write(strSQL);

                update.ExecuteNonQuery();
            }   //end of the while loop
        }
        
            //use dynamic when defining a method that returns a var
        static dynamic Keyval()
        {
            var dbname = ConfigurationManager.AppSettings;

            //Console.WriteLine(dbname.GetKey(0)+"==>"+dbname.Get(0));
            return dbname;
            
        }
    }
}

