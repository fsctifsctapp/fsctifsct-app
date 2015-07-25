using System;
using System.Data;
using System.Configuration;
using DBAccess;
using pbservice;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace WebService
{
    public class AgencySvEn
    {
        private n_cst_dbconnectservice svCon;
        private n_cst_agentoperate_service svAgen;
        private n_cst_agentproc_service svAgenUtil;
        private n_cst_agentmemb_service svAgmemb;

        private Security security;

        public AgencySvEn(String wsPass)
        {
            ConstructorEnding(wsPass, true);
        }

        public AgencySvEn(String wsPass, bool autoConnect)
        {
            ConstructorEnding(wsPass, autoConnect);
        }

        private void ConstructorEnding(String wsPass, bool autoConnect)
        {
            security = new Security(wsPass);
            if (autoConnect)
            {
                svCon = new n_cst_dbconnectservice();
                svCon.of_connectdb(security.ConnectionString);
                svAgen = new n_cst_agentoperate_service();
                svAgen.of_initservice(svCon);
                svAgenUtil = new n_cst_agentproc_service();
                svAgenUtil.of_initservice(svCon);

                // EGAT
                svAgmemb = new n_cst_agentmemb_service();
                svAgmemb.of_initservice(svCon);
            }
        }

        public void DisConnect()
        {
            try
            {
                svCon.of_disconnectdb();
            }
            catch { }
        }

        ~AgencySvEn()
        {
            DisConnect();
        }

        //---------------------------------------------------------




        //ed
        public int Initadjustreceive(ref str_agent strAgent)
        {
            try
            {
                int re = svAgen.of_initadjustreceive(ref strAgent);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //ed
        public int SaveAdjustReceive(str_agent strAgent)
        {
            try
            {
                int re = svAgen.of_saveadjustreceive(strAgent);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //ed
        public int CalMemMain(ref str_agent strAgent)
        {
            try
            {
                int re = svAgen.of_calmemmain(ref strAgent);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //ed
        public int InitMoveGroup(ref str_agent strAgent)
        {
            try
            {
                int re = svAgen.of_initmovegroup(ref strAgent);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //ed
        public int SaveMoveGroup(str_agent strAgent)
        {
            try
            {
                int re = svAgen.of_savemovegroup(strAgent);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //bask
        public int InitAgentMain(ref str_agent strAgent)
        {
            try
            {
                int result = svAgen.of_initagentdetail_main(ref strAgent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //bask
        public int InitAgentDetail(ref str_agent strAgent)
        {
            try
            {
                int result = svAgen.of_initagentdetail_detail(ref strAgent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //bask
        public int InitCancelRecieve(ref str_agent strAgent)
        {
            try
            {
                int result = svAgen.of_initcancelreceive(ref strAgent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //bask
        public int SaveCancelReceive(str_agent strAgent)
        {
            try
            {
                int re = svAgen.of_savecancelreceive(strAgent);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //bask
        public int InitClearAgent(ref str_agent strAgent)
        {
            try
            {
                int result = svAgen.of_initclearagent(ref strAgent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //bask
        public int SaveClearAgent(str_agent strAgent)
        {
            try
            {
                int re = svAgen.of_saveclearagent(strAgent);
                DisConnect();
                return re;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }
        //a
        //function init โอนเงินชำระลูกหนี้ตัวแทนตามทะเบียน
        public int of_initreceivemem(ref str_agent astr_agent)
        {
            try
            {

                int result = svAgen.of_initreceivemem(ref astr_agent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }

        }
        //a
        //function save โอนเงินชำระลูกหนี้ตัวแทนตามทะเบียน
        public int of_savereceivemem(str_agent astr_agent)
        {
            try
            {
                int result = svAgen.of_savereceivemem(astr_agent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }

        }
        //a 
        //function of_calmemmain
        public int of_calmemmain(ref str_agent astr_agent)
        {
            try
            {
                int result = svAgen.of_calmemmain(ref astr_agent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }

        }
        //a
        //function search ลูกหนี้
        public int of_searchagentmem(ref str_agent astr_agent)
        {
            try
            {

                int result = svAgen.of_searchagentmem(ref astr_agent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }

        }
        //a
        //function init รับเงินเพิ่ม
        public int of_initaddreceive(ref str_agent astr_agent)
        {
            try
            {

                int result = svAgen.of_initaddreceive(ref astr_agent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }

        }
        //a
        //function save รับเงินเพิ่ม
        public int of_saveaddreceive(str_agent astr_agent)
        {
            try
            {
                int result = svAgen.of_saveaddreceive(astr_agent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }

        }

        //Mai
        //function ประมวลผลเรียกเก็บลูกหนี้ตัวแทน
        public int AgentProcess(String xml_main)
        {
            try
            {
                str_agent astr_agent = new str_agent();
                astr_agent.xml_agentoption = xml_main;
                int result = svAgenUtil.of_agentprocess(astr_agent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai
        //Init โอนเงินชำระลูกหนี้ตัวแทนสังกัด(สังกัด)
        public String InitReceiveGroup(String xml_head)
        {
            try
            {
                str_agent astr_agent = new str_agent();
                astr_agent.xml_head = xml_head;
                int result = 0;
                String xml_detail = "";
                try
                {
                    result = svAgen.of_initreceivegroup(ref astr_agent);
                }
                catch (Exception ex) { xml_detail = ex.Message; }
                if (result == 1)
                {
                    xml_detail = astr_agent.xml_detail;
                }
                DisConnect();
                return xml_detail;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai
        // Save โอนชำระเงินลูกหนี้ตัวแทนสังกัด(สังกัด)
        public int SaveReceiveGroup(String xml_head, String xml_detail)
        {
            try
            {
                str_agent astr_agent = new str_agent();
                astr_agent.xml_head = xml_head;
                astr_agent.xml_detail = xml_detail;
                int re = svAgen.of_savereceivegroup(astr_agent);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }

        //Mai
        //Init โอนเงินชำระลูกหนี้ตัวแทนสังกัด(ทะเบียน)
        public String InitReceiveGroupMem(String xml_head)
        {
            try
            {
                str_agent astr_agent = new str_agent();
                astr_agent.xml_head = xml_head;
                int result = 0;
                String xml_detail = "";
                try
                {
                    result = svAgen.of_initreceivegroupmem(ref astr_agent);
                }
                catch (Exception ex) { xml_detail = ex.Message; }
                if (result == 1)
                {
                    xml_detail = astr_agent.xml_detail;
                }
                DisConnect();
                return xml_detail;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai
        // Save โอนชำระเงินลูกหนี้ตัวแทนสังกัด(ทะเบียน)
        public int SaveReceiveGroupMem(String xml_head, String xml_detail, String as_entry_id, String as_machine_id, DateTime adtm_adj_time, DateTime adtm_system_date)
        {
            try
            {
                str_agent astr_agent = new str_agent();
                astr_agent.xml_head = xml_head;
                astr_agent.xml_detail = xml_detail;
                int re = svAgen.of_savereceivegroupmem(astr_agent, as_entry_id, as_machine_id, adtm_adj_time, adtm_system_date);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }

        //Mai
        //Init รับคืนเงิน
        public String[] InitReturnReceive(String xml_head)
        {
            try
            {
                str_agent astr_agent = new str_agent();
                astr_agent.xml_head = xml_head;

                int result = 0;
                String[] arr = new String[2];
                try
                {
                    result = svAgen.of_initreturnreceive(ref astr_agent);
                }
                catch (Exception ex) { arr[0] = ex.Message; }
                if (result == 1)
                {

                    arr[0] = astr_agent.xml_head;
                    arr[1] = astr_agent.xml_detail;
                }
                DisConnect();
                return arr;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        
        ////Mai
        //// คำนวณยอดโอนชำระ หน้าจอโอนชำระเงินลูกหนี้ตัวแทนสังกัด(ทะเบียน)
        public int SaveReturnReceive(String xml_head, String xml_detail)
        {
            try
            {
                str_agent astr_agent = new str_agent();
                astr_agent.xml_head = xml_head;
                astr_agent.xml_detail = xml_detail;
                int re = svAgen.of_savereturnreceive(astr_agent);
                DisConnect();
                return re;
            }
            catch (Exception ex) { DisConnect(); throw ex; }
        }

        //Mai
        public String Cal_MemMain(String xml_head, String Column_name, Decimal amount)
        {
            try
            {
                str_agent astr_agent = new str_agent();
                astr_agent.xml_head = xml_head;
                astr_agent.column_name = Column_name;
                astr_agent.amount = amount;
                int result = 0;
                string xml_result = "";
                try
                {
                    result = svAgen.of_calmemmain(ref astr_agent);
                }
                catch (Exception ex) { xml_result = ex.Message; }
                if (result == 1)
                {
                    xml_result = astr_agent.xml_head;
                }
                DisConnect();
                return xml_result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //==== EGAT
        //Mai AgentSearchMember หน้าจอค้นหาทะเบียนตัวแทน
        public int SearchAgMemb(ref str_agent astr_agent)
        {
            try
            {

                int result = svAgmemb.of_searchagmemb(ref astr_agent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }

        }

        //Mai InitRegAgMember หน้าจอบันทึกตัวแทน
        public int InitRegAgMemb(ref str_agent astr_agent)
        {
            try
            {

                int result = svAgmemb.of_initreqagmemb(ref astr_agent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }

        }

        //Mai ChangeRegAgMember  หน้าจอบันทึกตัวแทน
        public int ChangeReqAgMember(ref str_agent astr_agent)
        {
            try
            {

                int result = svAgmemb.of_chgreqagmemb(ref astr_agent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }

        }

        //Mai SaveReqAgMemb  หน้าจอบันทึกตัวแทน
        public int SaveReqAgMemb(str_agent astr_agent)
        {
            try
            {
                int result = svAgmemb.of_savereqagmemb(astr_agent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }

        //Mai InitMembdet หน้าจอแสดงรายละเอียดตัวแทน
        public int InitMembdet(ref str_agent astr_agent)
        {
            try
            {
                int result = svAgmemb.of_initmembdet(ref astr_agent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }

        }

        //Mai InitMembdet_detail หน้าจอแสดงรายละเอียดตัวแทน
        public int InitMemdet_detail(ref str_agent astr_agent)
        {
            try
            {
                int result = svAgmemb.of_initmembdet_detail(ref astr_agent);
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                DisConnect();
                throw ex;
            }
        }


    }
}
