using Microsoft.AspNetCore.Mvc;
using TaskExecutor.Models;
using TaskExecutor.Repository;
using TaskExecutor.Services;

namespace TaskExecutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodesController : ControllerBase
    {
        private NodeRepository _nodeRepository;

        public NodesController()
        {
            _nodeRepository = NodeRepository.GetInstance();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult RegisterNode([FromBody] NodeRegistrationRequest nodeRegistrationRequest)
        {
            if (nodeRegistrationRequest?.Name == null || nodeRegistrationRequest?.Address == null)
            {
                return BadRequest(nodeRegistrationRequest?.ToString());
            }

            _nodeRepository.AddNode(new Node(nodeRegistrationRequest));
            new TaskOrchestrator().ExecuteNextTask();
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

            _nodeRepository.RemoveNode(name);
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

            _nodeRepository.MakeNodeOffline(name);
            return Ok();
        }

        [HttpGet]
        [Route("Status")]
        public IActionResult GetNodesWithStatus()
        {
            var nodes = _nodeRepository.GetAllNodes();
            return Ok(nodes);
        }
    }
}
