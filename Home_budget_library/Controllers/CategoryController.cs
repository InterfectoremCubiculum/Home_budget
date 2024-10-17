using Home_budget_library.Models;

namespace Home_budget_library.Controllers
{
    public class CategoryController
    {
        private readonly HomeBudgetDbContext _context;
        public CategoryController()
        {
            this._context = new HomeBudgetDbContext();
        }
        public CategoryController(HomeBudgetDbContext context)
        {
            this._context = context;
        }

        public bool IsCategory(string categoryName)
        {
            return _context.Categories.Any(u => u.Name.ToLower().Equals(categoryName.ToLower()));
        }

        public bool addCategory(string categoryName)
        {
            Category category = new Category() { Name = categoryName };
            _context.Categories.Add(category);
            return Save();
        }

        public int GetID(string categoryName)
        {
            Category category = _context.Categories.First(u => u.Name.ToLower().Equals(categoryName.ToLower()));
            if (category is null)
            {
                return 0;
            }
            return category.Id;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public List<Category> GetAll()
        {
            var categories = _context.Categories.ToList();
            return categories;
        }
        public List<string> GetAllNames()
        {
            var categories = _context.Categories
                                     .Select(c => c.Name)
                                     .ToList(); return categories;
        }
    }
}
