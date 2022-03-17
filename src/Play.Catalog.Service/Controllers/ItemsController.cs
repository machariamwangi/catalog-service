using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Name.Catalog.Service.Controller
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "potion", "Restores a small amount of Hp", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures Poison", 7, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze sword", "Deals a small amount of Damage", 20, DateTimeOffset.UtcNow)
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto>  GetById(Guid id)
        {
            var item = items.Where(item => item.Id == id).SingleOrDefault();
            if(item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public ActionResult<ItemDto> Post(CreatedItemDto createdItemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), createdItemDto.Name, createdItemDto.Description, createdItemDto.Price, DateTimeOffset.UtcNow);

            items.Add(item);

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
        {
            var existingitem = items.Where(item => item.Id == id).SingleOrDefault();
            if (existingitem == null)
            {
                return NotFound();
            }
            var updatedItem = existingitem with
            {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price =updateItemDto.Price
            };

            var index = items.FindIndex(existingitem => existingitem.Id == id);
            items[index] = updatedItem;

            return NoContent();
        }


        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            var index = items.FindIndex(existingitem => existingitem.Id == id);
            if (index < 0)
            {
                return NotFound();
            }
            items.RemoveAt(index);
            return NoContent(); 
        }
    }
}