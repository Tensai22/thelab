using TheLab.Models;

namespace TheLab.Service
{
	public class ReviewService
	{
		private readonly AppDbContext _context;

		public ReviewService(AppDbContext context)
		{
			_context = context;
		}

		public List<Reviews> GetAllReviews()
		{
			return _context.Reviews.ToList(); 
		}
	}

}
