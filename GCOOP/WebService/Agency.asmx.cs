using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using pbservice;
using WebService.Processing;

namespace WebService
{
    /// <summary>
    /// Summary description for Agency
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Agency : System.Web.Services.WebService
    {   
        //ed
        [WebMethod]
        public int Initadjustreceive(String wsPass, ref str_agent strAgent)
        {
            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.Initadjustreceive(ref strAgent);
        }
       
        //ed
        [WebMethod]
        public int SaveAdjustReceive(String wsPass, str_agent strAgent)
        {
            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.SaveAdjustReceive(strAgent);
        }

        //ed
        [WebMethod]
        public int CalMemMain(String wsPass, ref str_agent strAgent)
        {
            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.CalMemMain(ref strAgent);
        }

        //ed
        [WebMethod]
        public int InitMoveGroup(String wsPass, ref str_agent strAgent)
        {
            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.InitMoveGroup(ref strAgent);
        }

        //ed
        [WebMethod]
        public int SaveMoveGroup(String wsPass,str_agent strAgent)
        {
            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.SaveMoveGroup(strAgent);
        }
        
        //bask
        [WebMethod]
        public int InitCancelRecieve(String wsPass, ref str_agent strAgent)
        {
            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.InitCancelRecieve(ref strAgent);


        }
        
        //bask
        [WebMethod]
        public int SaveCancelReceive(String wsPass, str_agent strAgent)
        {
            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.SaveCancelReceive(strAgent);
        }      

        //a
        //function init โอนเงินชำระลูกหนี้ตัวแทนตามทะเบียน
        [WebMethod]
        public int of_initreceivemem(String wsPass, ref str_agent astr_agent)
        {
            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.of_initreceivemem(ref astr_agent);

        }
        //a
        //function save โอนเงินชำระลูกหนี้ตัวแทนตามทะเบียน
        [WebMethod]
        public int of_savereceivemem(String wsPass, str_agent astr_agent)
        {
            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.of_savereceivemem(astr_agent);
        }
        //a
        //function of_calmemmain
        [WebMethod]
        public int of_calmemmain(String wsPass, ref str_agent astr_agent)
        {

            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.of_calmemmain(ref astr_agent);

        }
        //a
        //function search ลูกหนี้
        [WebMethod]
        public int of_searchagentmem(String wsPass, ref str_agent astr_agent)
        {
            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.of_searchagentmem(ref astr_agent);

        }
        //a
        //function init รับเงินเพิ่ม
        [WebMethod]
        public int of_initaddreceive(String wsPass, ref str_agent astr_agent)
        {
            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.of_initaddreceive(ref astr_agent);

        }
        //a
        //function save รับเงินเพิ่ม
        [WebMethod]
        public int of_saveaddreceive(String wsPass, str_agent astr_agent)
        {
            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.of_saveaddreceive(astr_agent);
        }

        //bask
        [WebMethod]
        public int InitAgentMain(String wsPass,ref str_agent strAgent)
        {
            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.InitAgentMain(ref strAgent);
        }

        //bask
        [WebMethod]
        public int InitAgentDetail(String wsPass, ref str_agent strAgent)
        {
            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.InitAgentDetail(ref strAgent);
        }

        //bask
        [WebMethod]
        public int InitClearAgent(String wsPass, ref str_agent strAgent)
        {
            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.InitClearAgent(ref strAgent);
        }

        //bask
        [WebMethod]
        public int SaveClearAgent(String wsPass, str_agent strAgent)
        {
            AgencySvEn Agen = new AgencySvEn(wsPass);
            return Agen.SaveClearAgent(strAgent);
        }

        //Mai
        //ประมวลเรียกเก็บลูกหนี้ตัวแทน
        [WebMethod]
        public int AgentProcess(String wsPass, String xml_main)
        {
            AgencySvEn Ag = new AgencySvEn(wsPass);
            return Ag.AgentProcess(xml_main);
        }

        //Mai
        //function Process Bar ประมวลเรียกเก็บลูกหนี้ตัวแทน
        [WebMethod]
        public int RunAgentProcess(String wsPass, String xml_head, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            AgentProcess Ag = new AgentProcess(sec.ConnectionString, xml_head);
            return Processing.Progressing.Add(Ag, application, w_sheet_id, true);
        }

        //Mai
        //function Init โอนชำระลูกหนี้ตัวแทนสังกัด (สังกัด)
        [WebMethod]
        public String InitReceiveGroup(String wsPass, String xml_head)
        {
            AgencySvEn Ag = new AgencySvEn(wsPass);
            return Ag.InitReceiveGroup(xml_head);
        }

        //Mai
        //function Save โอนชำระลูกหนี้ตัวแทนสังกัด (สังกัด) Progressbar Save
        [WebMethod]
        public int RunSaveRecGroupProcess(String wsPass, String xml_head, String xml_detail, String application, String w_sheet_id)
        {
            Security sec = new Security(wsPass);
            SaveReceiveGroup Ag = new SaveReceiveGroup(sec.ConnectionString, xml_head, xml_detail);
            return Processing.Progressing.Add(Ag, application, w_sheet_id, true);
        }

        //Mai
        //function Init โอนชำระลูกหนี้ตัวแทนสังกัด (สังกัด)
        [WebMethod]
        public String InitReceiveGroupMem(String wsPass, String xml_head)
        {
            AgencySvEn Ag = new AgencySvEn(wsPass);
            return Ag.InitReceiveGroupMem(xml_head);
        }

        //Mai
        //function Save โอนชำระลูกหนี้ตัวแทนสังกัด (สังกัด) Progressbar Save
        [WebMethod]
        public int RunSaveRecGroupMemProcess(String wsPass, String xml_head, String xml_detail, String application, String w_sheet_id, String as_entry_id, String as_machine_id, DateTime adtm_adj_time, DateTime adtm_system_date)
        {
            Security sec = new Security(wsPass);
            SaveReceiveGroupMem Ag = new SaveReceiveGroupMem(sec.ConnectionString, xml_head, xml_detail, as_entry_id, as_machine_id, adtm_adj_time, adtm_system_date);
            return Processing.Progressing.Add(Ag, application, w_sheet_id, true);
        }

        //Mai
        //function Init รับคืนเงิน
        [WebMethod]
        public String[] InitReturnReceive(String wsPass, String xml_head)
        {
            AgencySvEn Ag = new AgencySvEn(wsPass);
            return Ag.InitReturnReceive(xml_head);
        }

        //Mai
        //function CalMemMain
        [WebMethod]
        public String Cal_MemMain(String wsPass, String xml_head, String column_name, Decimal amount)
        {
            AgencySvEn Ag = new AgencySvEn(wsPass);
            return Ag.Cal_MemMain(xml_head, column_name, amount);
        }


        //Mai
        //function Save โอนชำระลูกหนี้ตัวแทนสังกัด (สังกัด) Progressbar Save
        [WebMethod]
        public int SaveReturnReceive(String wsPass, String xml_head, String xml_detail)
        {
            AgencySvEn Ag = new AgencySvEn(wsPass);
            return Ag.SaveReturnReceive(xml_head, xml_detail);
        }


        //======EGAT 
        //Mai SearchChangeMemb หน้าจอค้นหาต้วแทน
        [WebMethod]
        public int SearchChangeMemb(String wsPass, ref str_agent astr_agent)
        {
            AgencySvEn Ag = new AgencySvEn(wsPass);
            return Ag.SearchAgMemb(ref astr_agent);

        }

        //Mai InitReqAgMemb หน้าจอบันทึกตัวแทน
        [WebMethod]
        public int InitReqAgMemb(String wsPass, ref str_agent astr_agent)
        {
            AgencySvEn Ag = new AgencySvEn(wsPass);
            return Ag.InitRegAgMemb(ref astr_agent);

        }

        //Mai ChgReqAgMemb หน้าจอบันทึกตัวแทน
        [WebMethod]
        public int ChgReqAgMemb(String wsPass, ref str_agent astr_agent)
        {
            AgencySvEn Ag = new AgencySvEn(wsPass);
            return Ag.ChangeReqAgMember(ref astr_agent);

        }

        //Mai SaveReqAgMemb หน้าจอบันทึกตัวแทน
        [WebMethod]
        public int SaveReqAgMemb(String wsPass, str_agent astr_agent)
        {
            AgencySvEn Ag = new AgencySvEn(wsPass);
            return Ag.SaveReqAgMemb(astr_agent);
        }

        //Mai InitMembdet หน้าจอแสดงรายละเอียดตัวแทน
        [WebMethod]
        public int InitMembdet(String wsPass, ref str_agent astr_agent)
        {
            AgencySvEn Ag = new AgencySvEn(wsPass);
            return Ag.InitMembdet(ref astr_agent);
        }

        //Mai InitMembdet_detail หน้าจอแสดงรายละเอียดตัวแทน
        [WebMethod]
        public int InitMembdet_detail(String wsPass, ref str_agent astr_agent)
        {
            AgencySvEn Ag = new AgencySvEn(wsPass);
            return Ag.InitMemdet_detail(ref astr_agent);

        }
    }
}
