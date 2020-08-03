using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Warframeaccountant.database_action;
using Warframeaccountant.domain;

namespace Warframeaccountant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private ItemRepo itemRepo;

        public ItemController ()
        {
            itemRepo = new ItemRepo();
        }


        [HttpGet]
        public List<Item> get()
        {
            Console.WriteLine("Get All Item");
            return itemRepo.getAllItem();
        }

        [HttpGet("name")]
        public List<Item> getAllNameItemList()
        {
            Console.WriteLine("Get order name list");
            return itemRepo.getAllOrderItemList("name");
        }

        [HttpGet("quantity")]
        public List<Item> getAllQuantityItemList()
        {
            Console.WriteLine("Get order quantity list");
            return itemRepo.getAllOrderItemList("quantity");
        }

        [HttpGet("{type}/name")]
        public List<Item> getNameOrderItemList (int type)
        {
            Console.WriteLine("Get filter item list order name");
            return itemRepo.getFilterOrderItemList("name", type);
        }

        [HttpGet("{type}/quantity")]
        public List<Item> getQuantityOrderItemList(int type)
        {
            Console.WriteLine("Get filter item list order quantity");
            return itemRepo.getFilterOrderItemList("quantity", type);
        }

        [HttpPost("new")]
        public Item addOne([FromBody] JsonElement newItem)
        {
            Console.WriteLine("Add new Item");

            var newItemObj = new Item()
            {
                itemId = Convert.ToInt32(newItem.GetProperty("itemId").ToString()),
                name = newItem.GetProperty("name").ToString(),
                type = Convert.ToInt32(newItem.GetProperty("type").ToString()),
                imageString = newItem.GetProperty("imageString").ToString(),
                quantity = Convert.ToInt32(newItem.GetProperty("quantity").ToString()),
                bprice = Convert.ToInt32(newItem.GetProperty("bprice").ToString()),
                eprice = Convert.ToInt32(newItem.GetProperty("eprice").ToString())
            };

            if (itemRepo.addOne(newItemObj))
            {
                return newItemObj;
            }
            else
            {
                return null;
            }
        }

        [HttpPut("all")]
        public String updateAll([FromBody] JsonElement itemList)
        {
            Console.WriteLine("Update all");
            var list = itemList.GetProperty("itemList").EnumerateArray();

            while(list.MoveNext())
            {
                var targetItem = new Item()
                {
                    itemId = Convert.ToInt32(list.Current.GetProperty("itemId").ToString()),
                    name = list.Current.GetProperty("name").ToString(),
                    type = Convert.ToInt32(list.Current.GetProperty("type").ToString()),
                    imageString = list.Current.GetProperty("imageString").ToString(),
                    quantity = Convert.ToInt32(list.Current.GetProperty("quantity").ToString()),
                    bprice = Convert.ToInt32(list.Current.GetProperty("bprice").ToString()),
                    eprice = Convert.ToInt32(list.Current.GetProperty("eprice").ToString())
                };

                if (itemRepo.updateOne(targetItem) == false)
                {
                    return "{result: 'faill'}";
                }
            }

            return "{result: 'success'}";
        }

        [HttpPut("one")]
        public Item updateOne ([FromBody] JsonElement targetItem)
        {
            var targetItemObj = new Item()
            {
                itemId = Convert.ToInt32(targetItem.GetProperty("itemId").ToString()),
                name = targetItem.GetProperty("name").ToString(),
                type = Convert.ToInt32(targetItem.GetProperty("type").ToString()),
                imageString = targetItem.GetProperty("imageString").ToString(),
                quantity = Convert.ToInt32(targetItem.GetProperty("quantity").ToString()),
                bprice = Convert.ToInt32(targetItem.GetProperty("bprice").ToString()),
                eprice = Convert.ToInt32(targetItem.GetProperty("eprice").ToString())
            };

            itemRepo.updateOne(targetItemObj);

            return targetItemObj;
        }

        [HttpDelete("remove/{itemId}")]
        public Item removeItem(int itemId)
        {
            Console.WriteLine($"Remove one item ID: {itemId}");
            return itemRepo.removeOne(itemId);
        }

        [HttpPut("changeOne/{itemId}/{amount}")]
        public String changeOneQuantity (int itemId, int amount)
        {
            Console.WriteLine($"Change Item {itemId} quantity with value {amount}");

            if(itemRepo.changeOneQuantity(itemId, amount))
            {
                return "{result: 'success'}";
            }
            else
            {
                return "{result: 'faill'}";
            }
        }
        
    }
}
