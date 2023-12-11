using Microsoft.AspNetCore.Mvc;
using TaskExecutor.DTOs;
using TaskExecutor.Models;
using TaskExecutor.Repository;
using TaskExecutor.Repository.Implementation;
using TaskExecutor.Services;
using TaskExecutor.Services.Implementation;

namespace TaskExecutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodesController : ControllerBase
    {
        private INodeService _nodeService;

        public NodesController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult RegisterNode([FromBody] NodeRegistrationRequest nodeRegistrationRequest)
        {
            if (nodeRegistrationRequest?.Name == null || nodeRegistrationRequest?.Address == null)
            {
                return BadRequest(nodeRegistrationRequest?.ToString());
            }

            _nodeService.AddNode(nodeRegistrationRequest.Name, nodeRegistrationRequest.Address);
            return Ok();
        }

        [HttpDelete]
        [Route("unregister/{name}")]
        public IActionResult UnregisterNode(string name)
        {
            if (name == null)
            {
                return BadRequest("Name cannot be Null.");
            }

            _nodeService.RemoveNode(name);
            return Ok();
        }

        [HttpPost]
        [Route("shutdown/{name}")]
        public IActionResult MakeNodeOffline(string name)
        {
            if (name == null)
            {
                return BadRequest("Name cannot be Null.");
            }

            _nodeService.MakeNodeOffline(name);
            return Ok();
        }

        [HttpGet]
        [Route("Status")]
        public IActionResult GetNodesWithStatus()
        {
            var nodes = _nodeService.GetAllNodes();
            return Ok(nodes);
        }
    }
}
