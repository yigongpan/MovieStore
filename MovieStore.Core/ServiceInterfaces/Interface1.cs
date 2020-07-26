using MovieStore.Core.Entities;
using System.Threading.Tasks;

namespace MovieStore.Core.ServiceInterfaces
{
    public interface IReviewService
    {
        Task<Review> AddReview(Review review);
    }
}