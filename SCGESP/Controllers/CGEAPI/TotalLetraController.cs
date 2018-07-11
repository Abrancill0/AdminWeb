using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class TotalLetraController : ApiController
    {

        public class datos
        {
            public double total { get; set; }
        }


        public string PostoCantidadLetra(datos Datos)
        {
            int M;
            string [] numitem = new string[10];
            string[] numescr = new string[10]; 
            string millon1;
            string millon;
            string mill;
            string mil;
            string pesos ="";
            double valor;
            string stvalor, stint, stcentav, xmonto;
            int pos, centav, longi, tope, i, j;
            valor = Math.Round(Datos.total, 2);
            stvalor = Convert.ToString(valor);
            pos = stvalor.IndexOf(".");

            if (pos > 0)
            {
                stint = stvalor.Substring(0, pos );
                stcentav = stvalor.Substring(pos + 1, 1);
                
                if (stcentav.Length == 1)
                {
                    stcentav = stcentav + "0";
                }
            }
            else
            {
                stint = stvalor;
                stcentav = "0";
            }

            centav = Convert.ToInt32(stcentav);
            xmonto = stint.Trim();
            longi = xmonto.Length - 1;
            i = 0;

            for (j = longi; j >= 1; j += -1)
            {
                numitem[i] = xmonto.Substring( j,1);
                i = i + 1;
            }

            try
            {
                M = Convert.ToInt32(Math.Log(valor) / Math.Log(10));
            }
            catch (Exception ex)
            {
                M = 0;
            }

            tope = longi;
            for (i = 1; i <= tope; i += 3)
            {
                if (numitem[i] == "0")
                {
                    numescr[i] = "";
                }
                else if (numitem[i] == "1")
                {
                    //numescr[i] = IIf(M == 10, "", "UN ");

                    numescr[i] = (M == 10)?"": "UN ";
                }
                else if (numitem[i] == "2")
                {
                    numescr[i] = "DOS ";
                }
                else if (numitem[i] == "3")
                {
                    numescr[i] = "TRES ";
                }
                else if (numitem[i] == "4")
                {
                    numescr[i] = "CUATRO ";
                }
                else if (numitem[i] == "5")
                {
                    numescr[i] = "CINCO ";
                }
                else if (numitem[i] == "6")
                {
                    numescr[i] = "SEIS ";
                }
                else if (numitem[i] == "7")
                {
                    numescr[i] = "SIETE ";
                }
                else if (numitem[i] == "8")
                {
                    numescr[i] = "OCHO ";
                }
                else if (numitem[i] == "9")
                {
                    numescr[i] = "NUEVE ";
                }

                if (numitem[i] == "1" & numitem[i + 1] == "1")
                {
                    numescr[i + 1] = "ONCE ";
                    numescr[i] = "";
                }
                else if (numitem[i] == "2" & numitem[i + 1] == "1")
                {
                    numescr[i + 1] = "DOCE ";
                    numescr[i] = "";
                }
                else if (numitem[i] == "3" & numitem[i + 1] == "1")
                {
                    numescr[i + 1] = "TRECE ";
                    numescr[i] = "";
                }
                else if (numitem[i] == "4" & numitem[i + 1] == "1")
                {
                    numescr[i + 1] = "CATORCE ";
                    numescr[i] = "";
                }
                else if (numitem[i] == "5" & numitem[i + 1] == "1")
                {
                    numescr[i + 1] = "QUINCE ";
                    numescr[i] = "";
                }
                else if (Convert.ToInt32(numitem[i]) > 5 & numitem[ i + 1] == "1")
                {
                    numescr[i + 1] = "DIECI";
                }

                if (numitem[i + 1] == "1" & numitem[i] == "0")
                {
                    numescr[i + 1] = "DIEZ ";
                }
                else if (numitem[i + 1] == "2" & numitem[i] == "0")
                {
                    numescr[i + 1] = "VEINTE ";
                }
                else if (numitem[i + 1] == "3" & numitem[i] == "0")
                {
                    numescr[i + 1] = "TREINTA ";
                }
                else if (numitem[i + 1] == "4" & numitem[i] == "0")
                {
                    numescr[i + 1] = "CUARENTA ";
                }
                else if (numitem[i + 1] == "5" & numitem[i] == "0")
                {
                    numescr[i + 1] = "CINCUENTA ";
                }
                else if (numitem[i + 1] == "6" & numitem[i] == "0")
                {
                    numescr[i + 1] = "SESENTA ";
                }
                else if (numitem[i + 1] == "7" & numitem[i] == "0")
                {
                    numescr[i + 1] = "SETENTA ";
                }
                else if (numitem[i + 1] == "8" & numitem[i] == "0")
                {
                    numescr[i + 1] = "OCHENTA ";
                }
                else if (numitem[i + 1] == "9" & numitem[i] == "0")
                {
                    numescr[i + 1] = "NOVENTA ";
                }
                else if (numitem[i + 1] == "2")
                {
                    numescr[i + 1] = "VEINTI";
                }
                else if (numitem[i + 1] == "3")
                {
                    numescr[i + 1] = "TREINTA Y ";
                }
                else if (numitem[i + 1] == "4")
                {
                    numescr[i + 1] = "CUARENTA Y ";
                }
                else if (numitem[i + 1] == "5")
                {
                    numescr[i + 1] = "CINCUENTA Y ";
                }
                else if (numitem[i + 1] == "6")
                {
                    numescr[i + 1] = "SESENTA Y ";
                }
                else if (numitem[i + 1] == "7")
                {
                    numescr[i + 1] = "SETENTA Y ";
                }
                else if (numitem[i + 1] == "8")
                {
                    numescr[i + 1] = "OCHENTA Y ";
                }
                else if (numitem[i + 1] == "9")
                {
                    numescr[i + 1] = "NOVENTA Y ";
                }
                else if (numitem[i + 1] == "0")
                {
                    numescr[i + 1] = "";
                }

                if (numitem[i + 2] == "0")
                {
                    numescr[i + 2] = "";
                }
                else if (numitem[i + 2] == "1")
                {
                    if ((numitem[i + 1] + numitem[i] != "00"))
                    {
                        numescr[i + 2] = "CIENTO ";
                    }
                    else
                    {
                        numescr[i + 2] = "CIEN ";
                    }
                }
                else if (numitem[i + 2] == "2")
                {
                    numescr[i + 2] = "DOSCIENTOS ";
                }
                else if (numitem[i + 2] == "3")
                {
                    numescr[i + 2] = "TRESCIENTOS ";
                }
                else if (numitem[i + 2] == "4")
                {
                    numescr[i + 2] = "CUATROCIENTOS ";
                }
                else if (numitem[i + 2] == "5")
                {
                    numescr[i + 2] = "QUINIENTOS ";
                }
                else if (numitem[i + 2] == "6")
                {
                    numescr[i + 2] = "SEISCIENTOS ";
                }
                else if (numitem[i + 2] == "7")
                {
                    numescr[i + 2] = "SETECIENTOS ";
                }
                else if (numitem[i + 2] == "8")
                {
                    numescr[i + 2] = "OCHOCIENTOS ";
                }
                else if (numitem[i + 2] == "9")
                {
                    numescr[i + 2] = "NOVECIENTOS ";
                }
            }

            mil = "";
            mill = "";
            millon1 = "";
            millon = "";
            if (longi < 4)
            {
                mill = "";
                mil = "";
            }
            else if (longi < 7)
            {
                mill = "";
                mil = "MIL ";
            }
            else if (longi == 7)
            {
                if (Convert.ToInt32(numitem[7]) <= 1)
                {
                    if (Convert.ToInt32((xmonto.Substring(2, 6))) == 0)
                    {
                        mill = "MILLON DE ";
                        mil = "";
                    }
                    else if (Convert.ToInt32((xmonto.Substring(2, 3))) == 0)
                    {
                        mill = "MILLON ";
                        mil = "";
                    }
                    else
                    {
                        mill = "MILLON ";
                        mil = "MIL ";
                    }
                }
                else
                {
                    if (Convert.ToInt32((xmonto.Substring(2, 6))) == 0)
                    {
                        mill = "MILLONES DE ";
                        mil = "";
                    }
                    else if (Convert.ToInt32((xmonto.Substring(2, 3))) == 0)
                    {
                        mill = "MILLONES ";
                        mil = "";
                    }
                    else
                    {
                        mill = "MILLONES ";
                        mil = "MIL ";
                    }
                }
            }
            else if (longi == 9)
            {
                if (Convert.ToInt32((xmonto.Substring( 4, 6))) == 0)
                {
                    mill = "MILLONES DE ";
                    mil = "";
                }
                else if (Convert.ToInt32((xmonto.Substring(7, 3))) == 0)
                {
                    mill = "MILLONES ";
                    mil = "MIL ";
                }
                else if (Convert.ToInt32((xmonto.Substring(4, 3))) == 0)
                {
                    mill = "MILLONES ";
                    mil = "";
                }
                else
                {
                    mill = "MILLONES ";
                    mil = "MIL ";
                }
            }
            else if (longi == 10)
            {
                millon = "MILLONES ";
                millon1 = "MIL ";
            }
            else
            {
                if (Convert.ToInt32((xmonto.Substring(3, 6))) == 0)
                {
                    mill = "MILLONES DE ";
                    mil = "";
                }
                else if (Convert.ToInt32((xmonto.Substring( 6, 3))) == 0)
                {
                    mill = "MILLONES ";
                    mil = "MIL ";
                }
                else if (Convert.ToInt32((xmonto.Substring(3, 3))) == 0)
                {
                    mill = "MILLONES ";
                    mil = "";
                }
                else
                {
                    mill = "MILLONES ";
                    mil = "MIL ";
                }
            }

            //if (TipoMoneda == "USD")
            //{
            //    if (centav > 0)
            //    {
            //        pesos = "USD " + stcentav + "/100";
            //    }
            //    else
            //    {
            //        pesos = "USD 00/100";
            //    }
            //}
            //else if (TipoMoneda == "MXN")
            //{
                if (centav > 0)
                {
                    pesos = "PESOS " + stcentav + "/100 M.N.";
                }
                else
                {
                    pesos = "PESOS 00/100 M.N.";
                }
            //}
            
            return numescr[10] + millon1 + numescr[9] + numescr[8] + numescr[7] + millon + mill + numescr[6] + numescr[5] + numescr[4] + mil + millon1 + numescr[3] + numescr[2] + numescr[1] + pesos;;
        }

    }
}
