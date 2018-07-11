using System.Configuration;

namespace SCGESP.Clases
{
    static class VariablesGlobales
    {

        //string ConnectionString = ConfigurationManager.ConnectionStrings["CGEConnectionString"].ConnectionString;
        //public static string CadenaConexion = "Data Source=ELPOTOSI-GAPP\\SQLGAPP;Initial Catalog=CGE;Persist Security Info=True;User ID=sa;Password=t649FGZR";
        public static string CadenaConexion = ConfigurationManager.ConnectionStrings["CGEConnectionString"].ConnectionString; //"Data Source=34.205.247.102;Initial Catalog=CGE;Persist Security Info=True;User ID=sa;Password=t649FGZR";
        //public static string CadenaConexionEle = "Data Source=25.54.9.134;Initial Catalog=ElegrpSp;Persist Security Info=True;User ID=sa;Password=44Brancill0";

    }
}