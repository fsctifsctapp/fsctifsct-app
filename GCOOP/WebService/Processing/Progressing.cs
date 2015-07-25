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
using pbservice;
using System.Collections.Generic;

namespace WebService.Processing
{
    public class Progressing
    {
        private static List<Running> runnings = new List<Running>();
        public static int count;

        public static int Add(Running running, String application, String w_sheet_id)
        {
            return Add(running, application, w_sheet_id, false);
        }

        public static int Add(Running running, String application, String w_sheet_id, bool isRemoveBefore)
        {
            if (isRemoveBefore)
            {
                try
                {
                    Remove(application, w_sheet_id);
                }
                catch { }
            }
            int i = IndexOf(application, w_sheet_id);
            if (i < 0)
            {
                runnings.Add(running);
                int max = runnings.Count;
                count = max;
                ((MainProgress)runnings[max - 1]).Start(w_sheet_id, application);
                return count;
            }
            else
            {
                throw new Exception("ไม่สามารถดำเนินการได้เนื่องจากมีการประมวลผล " + w_sheet_id + " อยู่แล้ว ");
            }
        }

        public static void Cancel(String application, String w_sheet_id)
        {
            int i = IndexOf(application, w_sheet_id);
            if (i > -1)
            {
                ((MainProgress)runnings[i]).Stop();
                runnings.Remove(runnings[i]);
                int max = runnings.Count;
                count = max;
            }
        }

        public static void Remove(String application, String w_sheet_id)
        {
            int i = IndexOf(application, w_sheet_id);
            if (i > -1)
            {
                try
                {
                    ((MainProgress)runnings[i]).Stop();
                    runnings[i].DisConnect();
                }
                catch { }
                try
                {
                    runnings.RemoveAt(i);
                }
                catch { }
                int max = runnings.Count;
                count = max;
            }
        }

        private static int IndexOf(String application, String w_sheet_id)
        {
            for (int i = 0; i < runnings.Count; i++)
            {
                String app = ((MainProgress)runnings[i]).Application;
                String id = ((MainProgress)runnings[i]).ID;
                if (application == app && id == w_sheet_id)
                {
                    return i;
                }
            }
            return -1;
        }

        private static str_progress GetStr(String application, String w_sheet_id)
        {
            int i = IndexOf(application, w_sheet_id);
            if (i > -1)
            {
                return runnings[i].GetProgress();
            }
            else
            {
                throw new Exception("ไม่มีการประมวลผล");
            }
        }

        public static String[] GetStatus(String application, String w_sheet_id)
        {
            str_progress p = GetStr(application, w_sheet_id);

            String[] str = new String[8];
            str[0] = p.status.ToString();
            str[1] = p.progress_max.ToString();
            str[2] = p.progress_index.ToString();
            str[3] = p.subprogress_max.ToString();
            str[4] = p.subprogress_index.ToString();
            str[5] = p.error_text.Replace(",", "|");
            str[6] = p.progress_text.Replace(",", "|");
            str[7] = p.subprogress_text.Replace(",", "|");

            return str;
        }
    }
}