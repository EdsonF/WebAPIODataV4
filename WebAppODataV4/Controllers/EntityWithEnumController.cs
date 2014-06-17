﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using WebAppODataV4.Models;

namespace WebAppODataV4.Controllers
{
    [ODataRoutePrefix("EntityWithEnum")]
    public class EntityWithEnumController : ODataController
    {
        private List<EntityWithEnum> someData = new List<EntityWithEnum>();

        public EntityWithEnumController()
        {
            someData.Add(new EntityWithEnum { Description = "test1", Name = "Van", PhoneNumberType = PhoneNumberTypeEnum.Home});
            someData.Add(new EntityWithEnum { Description = "test2", Name = "Bill", PhoneNumberType = PhoneNumberTypeEnum.Work });
            someData.Add(new EntityWithEnum { Description = "test3", Name = "Rob", PhoneNumberType = PhoneNumberTypeEnum.Cell });
        }

        [ODataRoute]
        [EnableQuery(PageSize = 20)]
        public IHttpActionResult Get()
        {
            return Ok(someData);
        }

        [ODataRoute]
        [EnableQuery(PageSize = 20)]
        public IHttpActionResult Get([FromODataUri] string key)
        {
            if (someData.Exists(t => t.Name == key))
            {
                return Ok(someData.FirstOrDefault(t => t.Name == key));
            }

            return BadRequest("key does not key");
        }

        [HttpPost]
        [ODataRoute]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IHttpActionResult Post(EntityWithEnum entityWithEnum)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            return Created(entityWithEnum);
        }

        [HttpGet]
        [EnableQuery(PageSize = 20, AllowedQueryOptions = AllowedQueryOptions.All)]
        [ODataRoute("Default.PersonSearchPerPhoneType(PhoneNumberTypeEnum={phoneNumberTypeEnum})")]
        public IHttpActionResult PersonSearchPerPhoneType([FromODataUri] PhoneNumberTypeEnum phoneNumberTypeEnum)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(someData.Where(t => t.PhoneNumberType.Equals(phoneNumberTypeEnum)));
        }

    }
}
