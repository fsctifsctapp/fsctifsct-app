using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.Web;

namespace CommonLibrary
{
    public class AdminWebState
    {
        private WebState state;
        private List<HttpSessionState> Sessions;
        private HttpApplicationState Application;

        public AdminWebState(WebState state)
        {
            this.state = state;
            this.Application = state.GetApplicationState();
            try
            {
                Sessions = Application["Sessions"] as List<HttpSessionState>;
            }
            catch { }
            bool isAvaliable = false;
            try
            {
                for (int i = 0; i < Sessions.Count; i++)
                {
                    bool isEquals = Sessions[i].SessionID == state.GetSessionState().SessionID;
                    if (isEquals)
                    {
                        isAvaliable = true;
                        break;
                    }
                }
            }
            catch { }
            if (!isAvaliable)
            {
                if (Sessions == null)
                {
                    Sessions = new List<HttpSessionState>();
                }
                Sessions.Add(state.GetSessionState());
            }
            for (int i = (Sessions.Count - 1); i >= 0; i--)
            {
                if (Sessions[i].Count == 0)
                {
                    Sessions.RemoveAt(i);
                }
            }
            Application["Sessions"] = Sessions;
        }
    }
}