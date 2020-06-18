using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace WebAPIDemo.Controllers
{
    public class EmployeeController : ApiController
    {
        EmployeeDBEntities db = new EmployeeDBEntities();

        public IEnumerable<Employee> Get()
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.ToList();
            }
        }

        public HttpResponseMessage Get(int id)
        {
            var entity= db.Employees.FirstOrDefault(e => e.ID == id);
            if(entity != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "employee with id : " + id.ToString()+" is not found");
            }
          
        }

        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                var message = Request.CreateErrorResponse(HttpStatusCode.Created, employee.ToString());
                message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                return message;
            }
            catch(Exception ex)
            {
               return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var entity = db.Employees.FirstOrDefault(e => e.ID == id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, " Employee with id = " + id.ToString() + " not found to delete");
                }
                else
                {
                    db.Employees.Remove(entity);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id,[FromBody]Employee employee)
        {
            try
            {
                var entity = db.Employees.FirstOrDefault(e => e.ID == id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, " Employee with id = " + id.ToString() + "is not found");
                }
                else
                {
                    entity.FirstName = employee.FirstName;
                    entity.LastName = employee.LastName;
                    entity.Gender = employee.Gender;
                    entity.Salary = employee.Salary;

                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


    }
}
