using System;
using System.Data;

using System.Configuration;
using System.Data.Odbc;

namespace Mysql_test
{
	class Program
	{
		static void Main(string[] args)
		{
			string dbname = KeyVal().GetKey(0);
			string tbl = KeyVal().Get(dbname);
			
			string[] arr =  tbl.Split(',');
			
			
			var conn = new OdbcConnection("DSN=modemmysql");
			 conn.Open();
			 try
			 {
			 string strsql = "SELECT "+arr[0]+".`aff_name`, "+arr[1]+".`trans_ammount`, "+arr[1]+".`commission`, `months`.`month`, `merchants`.`merchant` FROM `affs_trans` INNER JOIN `affiliates` ON `affs_trans`.`aff_id` = `affiliates`.`id` INNER JOIN `months` ON `affs_trans`.`mth_id` = `months`.`mid`	INNER JOIN `merchants` ON `affs_trans`.`mer_id` = `merchants`.`id` ";
			OdbcCommand cmd = new OdbcCommand(strsql, conn);
			OdbcDataReader dr = cmd.ExecuteReader();
			
			while (dr.Read())
			{
				Console.WriteLine(dr[0]+"|"+dr[1]+"|"+dr[2]+"|"+dr[3]+"|"+dr[4]);
			}
			dr.Close();
				 
			 }
			 catch (System.Exception ex)
			 {
				 Console.WriteLine(ex.ToString());
			 }

		
		}
		static dynamic KeyVal()
		{
			var dbname = ConfigurationManager.AppSettings;
			 
			return dbname;
		} 
	}
}