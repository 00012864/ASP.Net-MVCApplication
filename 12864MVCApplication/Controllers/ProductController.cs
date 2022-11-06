using _12864MVCApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _12864MVCApplication.Controllers
{
    public class ProductController : Controller
    {
        // Web API service url from Microservice Application 
        string Baseurl = "https://localhost:44359/";

        //info necessary for edit section 
        HttpClient client;
        Uri baseAddress = new Uri("https://localhost:44359/");

        public ProductController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        // GET: Product
        public async Task<ActionResult> Index()
        {

            string Baseurl = "https://localhost:44359/";
            List<Product> Products = new List<Product>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();

                // Format used for request - json data
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));

                // Requesting to get the products
                HttpResponseMessage Response = await client.GetAsync("api/Product");

                //To know if the response is successful
                if (Response.IsSuccessStatusCode)
                {
                    //To keep the information that was given in response to the request
                    var ProductResponse = Response.Content.ReadAsStringAsync().Result;

                    //Converting json string to object 
                    Products = JsonConvert.DeserializeObject<List<Product>>(ProductResponse);
                } 
                return View(Products);
            }
        }

        // GET: Product/GetProduct/5
        [HttpGet]
        public async Task<ActionResult> GetProduct(int id)
        {
            Product prod = new Product();
            List<Product> products = new List<Product>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                
                //sending get request to the specified uri and strong its response
                HttpResponseMessage Response = await client.GetAsync("api/Product/" + id);

                if (Response.IsSuccessStatusCode)
                {
                    var Result = Response.Content.ReadAsStringAsync().Result;
                    prod = JsonConvert.DeserializeObject<Product>(Result);
                }

            }

            return View(prod);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        //Post method is used because the product should be added as a resource
        public ActionResult Create(Product product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                var postJob = client.PostAsJsonAsync<Product>("api/Product", product);

                //since it is asynchronous task, waiting should happen for it to finish
                postJob.Wait(); 
                var postResult = postJob.Result;
                if (postResult.IsSuccessStatusCode)
                    return RedirectToAction("Index");

            }
            ModelState.AddModelError(string.Empty, "error occured");
            return View(product);
        }


        //Get the products first before updating it
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            Product prod = new Product();
            List<Product> products = new List<Product>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Response = await client.GetAsync("api/Product/" + id);
                if (Response.IsSuccessStatusCode)
                {
                    var Result = Response.Content.ReadAsStringAsync().Result;
                    prod = JsonConvert.DeserializeObject<Product>(Result);
                }

            }
            return View(prod);
        }

        public ActionResult Edit(Product product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.PutAsJsonAsync(Baseurl + "api/Product/" + product.Id, product).Result;
                if (Res.IsSuccessStatusCode)
                {
                    ViewBag.msg = "Update was successfull";
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.msg = "Sorry, update failed";
                }
                return View();
            }
        }

        [HttpGet]
        //The product selected to be deleted should be shown hence the get method
        public async Task<ActionResult> Delete(int id)
        {
            Product product = new Product();
            List<Product> products = new List<Product>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Response = await client.GetAsync("api/Product/" + id);
                if (Response.IsSuccessStatusCode)
                {
                    var Result = Response.Content.ReadAsStringAsync().Result;
                    product = JsonConvert.DeserializeObject<Product>(Result);
                }
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(Product pr)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Response = client.DeleteAsync(Baseurl + "api/Product/" + pr.Id).Result;
                if (Response.IsSuccessStatusCode)
                {
                    ViewBag.msg = "Product Deleted!";
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.msg = "Sorry, product did not get deleted";
                }

            }
            return RedirectToAction("Index");
        }
    }
}
