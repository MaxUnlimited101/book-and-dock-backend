using System;

namespace Backend.Entities
{
    public class PaymentMethod : BaseEntity
    {
        public string Name { get; set; }

        public PaymentMethod(string name)
        {
            Name = name;
        }
    }
}