using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBAccess;

namespace GcoopServiceCs
{
    /*public enum DocumentTypeCode
    {
        ACCTRNPAY = 0,
        ADJREQNO = 1,
        ADVCHQDEPT = 2,
        AMSECURITY = 3,
        APVCANCEL = 4,
        AUDITLOAN = 5,
        AUDITMEMBER = 6,
        BINFORMDEV = 7,
        CARMASTNO = 8,
        CHGPERIODDOCNO = 9,
        CMASSTED = 10,
        CMDBUYNO = 11,
        CMDDISOFFNO = 12,
        CMDEMPNO = 13,
        CMDGOODSID = 14,
        CMDLOTDOCNO = 15,
        CMDMOVENO = 16,
        CMDPLEANO = 17,
        CMDPODOCNO = 18,
        CMDPRODUCT = 19,
        CMDPROID = 20,
        CMDRCVDOCNO = 21,
        CMDREPAIRNO = 22,
        CMDREPANO = 23,
        CMDRVKDOCNO = 24,
        CMDSEQDEP = 25,
        CMDSEQLOCNO = 26,
        CMDSEQNO = 27,
        CMDSLIPNO = 28,
        CMDSODOCNO = 29,
        CMDSQDOCNO = 30,
        CMDSTKDOCNO = 31,
        CMSHLNRETRCV = 32,
        CMSHRWTDNO = 33,
        CMSLIPNO = 34,
        CMSLIPPAYNO = 35,
        CMSLIPRECEIPT = 36,
        CMVOUCHERNO = 37,
        CMVOUCHERNO_JV = 38,
        CMVOUCHERNO_PV = 39,
        CMVOUCHERNO_RV = 40,
        CM_APVPROCESS_ID = 41,
        COLCHGDOCNO = 42,
        COLLDETAILNO = 43,
        COLLMAST01 = 44,
        COLLMASTER = 45,
        CONTADJDOCNO = 46,
        CONTNO00 = 47,
        CONTNO01 = 48,
        CONTNO02 = 49,
        CONTNO03 = 50,
        CONTNO04 = 51,
        CONTNO05 = 52,
        CONTNO06 = 53,
        CONTNO07 = 54,
        CONTNO08 = 55,
        CONTNO09 = 56,
        CONTNO10 = 57,
        CONTNO20 = 58,
        CONTNO21 = 59,
        CONTNO22 = 60,
        CONTNO23 = 61,
        CONTNO28 = 62,
        CONTNO30 = 63,
        CONTPROBLEMSET = 64,
        CREDITDOCNO = 65,
        DPACCDOCNO10 = 66,
        DPACCDOCNO11 = 67,
        DPACCDOCNO12 = 68,
        DPACCDOCNO13 = 69,
        DPACCDOCNO14 = 70,
        DPACCDOCNO15 = 71,
        DPACCDOCNO21 = 72,
        DPACCDOCNO24 = 73,
        DPACCDOCNO31 = 74,
        DPACCDOCNO36 = 75,
        DPACCDOCNO48 = 76,
        DPACCDOCNO50 = 77,
        DPACCDOCNO52 = 78,
        DPACCDOCNO60 = 79,
        DPACCDOCNO61 = 80,
        DPACCDOCNO91 = 81,
        DPACCOUNTNO92 = 82,
        DPAPPLDOCNO = 83,
        DPCHGDOCNO = 84,
        DPCUTSTM = 85,
        DPEXTRAMEM = 86,
        DPPROCESSNO = 87,
        DPREQCHG = 88,
        DPREQSEQUEST = 89,
        DPSLIPNO = 90,
        EXECENROLLMENT = 91,
        EXECPROJECTNO = 92,
        FINCASHSLIPNO = 93,
        FINRECIEPTRECV = 94,
        FNCONTRLCASH = 95,
        FNMONEYORDER = 96,
        FNPAYSLIPNO = 97,
        FNRECEIPTBANK = 98,
        FNRECEIPTNO = 99,
        FNRECEIVENO = 100,
        FNTAXATPAY = 101,
        HRACTIVITY = 102,
        HRASSESS = 103,
        HRCANDIDATE = 104,
        HRDISCHANG = 105,
        HRDRAWEDUA = 106,
        HRDRAWHOSP = 107,
        HREDU = 108,
        HREMPLFILEMAS = 109,
        HREMPLOYEE = 110,
        HREXPR = 111,
        HRFAMILY = 112,
        HRHEALTH = 113,
        HRHELPSUN = 114,
        HRHOLDASSET = 115,
        HRLEAV = 116,
        HRLIFE = 117,
        HRLOANEMP = 118,
        HRLOANHOME = 119,
        HRPARALL = 120,
        HRPENALTY = 121,
        HRREQTOT = 122,
        HRSERMINAR = 123,
        HRTRANSFER = 124,
        HRWORKDATE = 125,
        INSAPPLDOCNO = 126,
        INSCLDOCNO = 127,
        INSDOCNO = 128,
        KPRECEIPTNO = 129,
        LNPAUSELOAN = 130,
        LNRECEIVENO = 131,
        LNREQUESDOCNO = 132,
        LNREQUESDOCNO00 = 133,
        LNREQUESDOCNO01 = 134,
        LNREQUESDOCNO02 = 135,
        LNREQUESDOCNO03 = 136,
        LNREQUESDOCNO04 = 137,
        LNREQUESDOCNO30 = 138,
        LNRETURNDOCNO = 139,
        MBAPPLDOCNO = 140,
        MBCHANGDOCNO = 141,
        MBCHGDOCNO = 142,
        MBCHGGROUP = 143,
        MBDIVSLIPPAYOUT = 144,
        MBGAINID = 145,
        MBMEMBERCONO = 146,
        MBMEMBERNO = 147,
        MBMEMCLOSE = 148,
        MBRSGDOCNO = 149,
        OLORGNO = 150,
        PAUSELOANNO = 151,
        RCVBKDOCNO = 152,
        SETINTARRNO = 153,
        SLCOMPOUND = 154,
        SLSLIPADJUST = 155,
        SLSLIPPAYIN = 156,
        SLSLIPPAYOUT = 157,
        TRANSLOANCOLL = 158,
        WFAPPLDOCNO = 159,
        WFBOOKDOCNO = 160,
        WFCHGDOCNO = 161,
        WFCRECEIVENO = 162,
        WFGENMEMNO = 163,
        WFHPMEMBER = 164,
        WFSLIPDOCNO = 165,
        vcvoucher = 166
    } */

