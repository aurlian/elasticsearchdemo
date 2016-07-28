using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3.ElasticSearch
{
    public class ElasticSearchRepo
    {
        private ElasticClient _esClient;

        protected ElasticClient _client
        {
            get
            {
                if (_esClient == null)
                {
                    var connection = new Uri("http://127.0.0.1:9200/");
                    var settings = new ConnectionSettings(connection);
                    _esClient = new ElasticClient(settings);
                }

                return _esClient;
            }
        }

        public async Task<Boolean> CreateIndexAsync(string indexName, string alias)
        {
            bool result = false;
            var resp = await _client.CreateIndexAsync(indexName,
                c => c.Settings(s => s.NumberOfReplicas(1)
                                      .NumberOfShards(1))
                       .Aliases(a => a.Alias(alias))
                       .Mappings(m => m.Map("symptom", s => s.Properties(p => p.String( x => x.Name("Name"))
                                                                               .String(x => x.Name("Description"))))));

            if(resp != null)
            {
                result = resp.Acknowledged;
            }

            return result;
        }

        public async Task<Boolean> RemoveAliasAsync(string alias)
        {
            bool result = false;

            var response = await _client.DeleteAliasAsync("mherindex", alias);

            if (response != null)
            {
                result = response.IsValid;
            }

            return result;
        }

        public async Task<Boolean> AddSymptomAsync(string index, Symptom symp)
        {
            bool result = false;

            var response = await _client.IndexAsync<Symptom>(symp, x => x.Index(index));

            if(response != null)
            {
                result = response.IsValid;
            }
            return result;
        }

        public async Task<Boolean> AddSymptomsAsync(string index)
        {
            bool result = false;

            List<Symptom> symptoms = new List<Symptom>();
            symptoms.Add(new Symptom() { Name = "Blue eye", Description = "The eye turns blue" });
            symptoms.Add(new Symptom() { Name = "Big head", Description = "The head becomes inflated" });
            symptoms.Add(new Symptom() { Name = "small hands", Description = "The hands shrink in size" });

            var bulk = await _client.IndexManyAsync<Symptom>(symptoms, index);

            if (bulk != null)
            {
                result = bulk.IsValid;
            }
            return result;
        }

        public async Task<Boolean> DeleteIndexAsync(string indexName)
        {
            bool result = false;
            var resp = await _client.DeleteIndexAsync(indexName);

            if (resp != null)
            {
                result = resp.Acknowledged;
            }

            return result;
        }

        public ISearchResponse<Symptom> Query()
        {
            QueryContainerDescriptor<Symptom> query = new QueryContainerDescriptor<Symptom>();
            var container = query.Term("name", "head");
            container |= query.Term("name", "red");

            SearchDescriptor<Symptom> searchQuery = new SearchDescriptor<Symptom>();
            searchQuery.Index("mher");
            searchQuery.Query(x => container);

            return _client.Search<Symptom>(searchQuery);

        }
        
        public async Task<bool> UpdateAsync(string id, string name)
        {
            dynamic updateDoc = new System.Dynamic.ExpandoObject();
            updateDoc.name = name;

            var response = await _client.UpdateAsync<Symptom, object>(new DocumentPath<Symptom>(id), u => u.Index("mher").Doc(updateDoc));

            return response.IsValid;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var response = await _client.DeleteAsync<Symptom>(new DocumentPath<Symptom>(id).Index("mher"));

            return response.IsValid;
        }

    }

    public class Symptom
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
