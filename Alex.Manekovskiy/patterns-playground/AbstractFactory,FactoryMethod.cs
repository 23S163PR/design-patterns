﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace patterns_playground
{
    public enum RDBMSType
    {
        Unknown,
        Oracle,
        MySql
    }

    public class ConfigurationBasedFactoryProvider
    {
        public DbAccessorFactory GetFactory(RDBMSType type = RDBMSType.Unknown)
        {
            if (type == RDBMSType.Unknown)
            {
                if (!Enum.TryParse<RDBMSType>(ConfigurationManager.AppSettings["DataAccess:DbType"].ToString(), out type))
                {
                    throw new ConfigurationException("Configuration value \"DataAccess:DbType\" is not valid");
                }
            }

            switch (type)
            {
                case RDBMSType.Oracle:
                    return new OracleDbAccessorFactory();
                case RDBMSType.MySql:
                    return new MySqlDbAccessorFactory();
                default:
                    throw new ArgumentException("Value is not supported", "type");
            }
        }
    }

    public class Client
    {
        private DbAccessorFactory dbAccessorFactory;
        private DbAccessor dbAccessor;

        public Client(DbAccessorFactory factory)
        {
            dbAccessorFactory = factory;
            dbAccessor = dbAccessorFactory.GetDbAccessor();
        }

        public void DoWork()
        {
            foreach (var record in dbAccessor.GetRecords())
            {
                Console.WriteLine(record);
            }
        }
    }

    public interface DbAccessor
    {
        IEnumerable GetRecords();
    }

    public class OracleDbAccessor : DbAccessor
    {
        public IEnumerable GetRecords()
        {
            yield return "Oracle Record 1";
            yield return "Oracle Record 2";
        }
    }

    class MySqlDbAccessor : DbAccessor
    {

        public IEnumerable GetRecords()
        {
            yield return "MySql Record 1";
            yield return "MySql Record 2";
        }
    }

    public interface DbAccessorFactory
    {
        DbAccessor GetDbAccessor();
    }

    public class OracleDbAccessorFactory : DbAccessorFactory
    {
        public DbAccessor GetDbAccessor()
        {
            return new OracleDbAccessor();
        }
    }

    public class MySqlDbAccessorFactory : DbAccessorFactory
    {
        public DbAccessor GetDbAccessor()
        {
            return new MySqlDbAccessor();
        }
    }
}
