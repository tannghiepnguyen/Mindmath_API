﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mindmath.Repository.Constant;
using Mindmath.Repository.Parameters;
using Mindmath.Service.IService;
using Mindmath.Service.Topics.DTO;
using System.Text.Json;

namespace Mindmath.API.Controllers
{
	[Route("api/chapters/{chapterId:guid}/topics")]
	[ApiController]
	public class TopicsController : ControllerBase
	{
		private readonly IServiceManager serviceManager;

		public TopicsController(IServiceManager serviceManager)
		{
			this.serviceManager = serviceManager;
		}

		[HttpPost]
		[Authorize(Roles = Roles.Admin)]
		public async Task<IActionResult> Add([FromBody] TopicForCreationDto topicForCreationDto, [FromRoute] Guid chapterId)
		{
			if (topicForCreationDto is null) return BadRequest();
			var topicDto = await serviceManager.TopicService.CreateTopic(chapterId, topicForCreationDto, trackChange: false);
			return CreatedAtRoute("TopicById", new { topicId = topicDto.Id, chapterId }, topicDto);
		}

		[HttpGet]
		[Authorize(Roles = Roles.Admin)]
		public async Task<IActionResult> GetAll([FromRoute] Guid chapterId, [FromQuery] TopicParameters topicParameters)
		{
			var topics = await serviceManager.TopicService.GetTopics(chapterId, topicParameters, trackChange: false);
			Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(topics.metaData));
			return Ok(topics.topics);
		}

		[HttpGet("active")]
		[Authorize(Roles = Roles.Teacher)]
		public async Task<IActionResult> GetActive([FromRoute] Guid chapterId, [FromQuery] TopicParameters topicParameters)
		{
			var topics = await serviceManager.TopicService.GetActiveTopics(chapterId, topicParameters, trackChange: false);
			Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(topics.metaData));
			return Ok(topics.topics);
		}

		[HttpGet("{topicId:guid}", Name = "TopicById")]
		[Authorize(Roles = Roles.Teacher)]
		public async Task<IActionResult> GetById([FromRoute] Guid topicId, [FromRoute] Guid chapterId)
		{
			var topic = await serviceManager.TopicService.GetTopic(chapterId, topicId, trackChange: false);
			return Ok(topic);
		}

		[HttpPut("{topicId:guid}")]
		[Authorize(Roles = Roles.Admin)]
		public async Task<IActionResult> Update([FromRoute] Guid topicId, [FromBody] TopicForUpdateDto topicForUpdate, [FromRoute] Guid chapterId)
		{
			await serviceManager.TopicService.UpdateTopic(chapterId, topicId, topicForUpdate, chapterTrackChange: false, topicTrackChange: true);
			return NoContent();
		}

		[HttpDelete("{topicId:guid}")]
		[Authorize(Roles = Roles.Admin)]
		public async Task<IActionResult> Delete([FromRoute] Guid topicId, [FromRoute] Guid chapterId)
		{
			await serviceManager.TopicService.DeleteTopic(chapterId, topicId, chapterTrackChange: false, topicTrackChange: true);
			return NoContent();
		}
	}
}
