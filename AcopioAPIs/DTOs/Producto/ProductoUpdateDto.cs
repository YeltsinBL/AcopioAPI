﻿using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Producto
{
    public class ProductoUpdateDto: UpdateDto
    {
        public int ProductoId { get; set; }
        public required string ProductoNombre { get; set; }
    }
}
