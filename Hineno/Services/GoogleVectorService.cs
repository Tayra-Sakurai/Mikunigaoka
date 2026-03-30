using Hineno.Extensions;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hineno.Services
{
    /// <summary>
    /// A AI-powered service class specifically for Google GenAI.
    /// </summary>
    public class GoogleVectorService : IVectorService
    {
        private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;
        private const int DIMENSIONS = 768;

        /// <summary>
        /// Initializes a new instance of the GoogleVectorService class using the specified embedding generator.
        /// </summary>
        /// <param name="embeddingGenerator">The embedding generator to use for creating vector embeddings from string inputs. Cannot be null.</param>
        public GoogleVectorService(IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
        {
            _embeddingGenerator = embeddingGenerator;
        }

        public async Task<float[]> GenerateVectorAsync(string title, string document)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);
            ArgumentException.ThrowIfNullOrWhiteSpace(document);

            AdditionalPropertiesDictionary additionalOptions = new()
            {
                { "TaskType", "RETRIEVAL_DOCUMENT" },
                { "Title", title },
            };

            EmbeddingGenerationOptions options = new()
            {
                AdditionalProperties = additionalOptions,
                Dimensions = DIMENSIONS
            };

            ReadOnlyMemory<float> embedding = await _embeddingGenerator.GenerateVectorAsync(document, options);

            return embedding.ToArray();
        }

        public async Task<float[]> GenerateVectorAsync(string classification)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(classification);

            AdditionalPropertiesDictionary additionalOptions = new()
            {
                { "TaskType", "CLASSIFICATION" },
            };

            EmbeddingGenerationOptions options = new()
            {
                AdditionalProperties = additionalOptions,
                Dimensions = DIMENSIONS,
            };

            ReadOnlyMemory<float> vector = await _embeddingGenerator.GenerateVectorAsync(classification, options);

            return vector.ToArray();
        }

        public async IAsyncEnumerable<float> GetMatchRatesAsync(string query, IEnumerable<float[]> vectors)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(query);
            ArgumentNullException.ThrowIfNull(vectors);

            AdditionalPropertiesDictionary additionalOptions = new()
            {
                { "TaskType", "RETRIEVAL_QUERY" },
            };

            EmbeddingGenerationOptions options = new()
            {
                AdditionalProperties = additionalOptions,
                Dimensions = DIMENSIONS,
            };

            ReadOnlyMemory<float> vector = await _embeddingGenerator.GenerateVectorAsync(query, options);

            foreach (var vec in vectors)
                yield return vec.GetInnerProduct(vector.ToArray());
        }

        public async Task<IReadOnlyList<ValueTuple<string, float>>> GetSuggestionsAsync(string query, IEnumerable<ValueTuple<string, float[]>> collection)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(query);
            ArgumentNullException.ThrowIfNull(collection);

            AdditionalPropertiesDictionary additionalOptions = new()
            {
                { "TaskType", "RETRIEVAL_QUERY" },
            };

            EmbeddingGenerationOptions options = new()
            {
                AdditionalProperties = additionalOptions,
                Dimensions = DIMENSIONS,
            };

            ReadOnlyMemory<float> queryVector = await _embeddingGenerator.GenerateVectorAsync(query, options);

            List<ValueTuple<string, float>> valueTuples = [];

            foreach (var (str, vec) in collection)
                valueTuples.Add(new(str, vec.GetInnerProduct(queryVector.ToArray())));

            (string, float)[] values =
                valueTuples.OrderByDescending(v => v.Item2)
                .ToArray();

            return [.. values];
        }

        public async Task<IReadOnlyList<(string, float)>> GetSuggestionsAsync(string query, IEnumerable<float[]> vectors, IEnumerable<string> suggestions)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(query);
            ArgumentNullException.ThrowIfNull(vectors);
            ArgumentNullException.ThrowIfNull(suggestions);

            return await GetSuggestionsAsync(query, suggestions.Zip(vectors).ToList());
        }
    }
}
