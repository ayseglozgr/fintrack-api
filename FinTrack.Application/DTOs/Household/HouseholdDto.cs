using System;
using System.Collections.Generic;
using System.Text;

namespace FinTrack.Application.DTOs.Household
{
    public class HouseholdDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
    }
}
