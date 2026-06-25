using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NLADotNetInternshipTraining.RbacStaticMvc.Features.Product;


[Authorize]
public class ProductController : Controller
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [Authorize(Policy = "ProductView")]
    public IActionResult Index([FromQuery] ProductListRequest request)
    {
        var response = _productService.GetProducts(request);
        return View(response);
    }

    [HttpGet]
    [Authorize(Policy = "ProductCreate")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Policy = "ProductCreate")]
    public IActionResult Create(ProductCreateRequest request)
    {
        var response = _productService.CreateProduct(request);
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Policy = "ProductUpdate")]
    public IActionResult Edit(int id)
    {
        var products = _productService.GetProducts(new ProductListRequest { PageSize = 1000 });
        var product = products.Products.FirstOrDefault(x => x.Id == id);
        if (product == null) return NotFound();

        var request = new ProductUpdateRequest
        {
            Name = product.Name,
            Price = product.Price,
            Description = product.Description
        };
        return View(request);
    }

    [HttpPost]
    [Authorize(Policy = "ProductUpdate")]
    public IActionResult Edit(int id, ProductUpdateRequest request)
    {
        var response = _productService.UpdateProduct(id, request);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize(Policy = "ProductDelete")]
    public IActionResult Delete(int id)
    {
        var response = _productService.DeleteProduct(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Superman")]
    public IActionResult AdminOnly()
    {
        return View(new AdminOnlyResponse { Message = "Only Admin can access this page." });
    }
}