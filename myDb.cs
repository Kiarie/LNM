using System;
using System.Collections;
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
			string dbname = Keyval().GetKey(0);//use GetKey for getting the Key in position specified
			string tblnm = Keyval().Get(dbname);// Get(string) where string is the key and thus it get s the value where key is string
			string dir = Keyval()["dir"];
			string UID = Keyval()["UID"];
			string PWD = Keyval()["PWD"];
			var conn = new OdbcConnection("DRIVER=Firebird/InterBase(r) driver; UID="+UID+"; PWD="+PWD+";DBNAME="+dir+";");
			conn.Open();
			//string tblnm = ReadDbname();
			try{
				string sql = "SELECT * FROM "+tblnm;

				OdbcCommand cmd = new OdbcCommand(sql, conn);
				OdbcDataReader dr = cmd.ExecuteReader();
				Console.WriteLine("FROM DATABASE: "+dbname+" TABLE ==> "+tblnm);
			
				while(dr.Read())
				{
					Console.WriteLine(dr[0]+"___"+dr[1]+"___"+dr[2]);
				}
				dr.Close();
				
			}
			catch(Exception ex){
			Console.WriteLine(ex.ToString());
			}

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