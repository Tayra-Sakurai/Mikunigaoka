using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hineno.Services
{
    /// <summary>
    /// Base interface for generating vectors and vector-based searches.
    /// </summary>
    public interface IVectorService
    {
        /// <summary>
        /// Generates a vector representation from the specified title and document content.
        /// </summary>
        /// <param name="title">The title associated with the document. Cannot be null.</param>
        /// <param name="document">The main content of the document to be converted into a vector. This cannot be null.</param>
        /// <returns>A float array representing the vectorized form of the input title and document.</returns>
        /// <exception cref="ArgumentNullException">One or more input string parameters are null.</exception>
        /// <exception cref="ArgumentException">One or more parameters are empty or only contain white spaces.</exception>
        Task<float[]> GenerateVectorAsync(string title, string document);

        /// <summary>
        /// Generates a vector which represents the classification name properties.
        /// </summary>
        /// <param name="classification">The class label name.</param>
        /// <returns>A <see cref="Task{float[]}"/> instance to manage the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="classification"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="classification"/> is empty or only contains white spaces.</exception>
        Task<float[]> GenerateVectorAsync(string classification);

        /// <summary>
        /// Asynchronously computes the match rates between the specified query and a collection of vector
        /// representations.
        /// </summary>
        /// <param name="query">The query string to compare against the provided vectors. Cannot be null.</param>
        /// <param name="vectors">A collection of float arrays representing the vectors to be matched against the query. Cannot be null.</param>
        /// <returns>An asynchronous stream of floating-point values, each representing the match rate for a corresponding vector
        /// in the input collection.</returns>
        /// <exception cref="ArgumentException"><paramref name="query"/> is empty or only contains white spaces.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="query"/> and/or <paramref name="vectors"/> are null.</exception>
        IAsyncEnumerable<float> GetMatchRatesAsync(string query, IEnumerable<float[]> vectors);

        /// <summary>
        /// Asynchronously retrieves a list of suggestion strings ranked by relevance to the specified query.
        /// </summary>
        /// <param name="query">The input query string for which suggestions are to be generated. Cannot be null.</param>
        /// <param name="vectors">A collection of feature vectors corresponding to the suggestions. Each vector represents the features of a
        /// suggestion and must align in order with the 'suggestions' parameter.</param>
        /// <param name="suggestions">A collection of suggestion strings to be considered for ranking. The order must correspond to the 'vectors'
        /// parameter.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of tuples, each
        /// consisting of a suggestion string and its associated relevance score. The list is ordered by descending
        /// relevance.</returns>
        /// <exception cref="ArgumentException">The query is empty or only contains white spaces.</exception>
        /// <exception cref="ArgumentNullException">The input values contain null value.</exception>
        Task<IReadOnlyList<ValueTuple<string, float>>> GetSuggestionsAsync(string query, IEnumerable<float[]> vectors, IEnumerable<string> suggestions);

        /// <param name="collection">The collection of tuples of vector and the raw string.</param>
        /// <inheritdoc cref="GetSuggestionsAsync(string, IEnumerable{float[]}, IEnumerable{string})"/>
        Task<IReadOnlyList<ValueTuple<string, float>>> GetSuggestionsAsync(string query, IEnumerable<ValueTuple<string, float[]>> collection);
    }
}
