using System;
using pbreport;

namespace WebServiceReport.Processing
{
    public interface Running
    {
        void DisConnect();

        str_progress GetProgress();

        void Run();
    }
}
