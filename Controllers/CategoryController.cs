using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebAPI.Model;





namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase{

        private readonly IConfiguration _configuration;

        public CategoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
               select CategoryId,CategoryName from dbo.Category";
             
            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CarAppCon");
            SqlDataReader  myReader;
            using (SqlConnection myCon=new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                using (SqlCommand myCommand =new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();


                        
                }
                    
            }
            return new JsonResult(table);


        }

        [HttpPost]
        public JsonResult Post(Category ct)
        {
            string query = @"
               insert into dbo.Category values
                  ('"+ct.CategoryName+@"')";


            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CarAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();



                }

            }
            return new JsonResult("Added succesfully");



        }
        [HttpPut]
        public JsonResult Put(Category ct)
        {
            string query = @"
               update dbo.Category set 
                CategoryName='" + ct.CategoryName + @"'
                where CategoryId=" + ct.CategoryId + @"

                 ";

                
               

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CarAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();



                }

            }
            return new JsonResult("Updated succesfully");



        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
              delete dbo.Category  
             where CategoryId=" + id + @"
                ";




            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CarAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();



                }

            }
            return new JsonResult("Deleted succesfully");



        }




    }
}
