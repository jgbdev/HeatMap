using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using HeatMapMonitor_Windows;

namespace HeatMap_Service
{
    public partial class TheService : ServiceBase
    {
        Manager manager;
        bool OK;

        public TheService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            EventLog.Source = ServiceName;
            EventLog.Log = "Application";

            ((ISupportInitialize)(EventLog)).BeginInit();
            if (!EventLog.SourceExists(EventLog.Source))
            {
                EventLog.CreateEventSource(EventLog.Source, EventLog.Log);
            }
            ((ISupportInitialize)(EventLog)).EndInit();

            Logger.OnMessageLogged += Logger_OnMessageLogged;
            Logger.OnErrorLogged += Logger_OnErrorLogged;

            manager = new Manager();
            OK = true;

            if (OK)
            {
                try
                {
                    if (!(OK = manager.Init()))
                    {
                        Logger_OnMessageLogged("Manager failed to initialise!");
                    }
                }
                catch (Exception ex)
                {
                    OK = false;

                    Logger_OnMessageLogged("Caught exception during intialisation: ");
                    Logger_OnMessageLogged(ex.Message);
                    Logger_OnMessageLogged(ex.StackTrace);
                }
            }
        }

        private void Logger_OnErrorLogged(string message)
        {
            EventLog.WriteEntry(message, EventLogEntryType.Error);
            Console.WriteLine(message);
        }

        private void Logger_OnMessageLogged(string message)
        {
            EventLog.WriteEntry(message, EventLogEntryType.Information);
            Console.WriteLine(message);
        }

        protected override void OnStop()
        {
            try
            {
                if (!(OK = manager.Shutdown()))
                {
                    Logger_OnMessageLogged("Manager failed to shutdown!");
                }
            }
            catch (Exception ex)
            {
                OK = false;

                Logger_OnMessageLogged("Caught exception during shutdown: ");
                Logger_OnMessageLogged(ex.Message);
                Logger_OnMessageLogged(ex.StackTrace);
            }
        }
    }
}
