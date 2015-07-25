using System;
using pbservice;

namespace WebService.Processing
{
    public interface Running
    {
        void DisConnect();

        str_progress GetProgress();

        void Run();
    }
}
