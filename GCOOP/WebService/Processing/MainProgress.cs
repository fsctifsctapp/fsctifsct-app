using System;
using pbservice;
using System.Threading;

namespace WebService.Processing
{
    public class MainProgress
    {
        protected String id;
        protected String application;
        protected String connectString;
        protected str_progress progress;

        
        protected Thread thread;
        private Running running;

        public String ID
        {
            get { return id; }
            set { id = value; }
        }

        public String Application
        {
            get { return application; }
            set { application = value; }
        }

        public str_progress Progress
        {
            get { return progress; }
            set { progress = value; }
        }

        public void Start(string id, string application)
        {
            this.id = id;
            this.application = application;
            thread = new Thread(new ThreadStart(running.Run));
            thread.Start();
        }

        public void Stop()
        {
            try
            {
                thread.Abort();
                thread = null;
            }
            catch
            {
                thread = null;
            }

        }

        protected void SetRunning(Running running)
        {
            this.running = running;
        }
    }
}
