using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.SessionState;

namespace CommonLibrary
{
    public class PDFUtil
    {
        private HttpSessionState Session;

        public PDFUtil(HttpSessionState Session)
        {
            this.Session = Session;
        }

        public String SourceFile
        {
            get
            {
                try
                {
                    return Session["source_file"].ToString();
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                try
                {
                    Session["source_file"] = value;
                }
                catch { }
            }
        }

        public String DesFile
        {
            get
            {
                try
                {
                    return Session["des_file"].ToString();
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                try
                {
                    Session["des_file"] = value;
                }
                catch { }
            }
        }

        public bool IsSendPDF
        {

            get
            {
                try
                {
                    return Convert.ToBoolean(Session["IsSendPDF"]);
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                try
                {
                    Session["IsSendPDF"] = value;
                }
                catch { }
            }
        }
    }
}
