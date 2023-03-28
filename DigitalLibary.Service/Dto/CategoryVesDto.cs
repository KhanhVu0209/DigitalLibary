using System;

namespace DigitalLibary.Service.Dto
{
    public class CategoryVesDto
    {
        public Guid Id { get; set; }
        public string? CategoryVesName { get; set; }
        public string? CategoryVesCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public bool? IsHide { get; set; }
    }
}
