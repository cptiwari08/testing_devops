using Contract.Interfaces.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Text.Json;

namespace ServerlessAPI.Endpoints
{
    public class Product
    {
        private readonly ILogger<Product> _logger;

        private readonly IProductBL _productBL;

        public Product(ILogger<Product> logger, IProductBL productBL)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productBL = productBL ?? throw new ArgumentNullException(nameof(productBL));
        }

        [Function("GetAll")]
        [OpenApiOperation(operationId: nameof(GetAll), tags: new[] { "Product" }, Description = "Get all products")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Product>))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAll([HttpTrigger(AuthorizationLevel.Function, "get", Route = "product")] HttpRequest req)
        {
            try
            {
                var products = await _productBL.GetAll().ConfigureAwait(false);
                return new OkObjectResult(products);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetAll endpoint failed", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [Function("Get")]
        [OpenApiOperation(operationId: nameof(Get), tags: new[] { "Product" }, Description = "Get a product by id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Product))]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
        [OpenApiResponseWithoutBody(HttpStatusCode.NotFound)]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([HttpTrigger(AuthorizationLevel.Function, "get", Route = "product/{id}")] HttpRequest req, int id)
        {
            try
            {
                if (id <= 0)
                    return new BadRequestResult();

                var product = await _productBL.Get(id).ConfigureAwait(false);
                if (product == null)
                    return new NotFoundResult();

                return new OkObjectResult(product);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get endpoint failed", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }            
        }

        [Function("Create")]
        [OpenApiOperation(operationId: nameof(Create), tags: new[] { "Product" }, Description = "Create a product")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Contract.Models.Product), Required = true, Description = "Product object that needs to be created")]
        [OpenApiResponseWithoutBody(HttpStatusCode.OK)]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create([HttpTrigger(AuthorizationLevel.Function, "post", Route = "product")] HttpRequest req)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var product = JsonSerializer.Deserialize<Contract.Models.Product>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});

                if (product == null || product.Name == null)
                    return new BadRequestResult();

                await _productBL.Create(product).ConfigureAwait(false);

                var httpResponse = new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent(requestBody),
                    Headers = { { "Location", $"/api/product/{product.Id}" } }
                };

                return new OkObjectResult(httpResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError("Create endpoint failed", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [Function("Update")]
        [OpenApiOperation(operationId: nameof(Update), tags: new[] { "Product" }, Description = "Update a product by id")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Contract.Models.Product), Required = true, Description = "Product object that needs to be updated")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithoutBody(HttpStatusCode.NoContent)]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update([HttpTrigger(AuthorizationLevel.Function, "put", Route = "product/{id}")] HttpRequest req, int id)
        {
            try
            {
                if (id <= 0)
                    return new BadRequestResult();

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var product = JsonSerializer.Deserialize<Contract.Models.Product>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (product == null || product.Id != id || product.Name == null)
                    return new BadRequestResult();

                await _productBL.Update(id, product).ConfigureAwait(false);

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError("Update endpoint failed", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [Function("Delete")]
        [OpenApiOperation(operationId: nameof(Delete), tags: new[] { "Product" }, Description = "Delete a product by id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithoutBody(HttpStatusCode.NoContent)]
        [OpenApiResponseWithoutBody(HttpStatusCode.NotFound)]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "product/{id}")] HttpRequest req, int id)
        {
            try
            {
                if (id <= 0)
                    return new BadRequestResult();

                var product = await _productBL.Get(id).ConfigureAwait(false);

                if (product == null)
                    return new NotFoundResult();

                await _productBL.Delete(id).ConfigureAwait(false);

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete endpoint failed", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
