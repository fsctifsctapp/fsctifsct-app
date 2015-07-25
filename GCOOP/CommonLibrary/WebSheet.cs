using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public interface WebSheet
    {
        void InitJsPostBack();
        void WebSheetLoadBegin();
        void CheckJsPostBack(String eventArg);
        void SaveWebSheet();
        void WebSheetLoadEnd();
    }
}