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

namespace patterns_playground
{
    class Program
    {
        static void Main(string[] args)
        {
            //var order = new Order
            //{
            //    Products = new List<string> { "Oranges", "Apples" }
            //};

            //var service = new OrderService();
            //service.PlaceOrder(order);

            //var client = new Client(new ConfigurationBasedFactoryProvider().GetFactory());

            //Singleton.Instance.Do();
            //Singleton.Instance.Do();
            //Singleton.Instance.Do();

            //var ctor = typeof(Singleton).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], null);
            //var newSingletonInstance = ctor.Invoke(null);

            //var instanceField = typeof(Singleton).GetField("instance", BindingFlags.Static | BindingFlags.NonPublic);
            //instanceField.SetValue(Singleton.Instance, newSingletonInstance);

            //Singleton.Instance.Do();
            //Singleton.Instance.Do();

            //var builder = new PersonBuilder();
            //var person = builder
            //    .WithId(1)
            //    .WithName("Alex")
            //    .WithDateOfBirth(new DateTime(1987, 12, 12))
            //    .CreateDirector()
            //    .WithSalary(100000m)
            //    .Build();

            //var etalon = new WeightManager
            //{
            //    Height = 150,
            //    TotalWeightMoved = 1000
            //};

            //var clone = (WeightManager)etalon.Clone();
            //clone.TotalWeightMoved = 2000;
            //clone.Name = "Ivan Kruzestern";

            ISwitchable lamp = new Light();

            ICommand switchClose = new CloseSwitchCommand(lamp);
            ICommand switchOpen = new OpenSwitchCommand(lamp);
            
            Switch @switch = new Switch(switchClose, switchOpen);
        }
    }
}