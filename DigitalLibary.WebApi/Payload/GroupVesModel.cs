using System;

namespace DigitalLibary.WebApi.Payload
{
    public class GroupVesModel
    {
        public Guid? Id { get; set; }
        public string? GroupName { get; set; }
        public Guid? IdcategoryVes { get; set; }
    }
}
