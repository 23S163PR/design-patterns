using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace patterns_playground
{
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
}
