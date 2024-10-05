using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace ClientSide.Pages
{
    public class IndexModel : PageModel
    {
        public Customer customer { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            customer = new Customer();
        }

        public void OnGet()
        {
        }
    }
}
