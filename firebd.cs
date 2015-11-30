using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using System.Data.Odbc;

namespace MyDb
{
	class conctProgram
	{
		static void Main(string[] args)
		{
			var conn = new OdbcConnection("DRIVER=Firebird/InterBase(r) driver; UID=SYSDBA; PWD=kiambu;DBNAME=C:\\Program Files\\Firebird\\Firebird_2_5\\examples\\empbuild\\"+dbname+".FDB;");
			conn.Open();
			string tblnm = ReadDbname();
			
			try{
			string strSQL = "SELECT * FROM `rexol`."+tblnm+" WHERE `sms_text` LIKE '%Confirmed.%' AND `sender_number` IS NOT NULL AND (`code` = '' OR `code` IS NULL)";
            OdbcDataAdapter da = new OdbcDataAdapter(strSQL, conn);
            cmd = new OdbcCommand(strSQL, conn);
            dr = cmd.ExecuteReader();


           	 while (dr.Read())
            	{

                string strText = dr[1].ToString();

                //Console.WriteLine(strText);
                //Console.ReadLine();

                string[] arrText = strText.Split(' ');
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

                strSQL = "Update `rexol`."+tblnm+" SET `code` = '" + arrText[0] + "', `amount` = '" + arrText[5] + "', `firstname` = '" + arrText[9] + "', `lastname` = '" + arrText[10] + "', `msisdn` = '" + arrText[8] + "' WHERE `id` = " + ID1;
                OdbcCommand update = new OdbcCommand(strSQL, con);
                //Response.Write(brk);
                //Response.Write(strSQL);

                update.ExecuteNonQuery();
            	}   //end of the while loop
				dr.Close();
			}
			catch(Exception ex)
			{
			Console.WriteLine(ex.ToString());
			}
		
        
		}
		static string ReadDbname()
		{			
		string dbnm = ConfigurationManager.AppSettings["DB"];//Read from the Config file with Key DB
		return dbnm;
		}
		static void Keyval()
		{
			string dbname = ConfigurationManager.AppSettings;

			for(DictionaryEntry value in dbname)
			{
				Console.WriteLine(value.key+"---"+value.value)
			}
		}

	}
}