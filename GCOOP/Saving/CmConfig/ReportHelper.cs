using System;
using System.Collections.Generic;

namespace Saving.CmConfig
{
    public enum ArgumentType
    {
        /// <summary>
        /// String ข้อความ
        /// </summary>
        String = 1,
        /// <summary>
        /// Number จำนวนเต็ม ทศนิยม และตัวเลขทั้งหมด
        /// </summary>
        Number = 2,
        /// <summary>
        /// DateTime วันเวลา
        /// </summary>
        DateTime = 3
    }
    public class ReportArgument
    {
        public int seqNo;
        public String argumentValue;
        public ArgumentType argumentType;
        public ReportArgument(int seqNo, String argumentValue, ArgumentType argumentType)
        {
            this.seqNo = seqNo;
            this.argumentValue = argumentValue;
            this.argumentType = argumentType;
        }
    }
    public class ReportHelper
    {
        List<ReportArgument> args;
        public ReportHelper()
        {
            args = new List<ReportArgument>(10);
        }
        public void AddArgument(String argumentValue, ArgumentType argumentType)
        {
            int seqNo = args.Count + 1;
            ReportArgument arg = new ReportArgument(seqNo,argumentValue,argumentType);
            args.Add(arg);
        }
        public String PopArgumentsXML()
        {
            String ls_xml = "<?xml version=\"1.0\" encoding=\"UTF-16LE\" standalone=\"no\"?"+@">

<d_print_stdcriteria>";

            //สร้าง XML มาตรฐานสำหรับ Report Criteria.
            foreach(ReportArgument arg in args){
                ls_xml += "<d_print_stdcriteria_row>";
                ls_xml += "<argument_seq>"+Convert.ToString(arg.seqNo)+"</argument_seq>";
                ls_xml += "<argument_value>"+arg.argumentValue+"</argument_value>";
                ls_xml += "<argument_type>" + Convert.ToString((int)arg.argumentType) + "</argument_type>";
                ls_xml += "</d_print_stdcriteria_row>";
            }
            ls_xml += "</d_print_stdcriteria>";

            //clear arguments.
            args.Clear();

            return ls_xml;
        }
        public String GetReportPDF(String application, String groupID, String reportID)
        {
            //ก่อนเรียกฟังชั่นนี้ต้องเรียก AddArgument มาให้ครบก่อน.
            //แปลง Argument ทั้งหมดให้อยู่ในรูป XML มาตรฐาน.
            String ls_criteria = PopArgumentsXML();
            
            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

            //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.

            //เคลียร์(ลบ)PDF ก่อนหน้านี้.

            //คืนค่า URL ของ PDF นี้กลับไปให้เด้ง Popup แสดง PDF เอาเอง.
            String ls_url = "";
            return ls_url;
        }

    }
}
