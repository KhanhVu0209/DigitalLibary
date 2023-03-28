using System;

namespace DigitalLibary.Service.Dto
{
    public class GroupVesDto
    {
        public Guid Id { get; set; }
        public string? GroupName { get; set; }
        public string? GroupCode { get; set; }
        public Guid? IdcategoryVes { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public bool? IsHide { get; set; }
    }
}
