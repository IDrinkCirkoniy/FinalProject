using System;
using System.Collections.Generic;

namespace ShoeClassLibrary.Models;

public partial class Producer
{
    public int ProducerId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Shoe> Shoes { get; set; } = new List<Shoe>();
}
