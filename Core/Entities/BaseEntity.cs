using System;
using System.Collections.Generic;
using System.Text;


// Don't have to add the Id property to every class 
// can be inherited instead
namespace Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }

    }
}
