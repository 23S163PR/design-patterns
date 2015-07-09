using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace patterns-playground
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

    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client(new DbAccessorFactoryProvider().GetFactory());



            Singleton.Instance.Do();
            Singleton.Instance.Do();
            Singleton.Instance.Do();

            var ctor = typeof(Singleton).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], null);
            var newSingletonInstance = ctor.Invoke(null);

            var instanceField = typeof(Singleton).GetField("instance", BindingFlags.Static | BindingFlags.NonPublic);
            instanceField.SetValue(Singleton.Instance, newSingletonInstance);

            Singleton.Instance.Do();
            Singleton.Instance.Do();

            var builder = new PersonBuilder();
            var person = builder
                .WithId(1)
                .WithName("Alex")
                .WithDateOfBirth(new DateTime(1987, 12, 12))
                .CreateDirector()
                .WithSalary(100000m)
                .Build();

            var etalon = new WeightManager
            {
                Height = 150,
                TotalWeightMoved = 1000
            };

            var clone = (WeightManager)etalon.Clone();
            clone.TotalWeightMoved = 2000;
            clone.Name = "Ivan Kruzestern";
        }
    }

    public class Person
    {
        public int Identifier { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class Director : Person
    {
        public decimal Salary { get; set; }
    }

    public class WeightManager : Person, ICloneable
    {
        public int TotalWeightMoved { get; set; }
        public int Height { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public enum Title
    {
        Director,
        NonDirector
    }

    public class PersonBuilder
    {
        protected Person person;

        public PersonBuilder()
        {
            person = new Person();
        }

        public DirectorBuilder CreateDirector()
        {
            return new DirectorBuilder(person);
        }

        public PersonBuilder WithId(int id)
        {
            person.Identifier = id;
            return this;
        }

        public PersonBuilder WithName(string name)
        {
            person.Name = name;
            return this;
        }

        public PersonBuilder WithDateOfBirth(DateTime birthDate)
        {
            person.BirthDate = birthDate;
            return this;
        }

        public Person Build()
        {
            return person;
        }
    }

    public class DirectorBuilder : PersonBuilder
    {
        public DirectorBuilder(Person person)
        {
            this.person = new Director
            {
                Identifier = person.Identifier,
                Name = person.Name,
                BirthDate = person.BirthDate
            };
        }

        public DirectorBuilder WithSalary(decimal salary)
        {
            ((Director)person).Salary = salary;
            return this;
        }
    }



    public class Singleton
    {
        private static readonly Singleton instance;
        public static Singleton Instance { get { return instance; } }

        private int doCounter = 0;

        static Singleton()
        {
            instance = new Singleton();
        }

        private Singleton()
        { }

        public void Do()
        {
            doCounter++;
            Console.WriteLine("Singleton.Do:: doCounter = {0}", doCounter);
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