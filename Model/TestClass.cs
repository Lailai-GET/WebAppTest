using System;
using System.Security.Principal;

namespace WebAppTest.Model
{
    public class TestClass
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public TestClass(Guid guid, string name, string description)
        {
            Id = guid;
            Name = name;
            Description = description;
            if (Id == Guid.Empty) Id = Guid.NewGuid();
        }

        public TestClass()
        {
            if (Id == Guid.Empty) Id = Guid.NewGuid();
        }
    }
}
