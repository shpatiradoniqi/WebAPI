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
using System.IO;
using Microsoft.AspNetCore.Hosting;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase{

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public CarController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;

        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
              select CarId,CarName,Category,
              convert(varchar(10),DateOfJoining,120)as DateOfJoining
               ,PhotoFileName from dbo.Car";


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
            return new JsonResult(table);


        }

        [HttpPost]
        public JsonResult Post(Car cr)
        {
            string query = @"
               insert into dbo.Car(CarName,Category,DateOfJoining,PhotoFileName)
                values
                  ('"
                      + cr.CarName + @"' 
                    , '" + cr.Category + @"'
                     , '" + cr.DateOfJoining + @"'
                     , '" + cr.PhotoFileName + @"'
                       )";


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
        public JsonResult Put(Car ct)
        {
            string query = @"
               update dbo.Car set 
              CarName='" + ct.CarName + @"'
              ,Category='" + ct.Category + @"'
              ,DateOfJoining='" + ct.DateOfJoining + @"'
                where CarId=" + ct.CarId + @"

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
              delete dbo.Car  
             where CarId=" + id + @"
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


        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("anonim.png");
            }
        }
        [Route("GetAllCategories")]
        public JsonResult GetAllCategories()
        {
            string query = @"
                    select CategoryName from dbo.Category
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
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }


    }

}

