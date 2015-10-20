using System;

namespace Radial.UnitTest.Persist.Nhs.Domain
{
    public class Order
    {
        public virtual int Id { get; set; }

        public virtual  decimal Amount { get; set; }
        //多出来的字段
        public virtual DateTime Time { get; set; }

        public virtual bool IsDelete { get; set; }
    }
}