    public class DocumentControlBranch
    {
        //public String NewDocumentNo(DocumentTypeCode docTypeCode, int year, String connectionString)
        //{
        //    return NewDocumentNo(docTypeCode.ToString(), year, connectionString);
        //}

        public String NewDocumentNo(String docTypeCode, String branch_id, String cs_type, int year, String connectionString)
        {
            Sta ta = new Sta(connectionString);
            String newDoc = "";
            try
            {
                newDoc = this.NewDocumentNo(docTypeCode, branch_id, cs_type, year, ta);
                ta.Close();
            }
            catch (Exception ex)
            {
                ta.Close();
                throw ex;
            }
            return newDoc;
        }

        //public String NewDocumentNo(DocumentTypeCode docTypeCode, int year, Sta ta)
        //{
        //    return this.NewDocumentNo(docTypeCode.ToString(), year, ta);
        //}

        public String NewDocumentNo(String docTypeCode, String branch_id, String cs_type, int year, Sta ta)
        {
            return this.NewDocumentNo(docTypeCode, branch_id, cs_type, year, ta, true);
        }

        private String NewDocumentNo(String docTypeCode, String branch_id, String cs_type, int year, Sta ta, bool isNext)
        {
            String resu = "";
            string sql = "select * from CMBRANCHDOCCONTROL where document_code = '" + docTypeCode + "' and branch_id = '" + branch_id + "' and cs_type = '" + cs_type + "' ";
            Sdt dt = ta.Query(sql);
            if (dt.Next())
            {
                int lastDoc = dt.GetInt32("last_documentno");
                String format = dt.GetString("document_format");
                String prefix = dt.GetString("document_prefix");
                int clearType = dt.GetInt32("clear_type");
                if (clearType == 1)
                {
                    String bb = "", pp = "", rr = "", yy = "", mm = "", dd = "";
                    char[] f = format.ToCharArray();
                    char[] n = new char[f.Length];
                    int ni = 0;
                    char cNow = 'Z';
                    char cEnd = 'Z';
                    for (int i = 0; i < f.Length; i++)
                    {
                        if (f[i].ToString().ToUpper() == "B")
                        {
                            bb += "B";
                        }
                        else if (f[i].ToString().ToUpper() == "P")
                        {
                            pp += "P";
                        }
                        else if (f[i].ToString().ToUpper() == "R")
                        {
                            rr += "R";
                        }
                        else if (f[i].ToString().ToUpper() == "Y")
                        {
                            yy += "Y";
                        }
                        else if (f[i].ToString().ToUpper() == "M")
                        {
                            mm += "M";
                        }
                        else if (f[i].ToString().ToUpper() == "D")
                        {
                            dd += "D";
                        }
                        cNow = f[i];
                        if (cNow != cEnd)
                        {
                            cEnd = f[i];
                            n[ni] = f[i];
                            ni++;
                        }
                    }
                    String newFormatFull = "";
                    for (int i = 0; i < ni; i++)
                    {
                        if (n[i].ToString().ToUpper() == "B")
                        {
                            newFormatFull += "000";
                        }
                        else if (n[i].ToString().ToUpper() == "P")
                        {
                            newFormatFull += prefix;
                        }
                        else if (n[i].ToString().ToUpper() == "R")
                        {
                            String format0 = rr.Replace('R', '0');
                            if (isNext)
                            {
                                lastDoc++;
                                ta.Exe("update CMBRANCHDOCCONTROL set last_documentno = " + lastDoc + " where document_code = '" + docTypeCode + "' and branch_id = '" + branch_id + "' and cs_type = '" + cs_type + "' ");
                            }
                            newFormatFull += lastDoc.ToString(format0);
                        }
                        else if (n[i].ToString().ToUpper() == "Y")
                        {
                            String yyyy = year.ToString();
                            newFormatFull += yyyy.Substring(yyyy.Length - yy.Length);
                        }
                        else if (n[i].ToString().ToUpper() == "M")
                        {
                            newFormatFull += "00";
                        }
                        else if (n[i].ToString().ToUpper() == "D")
                        {
                            newFormatFull += "00";
                        }
                    }
                    resu = newFormatFull;
                }
                else
                {
                    resu = lastDoc.ToString();
                }
            }
            else
            {
                throw new Exception("ไม่พบรหัสเอกสาร " + docTypeCode);
            }
            return resu;
        }
    }
}
