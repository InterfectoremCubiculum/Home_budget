using Home_budget_library.Models;

namespace Home_budget_library.Controllers
{
    public class CategoryController
    {
        private readonly HomeBudgetDbContext _context;
        public CategoryController()
        {
            _context = new HomeBudgetDbContext();
        }

        public bool IsCategory(string categoryName)
        {
            return _context.Categories.Any(u => u.name.ToLower().Equals(categoryName.ToLower()));
        }

        public bool addCategory(string categoryName)
        {
            Category category = new Category() { name = categoryName };
            _context.Categories.Add(category);
            return Save();
        }

        public int GetID(string categoryName)
        {
            Category category = _context.Categories.First(u => u.name.ToLower().Equals(categoryName.ToLower()));
            if(category is null)
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
    }
}
