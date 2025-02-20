﻿using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Distribuidor
{
    public class DistribuidorUpdateDto: UpdateDto
    {
        public int DistribuidorId { get; set; }
        public required string DistribuidorRuc { get; set; }
        public required string DistribuidorNombre { get; set; }
    }
}
