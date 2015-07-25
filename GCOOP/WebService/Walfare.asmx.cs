using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using GcoopServiceCs;
using GcoopServiceCs.WfStruct;
using System.Data;

namespace WebService
{
    /// <summary>
    /// Summary description for Walfare
    /// </summary>
    [WebService(Namespace = "http://isocare.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Walfare : System.Web.Services.WebService
    {
        [WebMethod]
        public int[] SaveReqWalfareNew(String wsPass, String application, String pbl, String xmlDwMain, String xmlDwRelate, String xmlDwSlip)
        {
            return new WalfareService(wsPass, application).SaveReqWalfareNew(pbl, xmlDwMain, xmlDwRelate, xmlDwSlip);
        }

        [WebMethod]
        public String SaveReqWalfareNewLight(String wsPass, String application, DateTime workDate, String pbl, DataTable dtMain, DataTable dtSlip, DataTable dtRelate)
        {
            return new WalfareService(wsPass, application).SaveReqWalfareNewLight(pbl, workDate, dtMain, dtSlip, dtRelate);
        }

        [WebMethod]
        public int[] UpdateReqWalfareNewLight(String wsPass, String application, DataTable dtMain, DataTable dtSlip, DataTable dtRelate)
        {
            return new WalfareService(wsPass, application).UpdateReqWalfareNewLight(dtMain, dtSlip, dtRelate);
        }

        [WebMethod]
        public int EditReqWalfareRelate(String wsPass, String application, String pbl, String xmlRelate)
        {
            return new WalfareService(wsPass, application).EditReqWalfareRelate(pbl, xmlRelate);
        }

        [WebMethod]
        public int DeleteWalRelateRowReqEdit(String wsPass, String application, String docreqno, String seq_no)
        {
            return new WalfareService(wsPass, application).DeleteReqWalfareRelate(docreqno, seq_no);
        }

        [WebMethod]
        public int NewUserAccount(String wsPass, String application, UserAccount strUserAcc)
        {
            return new WalfareService(wsPass, application).NewUserAccount(strUserAcc);
        }

        [WebMethod]
        public int PermissUsers(String wsPass, String application, String pbl, String xmlDwMain)
        {
            return new WalfareService(wsPass, application).PermissUsers(pbl, xmlDwMain);
        }

        [WebMethod]
        public string SavePaid(String wsPass, String application, String pbl, String xmlDwMain, String xmlDwSlip, String reqDocNo)
        {
            return new WalfareService(wsPass, application).SavePaid(pbl, xmlDwMain, xmlDwSlip, reqDocNo);
        }

        [WebMethod]
        public bool SavePaidAdd(String wsPass, String application, String pbl, String xmlDwList)
        {
            return new WalfareService(wsPass, application).SavePaidAdd(pbl, xmlDwList);
        }

        [WebMethod]
        public int SvAuditEdit(String wsPass, String application, String pbl, String Mxml, String Cxml, string UserNanme, string Branch_id, string Cs_type, string dwName, string[] colid)
        {
            return new WalfareService(wsPass, application).SvAuditEdit(pbl, Mxml, Cxml, UserNanme, Branch_id, Cs_type, dwName, colid);
        }

        [WebMethod]
        public Boolean GenSlip(String wsPass, String application, DateTime workDate, string user_name)
        {
            return new WalfareService(wsPass, application).GenSlip(workDate, user_name);
        }
        [WebMethod]
        public Boolean UpdateReqDocno(String wsPass, String application)
        {
            return new WalfareService(wsPass, application).UpdateReqDocno();
        }

         [WebMethod]
        public Boolean AgeChgProc(String wsPass, String application, String cs_type, DateTime st_date, DateTime end_date)
        {
            return new WalfareService(wsPass, application).AgeChgProc(cs_type, st_date, end_date);
        }

        [WebMethod]
        public string CheckDuplicate(String wsPass, String application, string pbl, String xmlDwMain, string[] listDB, string cs_type, int user_type)
        {
            return new WalfareService(wsPass, application).SetCheckDuplicate(pbl, xmlDwMain, listDB, cs_type, user_type);
        }

        [WebMethod]
        public string CheckDuplicateDT(String wsPass, String application, string pbl, DataTable DtMain, string[] listDB, string cs_type, int user_type)
        {
            return new WalfareService(wsPass, application).SetCheckDuplicateDT(pbl, DtMain, listDB, cs_type, user_type);
        }

        [WebMethod]
        public Boolean SaveWaitGroupPaid(String wsPass, String application, string pbl, String xmlDwMain, int period)
        {
            return new WalfareService(wsPass, application).SaveWaitGroupPaid(pbl, xmlDwMain, period);
        }

        [WebMethod]
        public Boolean SavePersonChgStatus(String wsPass, String application, string pbl, String xmlDwMain)
        {
            return new WalfareService(wsPass, application).SavePersonChgStatus(pbl, xmlDwMain);
        }

        [WebMethod]
        public Boolean SaveGroupPaid(String wsPass, String application, string pbl, String xmlDwMain, string entry_id, string branch_id, int statement_flag, int period, decimal fee, int fee_flag)
        {
            return new WalfareService(wsPass, application).SaveGroupPaid(pbl, xmlDwMain, entry_id, branch_id, statement_flag, period, fee, fee_flag);
        }

        [WebMethod]
        public Boolean SaveGroupPaidNew(String wsPass, String application, string pbl, String xmlDwMain, string entry_id, string branch_id, int statement_flag, int period, decimal fee, int fee_flag)
        {
            return new WalfareService(wsPass, application).SaveGroupPaidNew(pbl, xmlDwMain, entry_id, branch_id, statement_flag, period, fee, fee_flag);
        }


        [WebMethod]
        public Boolean ChgEffective(String wsPass, String application, string pbl, String xmlDwMain, string entry_id, string branch_id, int statement_flag, int period, decimal fee, int fee_flag)
        {
            return new WalfareService(wsPass, application).ChgEffective(pbl, xmlDwMain, entry_id, branch_id, statement_flag, period, fee, fee_flag);
        }

        [WebMethod]
        public bool TrnMemb(String wsPass, String application, String XmlMain, String pbl, String UserName, String oldBranch, String cs_type)
        {
            return new WalfareService(wsPass, application).TrnMemb(XmlMain, pbl, UserName, oldBranch, cs_type);
        }

        [WebMethod]
        public Boolean TrnMemb_ChgBranch(String wsPass, String application, String XmlMain, string pbl, string UserName, string oldbranch_id, string cs_type, DateTime deptopen_date, decimal prncbal)
        {
            return new WalfareService(wsPass, application).TrnMemb_ChgBranch(XmlMain, pbl, UserName, oldbranch_id, cs_type, deptopen_date, prncbal);
        }

        [WebMethod]
        public Boolean TrnMemb_ChgBranch_confirm(String wsPass, String application, String XmlMain, string pbl, string UserName, string cs_type, DateTime deptopen_date, decimal prncbal)
        {
            return new WalfareService(wsPass, application).TrnMemb_ChgBranch_confirm(XmlMain, pbl, UserName, cs_type, deptopen_date, prncbal);
        }

        [WebMethod]
        public Boolean TrnMemb_In(String wsPass, String application, String XmlMain, string pbl, string UserName, string cs_type)
        {
            return new WalfareService(wsPass, application).TrnMemb_In(XmlMain, pbl, UserName, cs_type);
        }

        [WebMethod]
        public Boolean Edit_deptacc_no(String wsPass, String application, string branch_id, string cs_type)
        {
            return new WalfareService(wsPass, application).Edit_deptacc_no(branch_id, cs_type);
        }

        [WebMethod]
        public Boolean Add_FeeYear(String wsPass, String application, String XmlMain, string pbl, string UserName, string cs_type, string branch_id)
        {
            return new WalfareService(wsPass, application).Add_FeeYear(XmlMain, pbl, UserName, cs_type, branch_id);
        }

        [WebMethod]
        public Boolean Edit_bank_branch(String wsPass, String application, String XmlMain, string pbl, string UserName, string cs_type, string branch_id)
        {
            return new WalfareService(wsPass, application).Edit_bank_branch(XmlMain, pbl, UserName, cs_type, branch_id);
        }

        [WebMethod]
        public Boolean TrnMemb_Out(String wsPass, String application, String XmlMain, string pbl, string UserName, string oldbranch_id, string cs_type)
        {
            return new WalfareService(wsPass, application).TrnMemb_Out(XmlMain, pbl, UserName, oldbranch_id, cs_type);
        }

        [WebMethod]
        public Boolean Pay_die_member(String wsPass, String application, String XmlMain, string pbl, string UserName, string cs_type)
        {
            return new WalfareService(wsPass, application).Pay_die_member(XmlMain, pbl, UserName, cs_type);
        }

        [WebMethod]
        public Boolean infrom_die_mm_save(String wsPass, String application, String XmlMain, string pbl, string UserName, string cs_type, string member_no, string deptaccount_no, string branch_id_in, string resignChg)
        {
            return new WalfareService(wsPass, application).infrom_die_mm_save(XmlMain, pbl, UserName, cs_type, member_no, deptaccount_no, branch_id_in, resignChg);
        }

        [WebMethod]
        public Boolean Pay_die_member_save(String wsPass, String application, String XmlMain, string pbl, string UserName, string cs_type)
        {
            return new WalfareService(wsPass, application).Pay_die_member_save(XmlMain, pbl, UserName, cs_type);
        }

        [WebMethod]
        public Boolean Update_Fee_Year(String wsPass, String application, String XmlMain, string pbl, string cs_type, string branch_id)
        {
            return new WalfareService(wsPass, application).Update_Fee_Year(XmlMain, pbl, cs_type, branch_id);
        }

        [WebMethod]
        public Boolean Update_Coopbranch(String wsPass, String application, String XmlMain, string pbl, string cs_type, string branch_id)
        {
            return new WalfareService(wsPass, application).Update_Coopbranch(XmlMain, pbl, cs_type, branch_id);
        }

        [WebMethod]
        public Boolean BranchDoc_New(String wsPass, String application, String XmlMain, string pbl, string cs_type, string document_code)
        {
            return new WalfareService(wsPass, application).BranchDoc_New(XmlMain, pbl, cs_type, document_code);
        }

        [WebMethod]
        public Boolean Memo_Admin_Save(String wsPass, String application, String XmlMain, string pbl, string UserName, string cs_type, string branch_id)
        {
            return new WalfareService(wsPass, application).Memo_Admin_Save(XmlMain, pbl, UserName, cs_type, branch_id);
        }

        [WebMethod]
        public Boolean SaveGroupPaidCancel(String wsPass, String application, string pbl, String xmlMain, string entry_id, string branch_id, DateTime inform_date, int period)
        {
            return new WalfareService(wsPass, application).SaveGroupPaidCancel(pbl, xmlMain, entry_id, branch_id, inform_date, period);
        }

        [WebMethod]
        public Boolean GenStatement(String wsPass, String application, DateTime deptopen_date, decimal prncbal, String groupBranch)
        {
            return new WalfareService(wsPass, application).GenStatement(deptopen_date, prncbal, groupBranch);
        }

        [WebMethod]
        public Boolean GenCodept(String wsPass, String application, DateTime deptopen_date, String groupBranch)
        {
            return new WalfareService(wsPass, application).GenCodept(deptopen_date, groupBranch);
        }

        [WebMethod]
        public Boolean StatusMem(String wsPass, String application, string deptaccount_no, DateTime deptclose_tdate, DateTime die_tdate, string resigncause_code, decimal reqchg_status, string branch_id, string period, string user, string cs_type, string dpreqchg_doc, string for_year)
        {
            return new WalfareService(wsPass, application).StatusMem(deptaccount_no, deptclose_tdate, die_tdate, resigncause_code, reqchg_status, branch_id, period, user, cs_type, dpreqchg_doc, for_year);
        }

        [WebMethod]
        public bool CancelApprovePaid(String wsPass, String application, string deptaccount_no, string branch_id, string slip_no, string period, string entry_id, decimal slip_amt, string Seffective_date)
        {
            return new WalfareService(wsPass, application).CancelApprovePaid(deptaccount_no, branch_id, slip_no, period, entry_id, slip_amt, Seffective_date);
        }

        [WebMethod]
        public Boolean GenNewCsType(String wsPass, String application, string cs_type)
        {
            return new WalfareService(wsPass, application).GenNewCsType(cs_type);
        }
    }
}
