using System;

namespace DBAccess
{
    public enum DbType
    {
        /// <summary>
        /// ใช้สำหรับ Oracle 10g ภาษาไทย
        /// </summary>
        Oracle = 0,
        /// <summary>
        /// ใช้สำหรับ Oracle 9i, 10g ภาษาอังกฤษ WE8MSWIN1252
        /// </summary>
        OleDb = 1,
    }
}
