
using Microsoft.AspNetCore.Http;

namespace EasyTransfer.Api.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
			try
			{
				await next.Invoke(context);
			}
            catch (BadRequestException badRequestException)
            {

                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(badRequestException.Message);
            }
            catch (AllreadyExistsException allreadyExistsException)
            {

                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(allreadyExistsException.Message);
            }
            catch (ManyBlikException manyBlikException)
            {

                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(manyBlikException.Message);
            }
            catch (Exception)
			{
				context.Response.StatusCode = 500;
				await context.Response.WriteAsync("Something went wrong");
			}
        }
    }
}
