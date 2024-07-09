
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.Domain;
using  NZWalks.API.Repositories;
namespace NZWalks.API.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase {

        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this.dbContext= dbContext;
            this.regionRepository = regionRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(){
    
            var regions = await regionRepository.GetAllAsync();

            var RegionsDto = new List<RegionDTO>();
            foreach (var region in regions){
                RegionsDto.Add(new RegionDTO(){
                    Name = region.Name,
                    Id = region.Id,
                    Code = region.Code,
                    RegionImageUrl= region.RegionImageUrl,
                    
            });
            }
            return Ok(RegionsDto.ToArray());
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetId([FromRoute] Guid id){
            var region = await regionRepository.GetByIdAsync(id);
            if(region == null){
                return NotFound();
            }
            var regionsDto = new RegionDTO(){
                  Name = region.Name,
                    Id = region.Id,
                    Code = region.Code,
                    RegionImageUrl= region.RegionImageUrl,
            };
            return Ok(regionsDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionDtoRequestDto addRegionDtoRequestDto){

                var regionDomainModel = new Region{
                    Name = addRegionDtoRequestDto.Name,
                            
                            Code = addRegionDtoRequestDto.Code,
                            RegionImageUrl= addRegionDtoRequestDto.RegionImageUrl,
                };
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);
            return CreatedAtAction(nameof(GetId), new { id =regionDomainModel.Id }, regionDomainModel);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionDtoRequestDto updateRegionDtoRequestDto){

            var region = new Region{
                Name = updateRegionDtoRequestDto.Name,
                            
                            Code = updateRegionDtoRequestDto.Code,
                            RegionImageUrl= updateRegionDtoRequestDto.RegionImageUrl,
            };
            region = await regionRepository.UpdateAsync(id, region);
            if(region == null){
                return NotFound();
            }
            
            return Ok();

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id){
            var region = dbContext.Regions.FirstOrDefault(x=>x.Id == id);
            if(region == null){
                return NotFound();
            }
            dbContext.Regions.Remove(region);
            await dbContext.SaveChangesAsync();
            return Ok();


        }
    } 
}


