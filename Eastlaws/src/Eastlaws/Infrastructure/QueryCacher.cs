﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Eastlaws.Infrastructure
{
    public class QueryInfo
    {
        public int ID { get; set; }
        public string Query { get; set; }
        public int ResultsCount { get; set; }
        public string Hash { get; set; }
        public string SecondaryQuery { get; set; }
    }

    public class QueryCacher
    {
        private const int MAX_CACHED_IDS = 100;
        private static Dictionary<string, QueryInfo> m_InMemoryCachedQuries = new Dictionary<string, QueryInfo>();

        public int ID { get; private set; }
        public int ServiceID { get; private set; }
        public string Query { get; private set; }
        public string SearchType  { get; private set; }
        public string QueryHash { get; private set; }
        public int ResultsCount { get; private set; }
        private bool IncreaseSearchCount { get; set; }
        public QueryInfo Info { get; private set; }
        public string SecondaryQuery { get; private set; }
        private bool IsTasfeya { get; set; }

        public QueryCacher(int ServiceID , string Query , string SearchType , bool NewSearch = false , string SecondaryQuery = null , bool IsTasfeya = false)
        {
            this.ServiceID = ServiceID;
            this.Query = Query;
            this.SearchType = SearchType;
            this.IncreaseSearchCount = NewSearch;
            this.SecondaryQuery = SecondaryQuery;
            this.IsTasfeya = IsTasfeya;
            PerformQueryHash();
            this.Info = Execute();
        }

        private void PerformQueryHash()
        {
            using (var md5 = MD5.Create())
            {
                byte[] HashedData = md5.ComputeHash(Encoding.UTF8.GetBytes(this.Query));
                StringBuilder Builder = new StringBuilder(32);
                for (int i = 0; i < HashedData.Length; i++)
                {
                    Builder.Append(HashedData[i].ToString("x2"));
                }
                this.QueryHash = Builder.ToString();
            }
        }

        public QueryInfo Execute()
        {
            Debug.WriteLine("Executed @ " + DateTime.Now.ToString("hh:mm ss fff"));
            string Proc = "QueryCache";
            if (IsTasfeya)
            {
                Proc = "QueryCacheTasfeya";
            }
            SqlConnection con = DataHelpers.GetConnection(DbConnections.Data);
            DynamicParameters Parameters = new DynamicParameters();
            Parameters.Add("@ServiceID", ServiceID);
            Parameters.Add("@Query", Query , DbType.String );
            Parameters.Add("@QueryHash", QueryHash);
            Parameters.Add("@SearchType", SearchType);
            Parameters.Add("@IncreaseHits", IncreaseSearchCount);
            Parameters.Add("@SecondaryQuery", SecondaryQuery);
            DateTime dtStart = DateTime.Now;
            QueryInfo Info = con.QuerySingle<QueryInfo>(Proc, Parameters, null, null, CommandType.StoredProcedure);
            TimeSpan ts = DateTime.Now - dtStart;
            Debug.WriteLine(ts);
            if (Info != null)
            {
                this.ID = Info.ID;
                this.ResultsCount = Info.ResultsCount;
                Info.Hash = QueryHash;
            }
            return Info;
        }

        public string GetCachedQuery(string JoinQuery = "" , string SortCol = " QCR.DefaultRank ")
        {
            return @"Select QCR.ItemID , " + SortCol + " as MyRank From EastlawsUsers..QueryCacheRecords QCR "
                 + "With (NoLock) "
                + JoinQuery
                +"\n" + "Where QCR.MasterID = " + this.ID;
        }









    }
}
