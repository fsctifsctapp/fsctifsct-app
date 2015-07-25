using System;

namespace Saving.CmConfig
{

    public enum PermissType
    {
        /// <summary>
        /// ไม่มีสิทธิ์อ่านข้อมูล, เปิดหน้าจอ, ดึงข้อมูล
        /// </summary>
        ReadDeny = 1,
        /// <summary>
        /// ไม่มีสิทธิ์เขียนข้อมูล, บันทึกข้อมูล, แก้ไขข้อมูล
        /// </summary>
        WriteDeny = 2,
        /// <summary>
        /// ไม่มีสิทธิ์ใช้งาน, ยังไม่ได้ Login
        /// </summary>
        LoginDeny = 3,
    }
}
