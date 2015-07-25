using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace CommonLibrary
{
    public class DepositService
    {
        public static string AnalizeAccountNo(String accountNo)
        {
            string as_accno = accountNo;
            char[] lch_temp;
            char lc_temp;
            string ls_accno, ls_accnoright, ls_zero;
            string ls_accmask, ls_realmask;
            int li_pos, li_pos1, li_len, li_right;

            ls_realmask = "";
            as_accno = as_accno.Trim();
            /******?*/
            //ls_accmask	= string( this.of_getattribconstant( "deptcode_format" ) )
            ls_accmask = "";
            //ls_accmask	= trim( ls_accmask )

            lch_temp = ls_accmask.ToCharArray();

            for (int i = 0; i < lch_temp.Length; i++)
            {
                lc_temp = Convert.ToChar(lch_temp[i].ToString().ToUpper());
                if (lc_temp == '-' || lc_temp == '/')
                    ls_realmask += "";
                else
                    ls_realmask += lc_temp.ToString();
            }

            ls_accmask = ls_realmask.Trim();
            li_len = ls_accmask.Length;
            ls_zero = li_len.ToString();//fill( '0', li_len )
            li_pos = ls_accmask.IndexOf('R');// pos( ls_accmask, 'R' )
            li_pos1 = li_pos - 1;
            li_right = li_len - li_pos1;
            ls_accnoright = as_accno.Substring(li_pos); //mid( as_accno, li_pos )
            ls_accnoright = ls_zero + ls_accnoright;//right( ls_zero + ls_accnoright , li_right )
            ls_accno = "";//left( as_accno, li_pos1  )
            ls_accno = "";//ls_accno + ls_accnoright

            return ls_accno;
        }
    }
}
