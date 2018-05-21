using LiveCharts.Contracts;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveCharts.Data
{
    public class ChartsDataNotification
    {

        private readonly static ChartsDataNotification _chartsDataNotification = new ChartsDataNotification(new ChangeNotificationCallback());
        private string _connString = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        private static string _baseSqlString = "select Process_ID, INTERFACE_ID, RUN_INSTANCE_ID, START_TIME, END_TIME, STATUS, MESSAGE from INTERFACE_RUN_TIMES";
        private static string _filterSqlString = _baseSqlString + " where Process_ID > 1";

        protected ChangeNotificationCallback changeNotificationCallback { get; private set; }

        private bool isRunning;

        private string oracleDependencyId;

        public ChartsDataNotification(ChangeNotificationCallback changeNotificationCallback)
        {
            this.changeNotificationCallback = changeNotificationCallback;
        }

        public bool IsRunning()
        {
            return isRunning;
        }

        public static ChartsDataNotification Instance()
        {
            return _chartsDataNotification;
        }

        public void StartService()
        {

            if (isRunning == false)
            {
                using (OracleConnection conn = new OracleConnection(_connString))
                using (OracleCommand cmd = new OracleCommand(_filterSqlString, conn))
                {
                    conn.Open();

                    try
                    {
                        cmd.AddRowid = true;
                        OracleDependency oraDep = new OracleDependency(cmd);
                        cmd.Notification.IsNotifiedOnce = false;

                        oraDep.OnChange += new OnChangeEventHandler(OnDBNotificationHandler);

                        oracleDependencyId = oraDep.Id;

                        OracleDataAdapter dataAdapter = new OracleDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet, "INTERFACE_RUN_TIMES");
                        isRunning = true;
                            
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }


        public void StopService()
        {
            if (isRunning)
            {
                OracleDependency oraDep = OracleDependency.GetOracleDependency(oracleDependencyId);
                if (oracleDependencyId == null)
                {
                    throw new Exception("Unknown oracle dep");
                }

                using (OracleConnection conn = new OracleConnection(_connString))
                {

                    conn.Open();
                    try
                    {
                        oraDep.RemoveRegistration(conn);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                isRunning = false;
            }
        }

        public void OnDBNotificationHandler(object src, OracleNotificationEventArgs args)
        {
            List<StepInstance> changedData = new List<StepInstance>();

            //collect all rowIds that changed
            List<string> rowIdList = new List<string>();
            foreach (DataRow row in args.Details.Rows)
            {
                rowIdList.Add(string.Format("'{0}'", row["Rowid"].ToString()));
            }


            string query = string.Format("select Process_ID, INTERFACE_ID, RUN_INSTANCE_ID, START_TIME, END_TIME, STATUS, MESSAGE from INTERFACE_RUN_TIMES WHERE rowid IN ({0})", string.Join(",", rowIdList));

            using (OracleConnection conn = new OracleConnection(_connString))
            using (OracleCommand cmd = new OracleCommand(query, conn))
            {
                conn.Open();

                try
                {
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            var processIdCol = dr.GetOrdinal("Process_ID");
                            var interfaceIdCol = dr.GetOrdinal("INTERFACE_ID");
                            var runInstanceIdCol = dr.GetOrdinal("RUN_INSTANCE_ID");
                            var startTimeCol = dr.GetOrdinal("START_TIME");
                            var endTimeCol = dr.GetOrdinal("END_TIME");
                            var statusCol = dr.GetOrdinal("STATUS");
                            var messageCol = dr.GetOrdinal("MESSAGE");

                            while (dr.Read())
                            {
                                var stepInstance = new StepInstance();
                                stepInstance.ParentRunInstanceId = dr.GetInt64(runInstanceIdCol);
                                stepInstance.Id = dr.GetInt64(interfaceIdCol);

                                if (dr.IsDBNull(startTimeCol) == false)
                                {
                                    stepInstance.StartTime = dr.GetDateTime(startTimeCol);
                                }
                                
                                if(dr.IsDBNull(endTimeCol) == false)
                                {
                                    stepInstance.EndTime = dr.GetDateTime(endTimeCol);
                                }

                                stepInstance.Status = dr.IsDBNull(statusCol) ? string.Empty : dr.GetString(statusCol);
                                stepInstance.Message = dr.IsDBNull(messageCol) ? string.Empty : dr.GetString(messageCol);

                                changedData.Add(stepInstance);
                            }
                        }
                    }
                }
                catch
                { }
                finally
                {
                    conn.Close();
                }
            }

            changeNotificationCallback.NotifyItemChanged(changedData.ToArray());
        }

        public ChangeNotificationCallback GetChangeNotificationCallback()
        {
            return changeNotificationCallback;
        }
    }
}
