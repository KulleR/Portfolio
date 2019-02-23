using AutoMapper;
using HalalEcodes.Controllers.Showcases;
using HalalEcodes.Data;
using HalalEcodes.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HalalEcodes.Controllers
{
    [Produces("application/json")]
    [Route("api/ecodes")]
    public class EcodesController : Controller
    {
        private readonly IEcodeRepository _ecodeRepository;
        private readonly IMapper _mapper;

        public EcodesController(IEcodeRepository ecodeRepository, IMapper mapper)
        {
            this._ecodeRepository = ecodeRepository;
            this._mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetEcodes()
        {
            return new JsonResult(_mapper.Map<List<Ecode>, List<EcodeShowcase>>(await _ecodeRepository.GetAll().Include(c => c.Category).ToListAsync()));
        }

        [HttpGet("{code}")]
        public IActionResult GetEcode(string code)
        {
            return new JsonResult(_mapper.Map<Ecode, EcodeShowcase>(_ecodeRepository.GetByCode(code)));
        }
    }
}