using System;

namespace DigitalLibary.Data.Entity
{
    public class VES
    {
        public Guid Id { get; set; }
        public string? MediaTitle { get; set; }
        public string? MediaPath { get; set; }
        public string? MediaDescription { get; set; }
        public string? FileNameExtention { get; set; }
        public string? FileNameDocument { get; set; }
        public string? FileAvatarExtention { get; set; }
        public string? Avatar { get; set; }
        public int? MediaLinkType { get; set; }
        public int? MediaType { get; set; }
        public int? Status { get; set; }
        public int? Number { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsHide { get; set; }
        public Guid? IdGroupVes { get; set; }
    }
}
