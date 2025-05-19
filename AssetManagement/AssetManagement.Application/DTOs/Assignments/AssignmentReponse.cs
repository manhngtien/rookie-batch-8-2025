using AssetManagement.Application.Helpers.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Application.DTOs.Assignments;
public class AssignmentReponse
{
    public int Id { get; set; }
    public string State { get; set; }
    public DateTime AssignedDate { get; set; }
    public string AssetCode { get; set; }
    public string AssetName { get; set; }
    public string AssignedBy { get; set; }
    public string AssignedTo { get; set; }
    public string Note { get; set; }
}


