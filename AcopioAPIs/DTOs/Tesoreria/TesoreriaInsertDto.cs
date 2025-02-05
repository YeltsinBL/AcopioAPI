﻿using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Tesoreria
{
    public class TesoreriaInsertDto: InsertDto
    {
        public int LiquidacionId { get; set; }
        public DateOnly TesoreriaFecha { get; set; }
        public decimal TesoreriaMonto { get; set; }
        public decimal TesoreriaPendientePagar { get; set; }
        public decimal TesoreriaPagado { get; set; }
        public required List<TesoreriaDetallePagoInsertDto> TesoreriaDetallePagos { get; set; }
    }
}
