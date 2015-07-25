using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public interface WebDialog
    {
        void InitJsPostBack();
        void WebDialogLoadBegin();
        void CheckJsPostBack(String eventArg);
        void WebDialogLoadEnd();
    }
}
