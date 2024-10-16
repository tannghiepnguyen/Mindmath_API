﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mindmath.Repository.Constant;
using Mindmath.Service.IService;
using Mindmath.Service.ProblemTypes.DTO;

namespace Mindmath.API.Controllers
{
	[Route("api/topics/{topicId:guid}/problem-types")]
	[ApiController]
	public class ProblemTypesController : ControllerBase
	{
		private readonly IServiceManager serviceManager;

		public ProblemTypesController(IServiceManager serviceManager)
		{
			this.serviceManager = serviceManager;
		}

		[HttpGet]
		[Authorize(Roles = Roles.Admin)]
		public async Task<IActionResult> GetProblemTypes(Guid topicId)
		{
			var problemTypes = await serviceManager.ProblemTypeService.GetProblemTypes(topicId, trackChange: false);
			return Ok(problemTypes);
		}

		[HttpGet("active")]
		[Authorize(Roles = Roles.Teacher)]
		public async Task<IActionResult> GetActiveProblemTypes(Guid topicId)
		{
			var problemTypes = await serviceManager.ProblemTypeService.GetActiveProblemTypes(topicId, trackChange: false);
			return Ok(problemTypes);
		}

		[HttpGet("{problemTypeId:guid}", Name = "ProblemTypeById")]
		[Authorize(Roles = Roles.Teacher)]
		public async Task<IActionResult> GetProblemType(Guid topicId, Guid problemTypeId)
		{
			var problemType = await serviceManager.ProblemTypeService.GetProblemType(topicId, problemTypeId, trackChange: false);
			return Ok(problemType);
		}

		[HttpPost]
		[Authorize(Roles = Roles.Admin)]
		public async Task<IActionResult> AddProblemType(Guid topicId, [FromBody] ProblemTypeForCreationDto problemTypeForCreation)
		{
			if (problemTypeForCreation is null) return BadRequest();
			var problemType = await serviceManager.ProblemTypeService.CreateProblemType(topicId, problemTypeForCreation, trackChange: false);
			return CreatedAtRoute("ProblemTypeById", new { problemTypeId = problemType.Id, topicId }, problemType);
		}

		[HttpPut("{problemTypeId:guid}")]
		[Authorize(Roles = Roles.Admin)]
		public async Task<IActionResult> UpdateProblemType(Guid topicId, Guid problemTypeId, [FromBody] ProblemTypeForUpdateDto problemTypeForUpdate)
		{
			await serviceManager.ProblemTypeService.UpdateProblemType(topicId, problemTypeId, problemTypeForUpdate, topicTrackChange: false, problemTypeTrackChange: true);
			return NoContent();
		}


	}
}
