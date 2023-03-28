using System;

namespace DigitalLibary.WebApi.Payload
{
    public class VESModel
    {
        public Guid? Id { get; set; }
        public string? MediaTitle { get; set; }
        public string? MediaPath { get; set; }
        public string? MediaDescription { get; set; }
        public int? MediaLinkType { get; set; }
        public int? MediaType { get; set; }
        public int? Number { get; set; }
        public Guid? IdGroupVes { get; set; }
        public string? FileNameExtention { get; set; }

        public Guid? IdFile { get; set; }

    }
}
