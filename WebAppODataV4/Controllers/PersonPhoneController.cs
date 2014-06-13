﻿using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using WebAppODataV4.Database;
using Microsoft.Data.OData;

namespace WebAppODataV4.Controllers
{
    public class PersonPhoneController : ODataController
    {
        readonly DomainModel _db = new DomainModel();
        private static readonly ODataValidationSettings _validationSettings = new ODataValidationSettings();

        [EnableQuery(PageSize = 20)]
        public IHttpActionResult GetPersonPhone(ODataQueryOptions<PersonPhone> queryOptions)
        {
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(_db.PersonPhone.AsQueryable());
        }

        [EnableQuery(PageSize = 20)]
        public IHttpActionResult GetPersonPhone([FromODataUri] int key, ODataQueryOptions<PersonPhone> queryOptions)
        {
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(_db.PersonPhone.SingleOrDefault(t => t.BusinessEntityID == key));
        }

        public IHttpActionResult Put([FromODataUri] int key, PersonPhone personPhone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != personPhone.BusinessEntityID)
            {
                return BadRequest();
            }

            _db.PersonPhone.AddOrUpdate(t => t.BusinessEntityID == key, personPhone);
            _db.SaveChanges();
            return Updated(personPhone);
        }

        public IHttpActionResult Post(PersonPhone personPhone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.PersonPhone.AddOrUpdate(personPhone);
            _db.SaveChanges();
            return Created(personPhone);

        }

        /// <summary>
        /// This is a Odata Action for complex data changes...
        /// </summary>
       [HttpPost]
        public async Task<IHttpActionResult> ChangePersonStatus([FromODataUri] int key, [FromBody]ODataActionParameters parameters)
        {
            if (ModelState.IsValid)
            {
                var level = parameters["Level"];
                // SAVE THIS TO THE DATABASE OR WHATEVER....
            }

            return Ok(true);
        }

        // PATCH: odata/PersonPhones(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<PersonPhone> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var personPhone = _db.PersonPhone.Single(t => t.BusinessEntityID == key);            
            delta.Patch(personPhone);
            _db.SaveChanges();
            return Updated(personPhone);
        }

        public IHttpActionResult Delete([FromODataUri] int key)
        {
            var entityInDb = _db.PersonPhone.SingleOrDefault(t => t.BusinessEntityID == key);
            _db.PersonPhone.Remove(entityInDb);
            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
