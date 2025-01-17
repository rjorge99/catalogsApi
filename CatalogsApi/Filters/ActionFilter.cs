﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace CatalogsApi.Filters
{
    public class ActionFilter : IActionFilter
    {
        private readonly ILogger<ActionFilter> _logger;

        public ActionFilter(ILogger<ActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("OnActionExecuting");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("OnActionExecuted");
        }
    }
}
