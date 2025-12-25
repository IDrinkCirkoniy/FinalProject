using Microsoft.AspNetCore.Mvc;
using ShoeClassLibrary.Contexts;
using System.Reflection.Metadata.Ecma335;

namespace ShoesWebApp.Controllers
{
    public class ShoeControler : Controller
    {
        private readonly ShoeContext _context;

        public ShoeControler(ShoeContext context)
        {
            _context = context;
        }
    }
}
