using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DTOs;
using WebApp.Models;
using WebApp.Models.NtoNClasses;
using WebApp.Repository;

namespace WebApp.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SafetyDocsController : ControllerBase
  {

    private readonly DataDBContext data;


    public SafetyDocsController( DataDBContext context)
    {
      data = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
      List<SafetyDocDataDTO> listaDokumenata = new List<SafetyDocDataDTO>();
      foreach(SafetyDoc item in data.SafetyDocs)
      {
        SafetyDocDataDTO sd = new SafetyDocDataDTO();
        sd.Type = item.Type;
        //sd.WorkPlanId = item.WorkPlan.Id;
        sd.Status = item.Status;
        sd.Details = item.Details;
        sd.Notes = item.Notes;
        sd.DateCreated = item.DateCreated;
        sd.OperationsCompleted = item.OperationsCompleted;
        sd.TagsRemoved = item.TagsRemoved;
        sd.GroundingRemoved = item.GroundingRemoved;
        sd.Ready = item.Ready;

        listaDokumenata.Add(sd);
      }

      return Ok(new { listaDokumenata });
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] SafeteyDocDTO body)
    {
     
        SafetyDoc sDoc = new SafetyDoc();
        sDoc.Type = body.SafetyDoc.Type;
     // sDoc.WorkPlan = data.WorkPlans.FirstOrDefault(x => x.Id == body.SafetyDoc.WorkPlanId);
        sDoc.Status = body.SafetyDoc.Status;
        sDoc.UserID = "id3";
        sDoc.Details = body.SafetyDoc.Details;
        sDoc.Notes = body.SafetyDoc.Notes;
        sDoc.DateCreated = DateTime.Now;
        sDoc.OperationsCompleted = body.SafetyDoc.OperationsCompleted;
        sDoc.TagsRemoved = body.SafetyDoc.TagsRemoved;
        sDoc.GroundingRemoved = body.SafetyDoc.GroundingRemoved;
        sDoc.Ready = body.SafetyDoc.Ready;

        foreach(DeviceDTO item in body.Devices)
        {
          await data.SafetyDocsDevices.AddAsync(new SafetyDocDevice { SafetyDoc = sDoc, Device = data.Devices.FirstOrDefault(x => x.Name == item.Name) });
        }
        await data.SafetyDocs.AddAsync(sDoc);
        await data.SaveChangesAsync();
        return Ok();
    
      
    }


    [HttpGet]
    [Route("GetSafetyDocIds")]
    public IActionResult GetSafetyDocIds()
    {
      List<int> lista = new List<int>();
      foreach(SafetyDoc item in data.SafetyDocs)
      {
        lista.Add(item.Id);
      }

      return Ok(new { lista });
    }

  }

}
