using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCGESP.Clases
{
    public static class Seguridad
    {

        /// Encripta una cadena
        public static string Encriptar(this string _cadenaAencriptar)
        {
            string result = string.Empty;
            try {
                byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
                result = Convert.ToBase64String(encryted);
            }
            catch(Exception err){
                var error = err;
                result = "";
            }
            
            return result;
        }

        /// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
        public static string DesEncriptar(this string _cadenaAdesencriptar)
        {
            string result = string.Empty;
            try
            {
                byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
                //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
                result = System.Text.Encoding.Unicode.GetString(decryted);
            } catch (Exception err) {
                var error = err;
                result = "";
            }
            return result;
        }
    }
}