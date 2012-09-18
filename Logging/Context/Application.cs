using System;
using System.Collections.Generic;

namespace Common.Logging.Context
{
    public class Application
    {
        public static readonly Application IMPROVEMENT_PORTAL = new Application("IP", "Improvement Portal");
        public static readonly Application IDENTITY_ADMIN = new Application("IA", "Identity Admin");
        public static readonly Application PGO = new Application("PG", "Press Ganey Online");
        public static readonly Application IDENTITY = new Application("ID", "Press Ganey Identity");
        public static readonly Application IDENTITY_TRICKLE_SYNC = new Application("TS", "Identity Trickle Sync");
        public static readonly Application PGO_QUICKREPORTS = new Application("PQ", "PGO Quick Reports");

        public static Dictionary<String, Application> AllApplications = new Dictionary<String, Application>();

        static Application()
        {
            AllApplications.Add(IMPROVEMENT_PORTAL.Key, IMPROVEMENT_PORTAL);
            AllApplications.Add(IDENTITY_ADMIN.Key, IDENTITY_ADMIN);
            AllApplications.Add(PGO.Key, PGO);
            AllApplications.Add(IDENTITY.Key, IDENTITY);
            AllApplications.Add(IDENTITY_TRICKLE_SYNC.Key, IDENTITY_TRICKLE_SYNC);
            AllApplications.Add(PGO_QUICKREPORTS.Key, PGO_QUICKREPORTS);
        }

        private Application(string key, string description)
        {
            Key = key;
            Desc = description;
        }

        public string Key { get; set; }
        public string Desc { get; set; }
    }
}
