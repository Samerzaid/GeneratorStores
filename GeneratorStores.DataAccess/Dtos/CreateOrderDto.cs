﻿namespace GeneratorStores.DataAccess.Dtos;

public class CreateOrderDto
{
    public string CustomerId { get; set; }
    public List<int> ProductIds { get; set; }
    public string Status { get; set; } = "Pending"; // 👈 Add this

}


