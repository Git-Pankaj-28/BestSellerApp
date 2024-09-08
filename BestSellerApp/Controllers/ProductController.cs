using BestSellerApp.Models;
using BestSellerApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BestSellerApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDbContext context;
        private readonly IWebHostEnvironment environment;


        // private readonly ProductDbContext context;
        public ProductController(ProductDbContext context,IWebHostEnvironment environment)
        {
            this.context= context;
            this.environment = environment;
        }


        public IActionResult Index()
         {
            var products=context.products.ToList();
            //for reverse order use following 
            //var products =context.products.OrderByDescending(p=>p.Id).ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductDTO productDTO)
        {
            if (productDTO.Image == null)
            {
                ModelState.AddModelError("Image", "The Image file is required");  // we dodnot add ant validation to image so adding it here
            }

            if (!ModelState.IsValid)
            {
                return View(productDTO);
            }

            /*///// Validate file extension
            ////var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            ////var fileExtension = Path.GetExtension(productDTO.Image.FileName).ToLower();

            ////if (!validExtensions.Contains(fileExtension))
            ////{
            ////    ModelState.AddModelError("Image", "Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");
            ////    return View(productDTO);
            ////}

            ////// Ensure the directory exists
            ////string uploadsFolder = Path.Combine(environment.WebRootPath, "products");
            ////if (!Directory.Exists(uploadsFolder))
            ////{
            ////    Directory.CreateDirectory(uploadsFolder);
            ////}

            ////// Generate a unique file name
            ////string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;

            ////// Combine directory and file name to get the full path
            ////string imgFullPath = Path.Combine(uploadsFolder, newFileName);

            ////// Save the file to the server
            ////using (var stream = new FileStream(imgFullPath, FileMode.Create))
            ////{
            ////    productDTO.Image.CopyTo(stream);
            ////}
            */






            //Save the image
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDTO.Image!.FileName);

            string imgFullPath = environment.WebRootPath + "/products/" + newFileName;
            using (var stream = System.IO.File.Create(imgFullPath))
            {
                productDTO.Image.CopyTo(stream);
            }
            //Save the product in database
            Product product = new Product()
            {
                Name = productDTO.Name,
                Brand = productDTO.Brand,
                Category = productDTO.Category,
                Price = productDTO.Price,
                Desciption = productDTO.Desciption,
                ImageFile = newFileName,
                CreatedAt = DateTime.Now
            };

            //save in db  

            context.products.Add(product);
            context.SaveChanges();

            return RedirectToAction("Index", "Product");
        }
       public IActionResult Edit(int id) {
            var product = context.products.Find(id);
            if(product == null)
            {
                RedirectToAction("Index", "Product");
            }
            

            // create ProductDTO from product 

            var productDTO = new ProductDTO()
            {
                Name = product.Name,
                Brand = product.Brand,
                Desciption= product.Desciption,
                Price  = product.Price,
            };

            ViewData["ProductId"] = product.Id;
            ViewData["ImageFile"] = product.ImageFile;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/YYYY");

            return View(productDTO);
        }
        [HttpPost]
        public IActionResult Edit(int id, ProductDTO productDTO)
        {
            var product = context.products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Product");
            }
            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFile"] = product.ImageFile;
                ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/YYYY");
                return View(productDTO);
            }

            // Update the image file new submitted 
            string newFileName = product.ImageFile;
            if (productDTO.Image != null)
            {
                newFileName = DateTime.Now.ToString("MM/dd/yyyy");
                newFileName += Path.GetExtension(productDTO.Image.FileName);

                string imgFullPath = environment.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imgFullPath))
                {
                    productDTO.Image.CopyTo(stream);
                }

                //Delete the old image

                string OldImgPath = environment.WebRootPath + "/products/" + product.ImageFile;
                System.IO.File.Delete(OldImgPath);
            

            }


            //Update the data in database
            product.Name = productDTO.Name;
            product.Brand= productDTO.Brand;
            product.Category= productDTO.Category;
            product.Price= productDTO.Price;
            product.Desciption=productDTO.Desciption;
            product.ImageFile = newFileName;

            context.SaveChanges();

            return RedirectToAction("Index","Product");
              

        }

        public IActionResult Delete(int id)
        {
            var product = context.products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Product");
            }
            string ImgFullPath=environment.WebRootPath +"/products"+product.ImageFile;
            System.IO.File.Delete(ImgFullPath);

            context.Remove(product);
            context.SaveChanges(true);
            return RedirectToAction("Index", "Product");
        }
    }
}






//string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
//newFileName += Path.GetExtension(productDTO.Image!.FileName);

//string imgFullPath = environment.WebRootPath + "/products" + newFileName;
//using (var stream = System.IO.File.Create(imgFullPath))
//{
//    productDTO.Image.CopyTo(stream);
//}
////Save the product in database

//Product product = new Product()
//{
//    Name = productDTO.Name,
//    Brand = productDTO.Brand,
//    Category = productDTO.Category,
//    Price = productDTO.Price,
//    Desciption = productDTO.Desciption,
//    ImageFile = newFileName,
//    CreatedAt = DateTime.Now
//};

////save in db  

//context.products.Add(product);
//context.SaveChanges();